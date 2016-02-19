using VomSharp.WireTypes;

namespace VomSharp
{
    [VdlUnionMapping(0, typeof(WireNamed))]
    [VdlUnionMapping(1, typeof(WireEnum))]
    [VdlUnionMapping(2, typeof(WireArray))]
    [VdlUnionMapping(3, typeof(WireList))]
    [VdlUnionMapping(4, typeof(WireSet))]
    [VdlUnionMapping(5, typeof(WireMap))]
    [VdlUnionMapping(6, typeof(WireStruct))]
    [VdlUnionMapping(7, typeof(WireUnion))]
    [VdlUnionMapping(8, typeof(WireOptional))]
    public interface IWireType : IMainValue
    {
    }
}