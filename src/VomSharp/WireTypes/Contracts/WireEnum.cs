using VomSharp.WireTypes;

namespace VomSharp
{
    public class WireEnum : IWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public string[] Labels { get; set; }
    }
}