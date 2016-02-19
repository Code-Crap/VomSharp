using VomSharp.WireTypes;
using TypeId = System.UInt64;

namespace VomSharp
{
    public class WireMap : IWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public TypeId Key { get; set; }
        [VdlField(2)]
        public TypeId Elem { get; set; }
    }
}