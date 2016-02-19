using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VomSharp
{
    class Decoder
    {
        #region Fields

        private readonly IPrimitiveDecoder mDecoder;
        private readonly IDictionary<long, IWireType> mTypeIdToWireType;
        private IReflectionStrategy mStrategy;
        private static readonly IDictionary<long, IWireType> mPredefinedTypeIdToWireType;

        #endregion

        #region Constructors

        static Decoder()
        {
            mPredefinedTypeIdToWireType =
                Enum.GetValues(typeof (PrimitiveType)).OfType<PrimitiveType>()
                    .ToDictionary(type => (long) type,
                                  type => (IWireType)
                                      new WireNamed
                                      {
                                          Base =
                                              (ulong) type,
                                          Name = type.ToString().ToLower()
                                      });

            mPredefinedTypeIdToWireType[(long)PrimitiveType.ByteList] =
                new WireList() {Name = "[]byte", Elem = (ulong)PrimitiveType.Byte};

            mPredefinedTypeIdToWireType[(long) PrimitiveType.StringList] =
                new WireList() {Name = "[]string", Elem = (ulong) PrimitiveType.String};
        }

        public Decoder(Stream stream)
        {
            mTypeIdToWireType = new Dictionary<long, IWireType>(mPredefinedTypeIdToWireType);
        }

        #endregion

        private IWireType LookupType(long typeId)
        {
            IWireType wireType;

            if (!mTypeIdToWireType.TryGetValue(typeId, out wireType))
            {
                throw new Exception("");
            }

            return wireType;
        }

        public void ReadHeader()
        {
            byte version = mDecoder.ReadByte();

            if (version != 0x80)
            {
                throw new Exception("Expected version 0x80");
            }
        }

        public object ReadNext()
        {
            long typeId = mDecoder.ReadInt64();

            if (typeId > 0)
            {
                return ReadValueMessage(typeId);
            }
            else
            {
                return ReadTypeMessage(-typeId);
            }
        }

        private object ReadTypeMessage(long typeId)
        {
            TypeDecoder decoder = new TypeDecoder(mDecoder);

            IWireType wireType = decoder.ReadWireType();

            mTypeIdToWireType[typeId] = wireType;

            return wireType;
        }

        private object ReadValueMessage(long typeId)
        {
            return ReadMainValue(typeId);
        }

        private object ReadMainValue(long typeId)
        {
            return ReadValue(typeId, true);
        }

        private object ReadCompositeValue(long typeId)
        {
            IWireType wireType = LookupType(typeId);

            return ReadComposite((dynamic) wireType);
        }

        #region Read Composite dynamic dispatch

        private object ReadComposite(ISequenceWireType wireType)
        {
            return ReadSequence(wireType);
        }

        private object ReadComposite(WireMap wireType)
        {
            return ReadMap(wireType);
        }

        private object ReadComposite(WireStruct wireType)
        {
            return ReadStruct(wireType);
        }

        private object ReadComposite(WireUnion wireType)
        {
            return ReadUnion(wireType);
        }

        private object ReadComposite(WireOptional wireType)
        {
            return ReadOptional(wireType);
        }

        private object ReadComposite(WireNamed wireType)
        {
            return ReadAny(wireType);
        }

        #endregion

        private object ReadAny(WireNamed wireType)
        {
            if ((PrimitiveType) wireType.Base != PrimitiveType.Any)
            {
                throw new Exception("");
            }

            if (mDecoder.PeekHeader() == ControlEntries.NIL)
            {
                return mStrategy.ActivateNil();
            }

            ulong typeId = mDecoder.ReadUInt64();

            return ReadValue((long) typeId, false);
        }

        private object ReadOptional(WireOptional wireType)
        {
            if (mDecoder.PeekHeader() == ControlEntries.NIL)
            {
                return mStrategy.ActivateNil();
            }

            return ReadValue((long) wireType.Elem, false);
        }

        private object ReadUnion(WireUnion wireType)
        {
            object instance = mStrategy.ActivateUnion(wireType);

            ulong index = mDecoder.ReadUInt64();

            if ((long) index >= wireType.Fields.Length)
            {
                throw new Exception("");
            }

            WireField field = wireType.Fields[index];

            ulong fieldType = field.Type;

            object fieldValue = ReadValue((long) fieldType, false);

            mStrategy.SetUnionFieldValue(instance, field, fieldValue);

            return instance;
        }

        private object ReadStruct(WireStruct wireType)
        {
            object instance = mStrategy.ActivateStruct(wireType);

            byte control = mDecoder.PeekHeader();

            while (control != ControlEntries.END)
            {
                ulong index = mDecoder.ReadUInt64();

                if ((long) index >= wireType.Fields.Length)
                {
                    throw new Exception("");
                }

                WireField field = wireType.Fields[index];

                ulong fieldType = field.Type;

                object fieldValue = ReadValue((long) fieldType, false);

                mStrategy.SetStructFieldValue(instance, field, fieldValue);

                control = mDecoder.PeekHeader();
            }

            return instance;
        }

        private object ReadMap(WireMap wireType)
        {
            long length = (long) mDecoder.ReadUInt64();

            IWireType keyType = LookupType((long) wireType.Key);
            IWireType elementType = LookupType((long) wireType.Elem);

            object instance = mStrategy.ActivateMap(wireType, keyType, elementType, length);

            for (int i = 0; i < length; i++)
            {
                object key = ReadValue((long) wireType.Key, false);
                object value = ReadValue((long) wireType.Elem, false);
                mStrategy.SetMapValue(instance, key, value);
            }

            return instance;
        }

        private object ReadSequence(ISequenceWireType wireType)
        {
            long length = (long) mDecoder.ReadUInt64();

            IWireType sequenceType = LookupType((long) wireType.SequenceType);

            object instance = mStrategy.ActivateCollection(wireType, sequenceType, length);

            for (int i = 0; i < length; i++)
            {
                object value = ReadSequenceItem(wireType);
                mStrategy.SetCollectionValue(instance, i, value);
            }

            return instance;
        }

        private object ReadSequenceItem(ISequenceWireType wireType)
        {
            return ReadValue((long) wireType.SequenceType, false);
        }

        private object ReadValue(long typeId, bool mainValue)
        {
            // Check if the type is a predefined type.
            if (IsPrimitiveValue(typeId))
            {
                return ReadPrimitive(typeId);
            }
            else
            {
                if (mainValue)
                {
                    // This value can be used to skip a message
                    ulong byteLength = mDecoder.ReadUInt64();
                }

                return ReadCompositeValue(typeId);
            }
        }

        private static bool IsPrimitiveValue(long typeId)
        {
            PrimitiveType casted = (PrimitiveType) typeId;
            return (casted >= PrimitiveType.Bool) &&
                   (casted <= PrimitiveType.Complex128);
        }

        private object ReadPrimitive(long typeId)
        {
            PrimitiveType type = (PrimitiveType) typeId;

            switch (type)
            {
                case PrimitiveType.Bool:
                    return mDecoder.ReadBoolean();
                case PrimitiveType.Byte:
                    return mDecoder.ReadByte();
                case PrimitiveType.UInt16:
                    return mDecoder.ReadUInt16();
                case PrimitiveType.UInt32:
                    return mDecoder.ReadUInt32();
                case PrimitiveType.UInt64:
                    return mDecoder.ReadUInt64();
                case PrimitiveType.Int16:
                    return mDecoder.ReadInt16();
                case PrimitiveType.Int32:
                    return mDecoder.ReadInt32();
                case PrimitiveType.Int64:
                    return mDecoder.ReadInt64();
                case PrimitiveType.Float32:
                    return mDecoder.ReadFloat32();
                case PrimitiveType.Float64:
                    return mDecoder.ReadFloat64();
                case PrimitiveType.Complex64:
                    return mDecoder.ReadComplex64();
                case PrimitiveType.Complex128:
                    return mDecoder.ReadComplex128();
            }

            return null;
        }
    }
}