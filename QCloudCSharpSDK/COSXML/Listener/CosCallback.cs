using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model;
using COSXML.CosException;
using System.IO;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/21/2018 10:12:14 AM
* bradyxiao
*/
namespace COSXML.Callback
{
    /// <summary>
    /// progress
    /// </summary>
    /// <param name="completed"></param>
    /// <param name="total"></param>
    public delegate void OnProgressCallback(long completed, long total);

    /// <summary>
    /// notify has been got response
    /// </summary>
    public delegate void OnNotifyGetResponse();

    /// <summary>
    /// success
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="cosRequest"></param>
    /// <param name="cosResult"></param>
    public delegate void OnSuccessCallback<T>(T cosResult)
        where T : CosResult;
     
    /// <summary>
    /// failed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cosRequest"></param>
    /// <param name="clientException"></param>
    /// <param name="serverException"></param>
    public delegate void OnFailedCallback (CosClientException clientException, CosServerException serverException);

    /// <summary>
    /// parse stream
    /// </summary>
    /// <param name="inputStream"></param>
    /// <param name="contentType"></param>
    /// <param name="contentLength"></param>
    /// <exception cref="CosServerException">throw CosServerException</exception>
    /// <exception cref="CosClientException">throw CosClientException</exception>
    /// <exception cref="Exception"> throw Excetpion</exception>
    public delegate void OnParseStream(Stream inputStream, string contentType, long contentLength);
}
