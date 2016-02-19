using System;

namespace VomSharp.WireTypes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class VdlFieldAttribute : Attribute
    {
        private readonly int mIndex;

        public VdlFieldAttribute(int index)
        {
            mIndex = index;
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