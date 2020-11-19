using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model;
using COSXML.CosException;
using System.IO;

namespace COSXML.Callback
{
    /// <summary>
    /// 上传、下载进度回调
    /// </summary>
    /// <param name="completed"></param>
    /// <param name="total"></param>
    public delegate void OnProgressCallback(long completed, long total);

    /// <summary>
    /// 获取结果的回调
    /// </summary>
    public delegate void OnNotifyGetResponse();

    /// <summary>
    /// 请求成功回调
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="cosRequest"></param>
    /// <param name="cosResult"></param>
    public delegate void OnSuccessCallback<T>(T cosResult)
        where T : CosResult;

    /// <summary>
    /// 请求失败回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cosRequest"></param>
    /// <param name="clientException"></param>
    /// <param name="serverException"></param>
    public delegate void OnFailedCallback(CosClientException clientException, CosServerException serverException);

    /// <summary>
    /// 解析流回调
    /// </summary>
    /// <param name="inputStream"></param>
    /// <param name="contentType"></param>
    /// <param name="contentLength"></param>
    /// <exception cref="CosServerException">throw CosServerException</exception>
    /// <exception cref="CosClientException">throw CosClientException</exception>
    /// <exception cref="Exception"> throw Excetpion</exception>
    public delegate void OnParseStream(Stream inputStream, string contentType, long contentLength);
}
