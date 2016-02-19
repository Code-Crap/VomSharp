using System;

namespace VomSharp
{
    public class TypeDecoder
    {
        private readonly IPrimitiveDecoder mDecoder;

        public TypeDecoder(IPrimitiveDecoder decoder)
        {
            mDecoder = decoder;
        }

        public IWireType ReadWireType()
        {
            long index = mDecoder.ReadInt64();

            switch (index)
            {
                case 0:
                    return ReadWireNamed();
                case 1:
                    return ReadWireEnum();
                case 2:
                    return ReadWireArray();
                case 3:
                    return ReadWireList();
                case 4:
                    return ReadWireSet();
                case 5:
                    return ReadWireMap();
                case 6:
                    return ReadWireStruct();
                case 7:
                    return ReadWireUnion();
                case 8:
                    return ReadWireOptional();
            }

            throw new Exception();
        }

        private WireNamed ReadWireNamed()
        {
            WireNamed result = new WireNamed();

            result.Name = mDecoder.ReadString();
            result.Base = mDecoder.ReadUInt32();

            return result;
        }

        private WireEnum ReadWireEnum()
        {
            WireEnum result = new WireEnum();

            result.Name = mDecoder.ReadString();
            result.Labels = mDecoder.ReadStrings();

            return result;
        }

        private WireArray ReadWireArray()
        {
            WireArray result = new WireArray();

            result.Name = mDecoder.ReadString();
            result.Elem = mDecoder.ReadUInt64();
            result.Len = mDecoder.ReadUInt64();

            return result;
        }

        private WireList ReadWireList()
        {
            WireList result = new WireList();

            result.Name = mDecoder.ReadString();
            result.Elem = mDecoder.ReadUInt64();

            return result;
        }

        private WireSet ReadWireSet()
        {
            WireSet result = new WireSet();

            result.Name = mDecoder.ReadString();
            result.Key = mDecoder.ReadUInt64();

            return result;
        }

        private WireMap ReadWireMap()
        {
            WireMap result = new WireMap();

            result.Name = mDecoder.ReadString();
            result.Key = mDecoder.ReadUInt64();
            result.Elem = mDecoder.ReadUInt64();

            return result;
        }

        private WireStruct ReadWireStruct()
        {
            WireStruct result = new WireStruct();

            result.Name = mDecoder.ReadString();
            result.Fields = ReadFieldArray();

            return result;
        }

        private WireUnion ReadWireUnion()
        {
            WireUnion result = new WireUnion();

            result.Name = mDecoder.ReadString();
            result.Fields = ReadFieldArray();

            return result;
        }

        private WireOptional ReadWireOptional()
        {
            WireOptional result = new WireOptional();

            result.Name = mDecoder.ReadString();
            result.Elem = mDecoder.ReadUInt64();

            return result;
        }

        private WireField[] ReadFieldArray()
        {

            return new WireField[] {};
        }

        private WireField ReadWireField()
        {
            WireField result = new WireField();

            result.Name = mDecoder.ReadString();
            result.Type = mDecoder.ReadUInt64();

            return result;
        }
    }
}