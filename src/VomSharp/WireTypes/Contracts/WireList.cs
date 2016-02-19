using VomSharp.WireTypes;
using TypeId = System.UInt64;

namespace VomSharp
{
    public class WireList : ISequenceWireType
    {
        [VdlField(0)]
        public string Name { get; set; }
        [VdlField(1)]
        public TypeId Elem { get; set; }

        public ulong SequenceType
        {
            get
            {
                return Elem;
            }
        }
    }
}