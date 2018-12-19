using System;
using System.Collections.Generic;
using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/9/2018 12:09:40 PM
* bradyxiao
*/
namespace COSXML.Auth
{
    /// <summary>
    /// cos 业务认证: secretId, signKey, keyTime
    /// signKey be calculated by secretKey and keyTime
    /// </summary>
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
