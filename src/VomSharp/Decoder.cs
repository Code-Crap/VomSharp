using System;
using System.Collections.Generic;
using System.IO;

namespace VomSharp
{
    class Decoder
    {
        private readonly Stream mStream;
        private readonly BinaryReader mBinaryReader;
        private readonly IPrimitiveDecoder mDecoder;
        private readonly IDictionary<long, IWireType> mTypeIdToWireType; 

        public Decoder(Stream stream)
        {
            mStream = stream;
            mBinaryReader = new BinaryReader(stream);
        }

        public IMainValue[] ReadMessages()
        {
            byte version = mDecoder.ReadByte();

            if (version != 0x80)
            {
                throw new Exception("Expected version 0x80");
            }

            long typeId = mDecoder.ReadInt64();

            if (typeId > 0)
            {
                IMainValue mainValue = ReadValueMessage(typeId);
            }
            else
            {
                ReadTypeMessage(-typeId);
            }

            return null;
        }

        private void ReadTypeMessage(long typeId)
        {
            TypeDecoder decoder = new TypeDecoder(mDecoder);

            IWireType wireType = decoder.ReadWireType();

            mTypeIdToWireType[typeId] = wireType;
        }

        private IMainValue ReadValueMessage(long typeId)
        {
            return null;
        }
    }
}