using System;
using System.Collections.Generic;

using System.Text;
using System.Globalization;

namespace COSXML.Utils
{
    
    /// <summary>
    /// * URL Encode: 
    ///  * 可分为绝对不需要编码的字符:[a~z][A~A][0~9][._-~] 
    ///  * 特殊字符需要视情况而言:!*'();:@&amp;=+$,/?#[]
    ///  * 空字符用+或者%20代替
    ///  * 汉字绝地需要编码
    ///  * 因此，针对完整URL则要分块进行编码：path块，query块，fragment块，host块
    ///  * path块，需要以'/'进行分割，即 '/' 不需要编码，其它均需要编码
    ///  * query块，需要以'&amp;'分割一对对key=value；key 和 value 需要编码, "="不编码
    ///  * fragment，全部进行编码
    ///  * URL Decode:
    ///  * 不需要判断是否是特殊字符，因为其解码规则很简单，直接根据内容中是否出现%来判断是否需要解码，
    ///  * 还是直接输出：若出现了%，则连续读出其后两位进行解码；反之，直接输出
    /// </summary>
    public sealed class URLEncodeUtils
    {
        //只有字母和数字[0-9a-zA-Z]、一些特殊符号"-_.!*~',以及某些保留字，才可以不经过编码直接用于URL。"
        public const string URLAllowChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// 针对 URL 中 path 编码，则需要先将其按照 '/'分割，然后进行逐个块进行 value 编码
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string EncodePathOfURL(string path)
        {

            if (String.IsNullOrEmpty(path))
            {

                return String.Empty;
            }

            char separator = '/';
            int start = 0;
            int length = path.Length;

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

            if (start < length)
            {
                result.Append(Encode(path.Substring(start)));
            }

            return result.ToString();
        }

        public static string Encode(string value)
        {

            return Encode(value, Encoding.UTF8);
        }

        /// <summary>
        /// 针对单个value，则只需满足 URLAllowChars 不需要编码即可.
        /// urlEncode: 转为一个byte -> 转为两个16进制 -> 前面加上 %
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encode(string value, Encoding encoding)
        {

            if (String.IsNullOrEmpty(value))
            {

                return String.Empty;
            }
            // %xy%xy
            // %xy%xy
            StringBuilder result = new StringBuilder(value.Length * 2);
            byte[] strToBytes = encoding.GetBytes(value);

            foreach (byte b in strToBytes)
            {
                char ch = (char)b;

                if (URLAllowChars.IndexOf(ch) != -1)
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

        /// <summary>
        /// 解码比较统一，此处借用 Uri 来实现
        /// </summary>
        /// <param name="valueEncode"></param>
        /// <returns></returns>
        public static string Decode(string valueEncode)
        {

            if (String.IsNullOrEmpty(valueEncode))
            {

                return String.Empty;
            }

            valueEncode.Replace('+', ' ');

            return Uri.UnescapeDataString(valueEncode);
        }
    }
}
