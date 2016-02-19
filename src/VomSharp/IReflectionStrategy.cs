namespace VomSharp
{
    interface IReflectionStrategy
    {
        object ActivatePrimitive(object value);
        object ActivateCollection(ISequenceWireType type, IWireType sequenceType, long length);
        void SetCollectionValue(object instance, int index, object value);
        object ActivateMap(WireMap type, IWireType keyType, IWireType elementType, long length);
        void SetMapValue(object instance, object key, object value);
        object ActivateStruct(WireStruct type);
        void SetStructFieldValue(object instance, WireField field, object fieldValue);
        object ActivateUnion(WireUnion wireType, WireField field, object fieldValue);
        object ActivateNil();
    }
}