using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Auth
{
    public class QCloudCredentials
    {
        public QCloudCredentials(string secretId, string signKey, string keyTime)
        {
            this.SecretId = secretId;
            this.SignKey = signKey;
            this.KeyTime = keyTime;
        }

        public string SecretId
        { get; private set; }

        public string SignKey
        { get; private set; }

        public string KeyTime
        { get; private set; }

    }

    public class SessionQCloudCredentials : QCloudCredentials
    {
        public SessionQCloudCredentials(string secretId, string signKey, string token, string keyTime) :
            base(secretId, signKey, keyTime)
        {
            this.Token = token;

        }

        public string Token
        { get; private set; }
    }
}
