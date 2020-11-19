using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Utils
{
    public sealed class CosValueAttribute : Attribute
    {
        public string Value
        {
            get;
            private set;
        }

        public CosValueAttribute(string value)
        {
            this.Value = value;
        }
    }
}
