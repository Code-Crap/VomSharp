using VomSharp.WireTypes;
using TypeId = System.UInt64;

namespace VomSharp
{
    public class WireSet : ISequenceWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public TypeId Key { get; set; }

        public ulong SequenceType
        {
            get
            {
                return Key;
            }
        }
    }
}