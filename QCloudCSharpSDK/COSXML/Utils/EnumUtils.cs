using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/1/2018 8:51:37 PM
* bradyxiao
*/
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
