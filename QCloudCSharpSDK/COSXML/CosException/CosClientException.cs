using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 11:26:53 AM
* bradyxiao
*/
namespace COSXML.CosException
{
    /// <summary>
    /// cosClientException for parametes in cos request.
    /// </summary>
    [Serializable]
    public class CosClientException : System.ApplicationException
    {
        /// <summary>
        /// errorCode is for client exception code.
        /// <see cref="Common.CosClientError"/>
        /// </summary>
        public int errorCode;

        public CosClientException(int errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        public CosClientException(int errorCode, string message, Exception cause) 
            :base(message, cause)
        {
            this.errorCode = errorCode;
        }
    }
}
