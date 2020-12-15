using System;
using System.Collections.Generic;
using System.Text;
using COSXML.CosException;
using COSXML.Common;

namespace COSXML.Utils
{
    public sealed class StringUtils
    {
        public static int Compare(string strA, string strB, bool ignoreCase)
        {

            if (strA == null || strB == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "strA = null or strA = null");
            }

            if (ignoreCase)
            {
                strA = strA.ToLower();
                strB = strB.ToLower();
            }

            int strALen = strA.Length;
            int strBLen = strB.Length;


            for (int i = 0, size = strALen > strBLen ? strBLen : strALen; i < size; i++)
            {
                int temp1 = (int)strA[i];
                int temp2 = (int)strB[i];

                if (temp1 > temp2)
                {

                    return 1;
                }
                else
if (temp1 < temp2)
                {

                    return -1;
                }

            }

            if (strALen > strBLen)
            {

                return 1;
            }

            if (strALen < strBLen)
            {

                return -1;
            }

            return 0;
        }

        // public static Dictionary<string, string> ParseURL(string url)
        // {
        //     Dictionary<string, string> urlTuple = new Dictionary<string, string>();

        //     if (String.IsNullOrEmpty(url))
        //     {

        //         return null;
        //     }

        //     int index = url.IndexOf("://");

        //     if (index > 0)
        //     {
        //         urlTuple.Add("Scheme", url.Substring(0, index));
        //     }
        //     else
        //     {
        //         throw new ArgumentException("url need start with http:// or https://");
        //     }

        //     int tmp = index;

        //     index = url.IndexOf('/', tmp + 3);

        //     if (index > 0)
        //     {
        //         urlTuple.Add("Host", url.Substring(tmp + 3, index - tmp - 3));
        //         tmp = index;
        //     }
        //     else
        //     {
        //         urlTuple.Add("Host", url.Substring(tmp + 3));

        //         return urlTuple;
        //     }

        //     index = url.IndexOf('?', tmp);

        //     if (index > 0)
        //     {
        //         urlTuple.Add("Path", url.Substring(tmp, index - tmp));
        //         tmp = index;
        //     }
        //     else
        //     {
        //         urlTuple.Add("Path", url.Substring(tmp));

        //         return urlTuple;
        //     }

        //     index = url.IndexOf("#", tmp + 1);

        //     if (index > 0)
        //     {
        //         urlTuple.Add("Query", url.Substring(tmp + 1, index - tmp - 1));
        //         tmp = index;
        //     }
        //     else
        //     {
        //         urlTuple.Add("Query", url.Substring(tmp + 1));

        //         return urlTuple;
        //     }

        //     urlTuple.Add("Fragment", url.Substring(tmp + 1));

        //     return urlTuple;
        // }
    }
}
