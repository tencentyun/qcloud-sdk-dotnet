using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/6/2018 11:37:45 AM
* bradyxiao
*/
namespace COSXML.Network
{
    /**
     * //最基本的划分
        [scheme:]scheme-specific-part[#fragment]  
        //对scheme-specific-part进一步划分
        [scheme:][//authority][path][?query][#fragment]  
        //对authority再次划分, 这是最细分的结构
        [scheme:][//host:port][path][?query][#fragment]
     */
    public sealed class HttpUrl
    {

        private string scheme;

        private string userName = "";

        private string userPwd = "";

        private string host;

        private int port = -1;

        private string path = "/";

        private Dictionary<string, string> queryParameters;

        private string fragment;



        public HttpUrl()
        {
            this.queryParameters = new Dictionary<string, string>();
        }

        public string Scheme
        {
            set
            {
                if (value == null) throw new ArgumentNullException("scheme == null");
                if (value.Equals("http", StringComparison.OrdinalIgnoreCase))
                {
                    this.scheme = "http";
                }
                else if (value.Equals("https", StringComparison.OrdinalIgnoreCase))
                {
                    this.scheme = "https";
                }
                else
                {
                    throw new ArgumentException("unexpected scheme: " + scheme);
                }
            }
            get{return this.scheme;}
            
        }

        public string UserName
        {
            set
            {
                if (value == null) throw new ArgumentNullException("userName == null");
                this.userName = value;
            }
            get{return this.userName;}
        }

        public string UserPassword
        {
             set
            {
                if (value == null) throw new ArgumentNullException("userPwd == null");
                this.userPwd = value;
            }
            get {return this.userPwd;}
        }

        public string Host
        {
            set
            {
                if (value == null) throw new ArgumentNullException("host == null");
                this.host = value;
            }
            get { return this.host; }
        }

        public int Port
        {
            set
            {
                if (value <= 0 || value >= 65535) throw new ArgumentException("unexpected port: " + port);
                this.port = value;
            }
            get { return this.port; }
        }

        public string Path
        {
            set
            {
                if (value != null)
                {
                    this.path = value; // need url encode
                }
            }
            get { return path; }
        }

        public void SetQueryParameters(Dictionary<string, string> queryParameters)
        {
            if (queryParameters != null)
            {
                foreach (KeyValuePair<string, string> pair in queryParameters)
                {
                    this.queryParameters.Add(pair.Key, pair.Value);
                }
                
            }
        }

        public Dictionary<string, string> GetQueryParameters()
        {
            return queryParameters;
        }

        public string Fragment
        {
            set
            {
                this.fragment = value;
            }
            get { return this.fragment; }
            
        }

        public override string ToString()
        {

            //if (scheme == null) throw new ArgumentNullException("scheme == null");
            //if (host == null) throw new ArgumentNullException("host == null");

            StringBuilder url = new StringBuilder();

            url.Append(scheme)
                .Append("://");

            if (userName != String.Empty || userPwd != String.Empty)
            {
                url.Append(userName);
                if (userPwd != String.Empty)
                {
                    url.Append(':')
                        .Append(userPwd);
                }
                url.Append('@');
            }

            if (host.IndexOf(':') != -1)
            {
                url.Append('[')
                    .Append(host)
                    .Append(']');
            }
            else
            {
                url.Append(host);
            }

            int effectivePort = EffecivePort();
            if (effectivePort != DefaultPort(scheme))
            {
                url.Append(':')
                    .Append(effectivePort);
            }

            url.Append(path);

            StringBuilder query = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in queryParameters)
            {
                query.Append(pair.Key);
                if (!String.IsNullOrEmpty(pair.Value))
                {
                    query.Append('=').Append(pair.Value);
                }
                query.Append('&');
            }
            string queryString = query.ToString();
            if (queryString.EndsWith("&"))
            {
                queryString = queryString.Remove(queryString.Length - 1);
                url.Append('?');
                url.Append(queryString);
            }

            if (fragment != null)
            {
                url.Append('#')
                    .Append(fragment);
            }

            return url.ToString();

        }

        public int EffecivePort()
        {
            return port != -1 ? port : DefaultPort(scheme);
        }

        private int DefaultPort(string scheme)
        {
            if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
            {
                return 80;
            }
            else if (scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                return 443;
            }
            else
            {
                return -1;
            }
        }

        //private int DelimiterOffset(string input, int pos, int limit, char delimiter)
        //{
        //    for (int i = pos; i < limit; i++)
        //    {
        //        if (input[i] == delimiter) return i;
        //    }
        //    return limit;
        //}

        //private int DelimiterOffset(string input, int pos, int limit, string delimiters)
        //{
        //    for (int i = pos; i < limit; i++)
        //    {
        //        if (delimiters.IndexOf(input[i]) != -1) return i;
        //    }
        //    return limit;
        //}

        //private void PathSegmentToString(StringBuilder outPut, List<string> pathSegments)
        //{
        //    foreach (string path in pathSegments)
        //    {
        //        outPut.Append('/').Append(path);
        //    }
        //}

        //private void NamesAndValuesToQueryString(StringBuilder outPut, List<string> namesAndValues)
        //{
        //    for (int i = 0, size = namesAndValues.Count; i < size; i += 2)
        //    {
        //        string name = namesAndValues[i];
        //        string value = namesAndValues[i + 1];
        //        if (i > 0) outPut.Append('&');
        //        outPut.Append(name);
        //        if (value != null)
        //        {
        //            outPut.Append('=');
        //            outPut.Append(value);
        //        }
        //    }
        //}
    }

}
