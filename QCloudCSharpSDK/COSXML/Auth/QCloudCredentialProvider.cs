using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Utils;
using COSXML.CosException;
using COSXML.Common;
using COSXML.Network;
using COSXML.Log;
using System.IO;
using Newtonsoft.Json;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/9/2018 12:16:20 PM
* bradyxiao
*/
namespace COSXML.Auth
{

    public abstract class QCloudCredentialProvider
    {
        public abstract QCloudCredentials GetQCloudCredentials();

        public abstract void Refresh();
    }

    /// <summary>
    /// 通过 云 api
    /// </summary>
    public class DefaultQCloudCredentialProvider : QCloudCredentialProvider
    {
        private string secretId;

        private string secretKey;

        private string keyTime;

        public DefaultQCloudCredentialProvider(string secretId, string secretKey, long keyDurationSecond)
            : this(secretId, secretKey, TimeUtils.GetCurrentTime(TimeUnit.SECONDS), keyDurationSecond)
        {}

        public DefaultQCloudCredentialProvider(string secretId, string secretKey, long keyStartTimeSecond, long keyDurationSecond)
        {
            this.secretId = secretId;
            this.secretKey = secretKey;
            long keyEndTime = keyStartTimeSecond + keyDurationSecond;
            this.keyTime = String.Format("{0};{1}", keyStartTimeSecond, keyEndTime);
        }

        public override QCloudCredentials GetQCloudCredentials()
        {
            if (IsNeedUpdateNow()) Refresh();
            if (secretId == null) throw new CosClientException((int)CosClientError.INVALID_CREDENTIALS, "secretId == null");
            if (secretKey == null) throw new CosClientException((int)CosClientError.INVALID_CREDENTIALS, "secretKey == null");
            if (keyTime == null) throw new CosClientException((int)CosClientError.INVALID_CREDENTIALS, "keyTime == null");
            string signKey = DigestUtils.GetHamcSha1ToHexString(keyTime, Encoding.UTF8, secretKey, Encoding.UTF8);
            return new QCloudCredentials(secretId, signKey, keyTime);
        }

        public override void Refresh()
        {
            //TODO update value
            QLog.D("DefaultQCloudCredentialProvider", "need to update QCloudCredentials");
        }

        public bool IsNeedUpdateNow() 
        {
            if (String.IsNullOrEmpty(keyTime) || String.IsNullOrEmpty(secretId) || String.IsNullOrEmpty(secretKey))
            {
                return true;
            }
            int index = keyTime.IndexOf(';');
            long endTime = -1L;
            long.TryParse(keyTime.Substring(index + 1), out endTime);
            long nowTime = TimeUtils.GetCurrentTime(TimeUnit.SECONDS);
            if (endTime <= nowTime) return true;
            return false;
        }
    }

    /// <summary>
    /// 通过 临时密钥
    /// </summary>
    public class DefaultSessionQCloudCredentialProvider : QCloudCredentialProvider
    {
        private string tmpSecretId;
        private string tmpSecretKey;
        private string keyTime;
        private string token;

        public DefaultSessionQCloudCredentialProvider() { }

        public override QCloudCredentials GetQCloudCredentials()
        {
            if (IsNeedUpdateNow()) Refresh();
            if (tmpSecretId == null) throw new CosClientException((int)CosClientError.INVALID_CREDENTIALS, "secretId == null");
            if (tmpSecretKey == null) throw new CosClientException((int)CosClientError.INVALID_CREDENTIALS, "secretKey == null");
            if (keyTime == null) throw new CosClientException((int)CosClientError.INVALID_CREDENTIALS, "keyTime == null");
            string signKey = DigestUtils.GetHamcSha1ToHexString(keyTime, Encoding.UTF8, tmpSecretKey, Encoding.UTF8);
            return new SessionQCloudCredentials(tmpSecretId, signKey, token, keyTime);
        }

        public override void Refresh()
        {
            //TODO update value
            QLog.D("DefaultSessionQCloudCredentialProvider", "need to update QCloudCredentials");
        }

        public bool IsNeedUpdateNow()
        {
            if (String.IsNullOrEmpty(keyTime) || String.IsNullOrEmpty(tmpSecretId) || String.IsNullOrEmpty(tmpSecretKey) || String.IsNullOrEmpty(token))
            {
                return true;
            }
            int index = keyTime.IndexOf(';');
            long endTime = -1L;
            long.TryParse(keyTime.Substring(index + 1), out endTime);
            long nowTime = TimeUtils.GetCurrentTime(TimeUnit.SECONDS);
            if (endTime <= nowTime) return true;
            return false;
        }

        /// <summary>
        /// 第一种方式：直接复制临时密钥信息
        /// </summary>
        /// <param name="tmpSecretId">临时安全证书 Id</param>
        /// <param name="tmpSecretKey">临时安全证书 Key</param>
        /// <param name="tmpExpiredTime">证书无效的时间，返回 Unix 时间戳，精确到秒</param>
        /// <param name="sessionToken">token 值</param>
        public void SetQCloudCredential(string tmpSecretId, string tmpSecretKey, long tmpExpiredTime, string sessionToken)
        {
            this.tmpSecretId = tmpSecretId;
            this.tmpSecretKey = tmpSecretKey;
            this.token = sessionToken;
            this.keyTime = String.Format("{0};{1}", TimeUtils.GetCurrentTime(TimeUnit.SECONDS), tmpExpiredTime);
        }

        /// <summary>
        /// 第二种方式：去请求cam获取临时密钥信息
        /// </summary>
        public void SetQCloudCredential(string secretId, string secretKey)
        {
            CAMRequest cAMRequest = new CAMRequest(secretId, secretKey);
            Request request = cAMRequest.BuildRequest();
            CAMResult camResult = new CAMResult();
            CAMResponse response = new CAMResponse(camResult);

            HttpClient.GetInstance().excute(request, response);

            this.tmpSecretId = camResult.tmpSecretId;
            this.tmpSecretKey = camResult.tmpSecretKey;
            this.token = camResult.sessionToken;
            this.keyTime = String.Format("{0};{1}", TimeUtils.GetCurrentTime(TimeUnit.SECONDS), camResult.keyEndTime);

            QLog.D("DefaultSessionQCloudCredentialProvider", String.Format("id ={0}, key ={1}, keyTime = {2}",tmpSecretId, tmpSecretKey, keyTime));

        }

        /// <summary>
        /// 第三种方式：去请求第三方搭建的临时密钥服务器
        /// </summary>
        /// <param name="url"></param>
        public void SetQCloudCredential(string method, bool isHttps, HttpUrl httpUrl)
        {
            Request request = new Request();
            request.Method = method;
            request.IsHttps = isHttps;
            request.Url = httpUrl;
            request.Body = null;

            CAMResult camResult = new CAMResult();
            CAMResponse response = new CAMResponse(camResult);
            HttpClient.GetInstance().excute(request, response);
            this.tmpSecretId = camResult.tmpSecretId;
            this.tmpSecretKey = camResult.tmpSecretKey;
            this.token = camResult.sessionToken;
            this.keyTime = String.Format("{0};{1}", TimeUtils.GetCurrentTime(TimeUnit.SECONDS), camResult.keyEndTime);
        }

        internal class CAMResponse : Response
        {
            public CAMResponse(CAMResult cmaResult)
            {
                this.Body = new ResponseBody();
                this.Body.ParseStream = cmaResult.ParseResponseBody;
            }
            
        }

        public void TestCAMResult()
        {
            string content = "{\"code\": 0,\"message\": \"\",\"codeDesc\": \"Success\",\"data\": " +
                "{\"credentials\": {\"sessionToken\": \"5e776c4216ff4d31a7c74fe194a978a3ff2a42864\"," +
                "\"tmpSecretId\": \"AKIDcAZnqgar9ByWq6m7ucIn8LNEuY2MkPCl\"," +
                "\"tmpSecretKey\": \"VpxrX0IMCpHXWL0Wr3KQNCqJix1uhMqD\"}," +
                "\"expiredTime\": 1506433269," +
                "\"expiration\": \"2017-09-26T13:41:09Z\"}}";
            byte[] data = Encoding.UTF8.GetBytes(content);
            MemoryStream memoryStream = new MemoryStream(data);
            CAMResult cAMResult = new CAMResult();
            cAMResult.ParseResponseBody(memoryStream, "application/xml", data.LongLength);
            memoryStream.Close();
            data = null;
            if (StringUtils.Compare(cAMResult.sessionToken, "5e776c4216ff4d31a7c74fe194a978a3ff2a42864", false) != 0)
            {
                throw new Exception("cam parse error");
            }
            if (StringUtils.Compare(cAMResult.tmpSecretId, "AKIDcAZnqgar9ByWq6m7ucIn8LNEuY2MkPCl", false) != 0)
            {
                throw new Exception("cam parse error");
            }
            if (StringUtils.Compare(cAMResult.tmpSecretKey, "VpxrX0IMCpHXWL0Wr3KQNCqJix1uhMqD", false) != 0)
            {
                throw new Exception("cam parse error");
            }

        }
        internal class CAMResult
        {
            public string tmpSecretId, tmpSecretKey, sessionToken, keyEndTime;
            public void ParseResponseBody(Stream inputStream, string contentType, long contentLength) 
            {
                StringBuilder contentBuilder = new StringBuilder();
                StreamReader streamReader = new StreamReader(inputStream);
                string line = streamReader.ReadLine();
                while (line != null)
                {
                    contentBuilder.Append(line);
                    line = streamReader.ReadLine();
                }
                string content = contentBuilder.ToString();
                QLog.D("XIAO", content);
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(content));
                Dictionary<string, string> result = new Dictionary<string, string>();
                string key = null;
                string value = null;
                while (jsonTextReader.Read())
                {
                    switch(jsonTextReader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            key = (string)jsonTextReader.Value;
                            break;
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.String:
                        case JsonToken.Boolean:
                            value = jsonTextReader.Value.ToString();
                            result.Add(key, value);
                            break;
                        }
                }
                jsonTextReader.Close();
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if ("sessionToken".Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        this.sessionToken = pair.Value;
                    }
                    else if ("tmpSecretId".Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        this.tmpSecretId = pair.Value;
                    }
                    else if ("tmpSecretKey".Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        this.tmpSecretKey = pair.Value;
                    }
                    else if ("expiredTime".Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        this.keyEndTime = pair.Value;
                    }
                    else if ("code".Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        if ( !"0".Equals(pair.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            QLog.E("XIAO", "get acm error");
                            break;
                        }
                    }
                }
               
            }

        }

        internal class CAMRequest
        {
            private string policy = "{\"statement\": [{\"action\": [\"name/cos:*\"],\"effect\": \"allow\"," +
                "\"resource\":[\"qcs::cos:gz:uid/1253960454:prefix//1253960454/*\"]}],\"version\": \"2.0\"}";

            private string secretId;
            private string secretKey;

            public CAMRequest(string secretId, string secretKey)
            {
                this.secretId = secretId;
                this.secretKey = secretKey;
            }

            public Request BuildRequest()
            {
                string camHost = "sts.api.qcloud.com";
                string camPath = "/v2/index.php";
                string camMethod = "GET";
                bool isHttps = true;
                Dictionary<string, string> queryParameters = new Dictionary<string, string>();
                queryParameters.Add("policy", policy);
                queryParameters.Add("name", "brady");
                queryParameters.Add("Action", "GetFederationToken");
                queryParameters.Add("SecretId", secretId);
                queryParameters.Add("Nonce", new Random().Next(1, int.MaxValue).ToString());
                queryParameters.Add("Timestamp", TimeUtils.GetCurrentTime(TimeUnit.SECONDS).ToString());
                queryParameters.Add("RequestClient", "net-sdk-v5");
                queryParameters.Add("durationSeconds", 7200.ToString());
                string plainText = MakeSignPlainText(queryParameters, camMethod, camHost, camPath);
                string hamcSha1 = DigestUtils.GetHamcSha1ToBase64(plainText, Encoding.UTF8, secretKey, Encoding.UTF8);
                queryParameters.Add("Signature", hamcSha1);

                HttpUrl httpUrl = new HttpUrl();
                httpUrl.Scheme = isHttps ? "https" : "http";
                httpUrl.Host = camHost;
                httpUrl.Path = camPath;
                Dictionary<string, string> tmp = new Dictionary<string, string>(queryParameters.Count);
                foreach (KeyValuePair<string, string> pair in queryParameters)
                {
                    tmp.Add(pair.Key, URLEncodeUtils.Encode(pair.Value).Replace("%20", "+"));
                }
                queryParameters.Clear();
                httpUrl.SetQueryParameters(tmp);
                
                Request request = new Request();
                request.Method = camMethod;
                request.IsHttps = isHttps;
                request.Url = httpUrl;

                request.Body = null;
                return request;
            }

            public string Policy
            {
                get { return policy; }
                set { policy = value; }
            }

            private string MakeSignPlainText(Dictionary<string, string> requestParameters, string requestMethod,
                string requestHost, string requestPath)
            {
                StringBuilder result = new StringBuilder();
                //排序
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(requestParameters);

                list.Sort(delegate (KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
                {
                    return StringUtils.Compare(s1.Key, s2.Key, false);
                });

                result.Append(requestMethod).Append(requestHost).Append(requestPath);
                bool isFirst = true;
                foreach (KeyValuePair<string, string> pair in list)
                {
                    if (isFirst)
                    {
                        result.Append('?');
                        isFirst = false;
                    }
                    else
                    {
                        result.Append('&');
                    }
                    result.Append(pair.Key.Replace('_', '.')).Append('=').Append(pair.Value);
                }
                return result.ToString();
            }
        }
    }
}
