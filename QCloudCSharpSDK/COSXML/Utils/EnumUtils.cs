using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Utils
{
    public static class EnumUtils
    {
        public static string GetValue(Enum value)
        {

            if (value == null)
            {

                return null;
            }

            string name = value.ToString();

            var fieldInfo = value.GetType().GetField(name);

            var attributes = fieldInfo.GetCustomAttributes(typeof(CosValueAttribute), false);

            return attributes != null && attributes.Length > 0 ? ((CosValueAttribute)attributes[0]).Value : name;
        }
    }
}
