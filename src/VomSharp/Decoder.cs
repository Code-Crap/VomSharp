using System;
using System.IO;

namespace VomSharp
{


    class Decoder
    {
        private readonly Stream mStream;
        private readonly BinaryReader mBinaryReader;

        public Decoder(Stream stream)
        {
            mStream = stream;
            mBinaryReader = new BinaryReader(stream);
        }

        public IMainValue[] ReadMessages()
        {
            byte version = mBinaryReader.ReadByte();

            if (version != 0x80)
            {
                throw new Exception("Expected version 0x80");
            }

            return null;
        }
    }
}