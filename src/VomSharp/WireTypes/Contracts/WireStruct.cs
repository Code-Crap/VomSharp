using VomSharp.WireTypes;

namespace VomSharp
{
    public class WireStruct : IWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public WireField[] Fields { get; set; }
    }
}