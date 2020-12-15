using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace COSXML.Network
{
    public class Response
    {

        public int Code { get; set; }

        public string Message { get; set; }

        public Dictionary<string, List<string>> Headers { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public ResponseBody Body { get; set; }

        public virtual void HandleResponseHeader() 
        { 

        }

        /// <summary>
        /// handle body successfully or throw exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isSuccess"></param>
        public virtual void OnFinish(bool isSuccess, Exception ex) 
        { 
            
        }

    }

}
