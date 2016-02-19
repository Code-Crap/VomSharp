using System;

namespace VomSharp.WireTypes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    internal sealed class VdlUnionMappingAttribute : Attribute
    {
        private readonly int mIndex;
        private readonly Type mImplementationType;

        public VdlUnionMappingAttribute(int index, Type implementationType)
        {
            mIndex = index;
            mImplementationType = implementationType;
        }

        public Type ImplementationType
        {
            get
            {
                return mImplementationType;
            }
        }

        public int Index
        {
            get
            {
                return mIndex;
            }
        }
    }
}