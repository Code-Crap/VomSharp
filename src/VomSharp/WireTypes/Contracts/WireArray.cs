using TypeId = System.UInt64;
using VomSharp.WireTypes;

namespace VomSharp
{
    public class WireArray : IWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public TypeId Elem { get; set; }
        [VdlField(2)]
        public ulong Len { get; set; }
    }
}