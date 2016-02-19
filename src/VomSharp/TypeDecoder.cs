using System;

namespace VomSharp
{
    public class TypeDecoder
    {
        private const ulong END = ControlEntries.END;

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

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Base = mDecoder.ReadUInt32();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireEnum ReadWireEnum()
        {
            WireEnum result = new WireEnum();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Labels = mDecoder.ReadStrings();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireArray ReadWireArray()
        {
            WireArray result = new WireArray();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Elem = mDecoder.ReadUInt64();
                        break;
                    case 2:
                        result.Len = mDecoder.ReadUInt64();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireList ReadWireList()
        {
            WireList result = new WireList();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Elem = mDecoder.ReadUInt64();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireSet ReadWireSet()
        {
            WireSet result = new WireSet();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Key = mDecoder.ReadUInt64();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireMap ReadWireMap()
        {
            WireMap result = new WireMap();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Key = mDecoder.ReadUInt64();
                        break;
                    case 2:
                        result.Elem = mDecoder.ReadUInt64();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireStruct ReadWireStruct()
        {
            WireStruct result = new WireStruct();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Fields = ReadFieldArray();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireUnion ReadWireUnion()
        {
            WireUnion result = new WireUnion();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Fields = ReadFieldArray();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireOptional ReadWireOptional()
        {
            WireOptional result = new WireOptional();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Elem = mDecoder.ReadUInt64();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }

        private WireField[] ReadFieldArray()
        {
            long arrayLength = mDecoder.ReadInt64();

            WireField[] result = new WireField[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
                result[i] = ReadWireField();
            }

            return result;
        }

        private WireField ReadWireField()
        {
            WireField result = new WireField();

            byte header = mDecoder.PeekHeader();

            while (header != END)
            {
                ulong index = mDecoder.ReadUInt64();

                switch (index)
                {
                    case 0:
                        result.Name = mDecoder.ReadString();
                        break;
                    case 1:
                        result.Type = mDecoder.ReadUInt64();
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                header = mDecoder.PeekHeader();
            }

            return result;
        }
    }
}