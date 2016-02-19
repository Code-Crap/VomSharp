using TypeId = System.UInt64;
using System.Numerics;

namespace VomSharp
{
    public enum PrimitiveType : long
    {
        Bool = 1,
        //Boolean = Bool,
        Byte = 2,
        String = 3,
        UInt16 = 4,
        UInt32 = 5,
        UInt64 = 6,
        Int16 = 7,
        Int32 = 8,
        Int64 = 9,
        Float32 = 10,
        //Single = Float32,
        Float64 = 11,
        //Double = Float64,
        Complex64 = 12,
        Complex128 = 13,
        //Complex = Complex128,
        TypeObject = 14,
        Any = 15,
        ByteArray = 39,
        StringArray = 40
    }


    public interface IPrimitiveDecoder
    {
        byte PeekHeader();

        bool ReadBoolean();
        byte ReadByte();
        string ReadString();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        float ReadFloat32();
        double ReadFloat64();
        Complex ReadComplex64();
        Complex ReadComplex128();
        byte[] ReadBytes();
        string[] ReadStrings();
    }


    public interface IPrimitiveEncoder
    {
        void Write(bool value);
        void Write(byte value);
        void Write(string value);
        void Write(ushort value);
        void Write(uint value);
        void Write(ulong value);
        void Write(short value);
        void Write(int value);
        void Write(long value);
        void Write(float value);
        void Write(double value);
        void Write(Complex value);
        void Write(byte[] value);
        void Write(string[] value);
    }
}