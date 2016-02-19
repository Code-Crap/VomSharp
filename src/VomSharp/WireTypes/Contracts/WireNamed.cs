using VomSharp.WireTypes;
using TypeID = System.UInt64;

namespace VomSharp
{
    public class WireNamed : IWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public TypeID Base { get; set; }
    }
}