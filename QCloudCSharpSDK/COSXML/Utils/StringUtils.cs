using System;
using System.Collections.Generic;
using System.Text;
using COSXML.CosException;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/23/2018 2:57:33 PM
* bradyxiao
*/
namespace COSXML.Utils
{
    public sealed class StringUtils
    {
        public static int Compare(string strA, string strB, bool ignoreCase)
        {
            if (strA == null || strB == null) throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "strA = null or strA = null");
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
                else if (temp1 < temp2)
                {
                    return -1;
                }

            }
            if (strALen > strBLen) return 1;
            if (strALen < strBLen) return -1;
            return 0;
        }
    }
}
