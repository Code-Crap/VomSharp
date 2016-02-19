using TypeId = System.UInt64;

namespace VomSharp
{
    public interface ISequenceWireType : IWireType
    {
        TypeId SequenceType { get; }
    }
}