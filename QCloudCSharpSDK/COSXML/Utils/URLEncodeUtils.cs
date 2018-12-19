using System;
using System.Collections.Generic;

using System.Text;
using System.Globalization;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 5:40:47 PM
* bradyxiao
*/
namespace COSXML.Utils
{
    public sealed class URLEncodeUtils
    {
        //只有字母和数字[0-9a-zA-Z]、一些特殊符号"-_.!*~',以及某些保留字，才可以不经过编码直接用于URL。"
        public const string URLAllowChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.!*~";

        public static string EncodeURL(string path)
        {
            if (String.IsNullOrEmpty(path)) return String.Empty;
            char separator = '/';
            int start = 0, length = path.Length;
            int index = path.IndexOf(separator, start);
            StringBuilder result = new StringBuilder();
            while (index != -1 && start < length)
            {
                if (start != index)
                {
                    result.Append(Encode(path.Substring(start, index - start)));
                }
                result.Append(separator);
                start = index + 1;
                index = path.IndexOf(separator, start);
            }
            result.Append(Encode(path.Substring(start)));
            return result.ToString();
        }

        public static string Encode(string value)
        {
            return Encode(value, Encoding.UTF8);
        }

        /// <summary>
        /// urlEncode: 转为一个byte -> 转为两个16进制 -> 前面加上 %
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encode(string value, Encoding encoding)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;
            StringBuilder result = new StringBuilder(value.Length * 2); // %xy%xy
            byte[] strToBytes = encoding.GetBytes(value);
            foreach(byte b in strToBytes)
            {
                char ch = (char)b;
                if(URLAllowChars.IndexOf(ch) != -1)
                {
                    result.Append(ch);
                }
                else
                {
                    result.Append('%').Append(String.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)b));
                }
            }
            return result.ToString();
        }
    }
}
