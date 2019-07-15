using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Service;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model;
using COSXML.Model.Tag;

namespace COSXML
{ 

    public interface CosXml
    {
        /// <summary>
        /// 生成签名串
        /// </summary>
        /// <param name="method">http method</param>
        /// <param name="path">http url path</param>
        /// <param name="queryParameters">http url query</param>
        /// <param name="headers">http header</param>
        /// <param name="signTime">sign time</param>
        /// <returns></returns>
        string GenerateSign(string method, string key, Dictionary<string, string> queryParameters, Dictionary<string, string> headers,
            long signDurationSecond);

        /// <summary>
        /// 生成预签名URL
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryParameters"></param>
        /// <param name="headers"></param>
        /// <param name="signTime"></param>
        /// <returns></returns>
        string GenerateSignURL(PreSignatureStruct preSignatureStruct);

        string GetAccessURL(CosRequest request);

        /// <summary>
        /// get service for cos
        ///    try
        ///    {
        ///    	GetServiceRequest request = new GetServiceRequest();
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetServiceResult result = cosXml.GetService(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request"> <see cref="COSXML.Model.Service.GetServiceRequest"/>GetServiceRequest </param>
        /// <returns><see cref="COSXML.Model.Service.GetServiceResult"/>GetServiceResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetServiceResult GetService(GetServiceRequest request);

        /// <summary>
        /// asynchronous get service for cos
        ///    //异步方法
        ///    GetServiceRequest request = new GetServiceRequest();
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    cosXml.GetService(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetServiceResult result = cosResult as GetServiceResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request">GetServiceRequest</param>
        /// <param name="successCallback">OnSuccessCallback</param>
        /// <param name="failCallback">OnFailedCallback</param>
        void GetService(GetServiceRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	PutBucketRequest request = new PutBucketRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	PutBucketResult result = cosXml.PutBucket(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutBucketRequest</param>
        /// <returns>PutBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketResult PutBucket(PutBucketRequest request);

        /// <summary>
        /// asynchronous put bucket for cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    PutBucketRequest request = new PutBucketRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    cosXml.PutBucket(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutBucketResult result = cosResult as PutBucketResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucket(PutBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// Head bucket for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	HeadBucketRequest request = new HeadBucketRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	HeadBucketResult result = cosXml.HeadBucket(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">HeadBucketRequest</param>
        /// <returns>HeadBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        HeadBucketResult HeadBucket(HeadBucketRequest request);

        /// <summary>
        /// asynchronous Head bucket for cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    HeadBucketRequest request = new HeadBucketRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    cosXml.HeadBucket(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		HeadBucketResult result = cosResult as HeadBucketResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void HeadBucket(HeadBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// Get bucket for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketRequest request = new GetBucketRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetBucketResult result = cosXml.GetBucket(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketRequest</param>
        /// <returns>GetBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketResult GetBucket(GetBucketRequest request);

        /// <summary>
        /// asynchronous Get bucket for cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketRequest request = new GetBucketRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    cosXml.GetBucket(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetBucketResult result = cosResult as GetBucketResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucket(GetBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	DeleteBucketCORSResult result = cosXml.DeleteBucketCORS(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">DeleteBucketRequest</param>
        /// <returns>DeleteBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketResult DeleteBucket(DeleteBucketRequest request);

        /// <summary>
        /// asynchronous delete bucket for cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.DeleteBucketCORS(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		DeleteBucketCORSResult result = cosResult as DeleteBucketCORSResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteBucket(DeleteBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket location for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketLocationRequest request = new GetBucketLocationRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetBucketLocationResult result = cosXml.GetBucketLocation(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketLocationRequest</param>
        /// <returns>GetBucketLocationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketLocationResult GetBucketLocation(GetBucketLocationRequest request);

        /// <summary>
        /// asynchronous get bucket location
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketLocationRequest request = new GetBucketLocationRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    cosXml.GetBucketLocation(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetBucketLocationResult result = cosResult as GetBucketLocationResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucketLocation(GetBucketLocationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket acl for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	PutBucketACLRequest request = new PutBucketACLRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置私有读写权限
        ///    	request.SetCosACL(CosACL.PRIVATE);
        ///    	//授予1131975903账号读权限
        ///    	COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
        ///    	readAccount.AddGrantAccount("1131975903", "1131975903");
        ///    	request.SetXCosGrantRead(readAccount);
        ///    	//执行请求
        ///    	PutBucketACLResult result = cosXml.PutBucketACL(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutBucketACLRequest</param>
        /// <returns>PutBucketACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketACLResult PutBucketACL(PutBucketACLRequest request);

        /// <summary>
        /// put bucket acl
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    PutBucketACLRequest request = new PutBucketACLRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置私有读写权限
        ///    request.SetCosACL(CosACL.PRIVATE);
        ///    //授予1131975903账号读权限
        ///    COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
        ///    readAccount.AddGrantAccount("1131975903", "1131975903");
        ///    request.SetXCosGrantRead(readAccount);
        ///    //执行请求
        ///    cosXml.PutBucketACL(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutBucketACLResult result = cosResult as PutBucketACLResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucketACL(PutBucketACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket acl for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketACLRequest request = new GetBucketACLRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetBucketACLResult result = cosXml.GetBucketACL(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketACLRequest</param>
        /// <returns>GetBucketACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketACLResult GetBucketACL(GetBucketACLRequest request);

        /// <summary>
        /// get bucket acl
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketACLRequest request = new GetBucketACLRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    cosXml.GetBucketACL(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetBucketACLResult result = cosResult as GetBucketACLResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucketACL(GetBucketACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket policy for cos
        /// 
        /// </summary>
        /// <param name="request">DeleteBucketPolicyRequest</param>
        /// <returns>DeleteBucketPolicyResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketPolicyResult DeleteBucketPolicy(DeleteBucketPolicyRequest request);

        /// <summary>
        /// delete bucket policy for cos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteBucketPolicy(DeleteBucketPolicyRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket cros for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	PutBucketCORSRequest request = new PutBucketCORSRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置跨域访问配置CORS
        ///    	COSXML.Model.Tag.CORSConfiguration.CORSRule corsRule = new COSXML.Model.Tag.CORSConfiguration.CORSRule();
        ///    	corsRule.id = "corsconfigureId";
        ///    	corsRule.maxAgeSeconds = 6000;
        ///    	corsRule.allowedOrigin = "http://cloud.tencent.com";
        ///    
        ///    	corsRule.allowedMethods = new List&lt;string&gt;();
        ///    	corsRule.allowedMethods.Add("PUT");
        ///    
        ///    	corsRule.allowedHeaders = new List&lt;string&gt;();
        ///    	corsRule.allowedHeaders.Add("Host");
        ///    
        ///    	corsRule.exposeHeaders = new List&lt;string&gt;();
        ///    	corsRule.exposeHeaders.Add("x-cos-meta-x1");
        ///    
        ///    	request.SetCORSRule(corsRule);
        ///    
        ///    	//执行请求
        ///    	PutBucketCORSResult result = cosXml.PutBucketCORS(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///     	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutBucketCORSRequest</param>
        /// <returns>PutBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketCORSResult PutBucketCORS(PutBucketCORSRequest request);

        /// <summary>
        /// put bucket cors
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    PutBucketCORSRequest request = new PutBucketCORSRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    //设置跨域访问配置CORS
        ///    COSXML.Model.Tag.CORSConfiguration.CORSRule corsRule = new COSXML.Model.Tag.CORSConfiguration.CORSRule();
        ///    corsRule.id = "corsconfigureId";
        ///    corsRule.maxAgeSeconds = 6000;
        ///    corsRule.allowedOrigin = "http://cloud.tencent.com";
        ///    
        ///    corsRule.allowedMethods = new List&lt;string&gt;();
        ///    corsRule.allowedMethods.Add("PUT");
        ///    
        ///    corsRule.allowedHeaders = new List&lt;string&gt;();
        ///    corsRule.allowedHeaders.Add("Host");
        ///    
        ///    corsRule.exposeHeaders = new List&lt;string&gt;();
        ///    corsRule.exposeHeaders.Add("x-cos-meta-x1");
        ///    
        ///    request.SetCORSRule(corsRule);
        ///    
        ///    cosXml.PutBucketCORS(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutBucketCORSResult result = cosResult as PutBucketCORSResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucketCORS(PutBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket cros for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketCORSRequest request = new GetBucketCORSRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetBucketCORSResult result = cosXml.GetBucketCORS(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketCORSRequest</param>
        /// <returns>GetBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketCORSResult GetBucketCORS(GetBucketCORSRequest request);

        /// <summary>
        /// get bucket cors
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketCORSRequest request = new GetBucketCORSRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.GetBucketCORS(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetBucketCORSResult result = cosResult as GetBucketCORSResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucketCORS(GetBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket cros for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	DeleteBucketCORSResult result = cosXml.DeleteBucketCORS(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">DeleteBucketCORSRequest</param>
        /// <returns>DeleteBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketCORSResult DeleteBucketCORS(DeleteBucketCORSRequest request);

        /// <summary>
        /// delete bucket cors
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.DeleteBucketCORS(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		DeleteBucketCORSResult result = cosResult as DeleteBucketCORSResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteBucketCORS(DeleteBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket lifecycle for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	PutBucketLifecycleRequest request = new PutBucketLifecycleRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置 lifecycle
        ///    	COSXML.Model.Tag.LifecycleConfiguration.Rule rule = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
        ///    	rule.id = "lfiecycleConfigureId";
        ///    	rule.status = "Enabled"; //Enabled，Disabled
        ///    
        ///    	rule.filter = new COSXML.Model.Tag.LifecycleConfiguration.Filter();
        ///    	rule.filter.prefix = "2/";
        ///    
        ///    	//指定分片过期删除操作
        ///    	rule.abortIncompleteMultiUpload = new COSXML.Model.Tag.LifecycleConfiguration.AbortIncompleteMultiUpload();
        ///    	rule.abortIncompleteMultiUpload.daysAfterInitiation = 2;
        ///    
        ///    	request.SetRule(rule);
        ///    
        ///    	//执行请求
        ///    	PutBucketLifecycleResult result = cosXml.PutBucketLifecycle(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutBucketLifecycleRequest</param>
        /// <returns>PutBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketLifecycleResult PutBucketLifecycle(PutBucketLifecycleRequest request);

        /// <summary>
        /// put bucket lifecycle
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    PutBucketLifecycleRequest request = new PutBucketLifecycleRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置 lifecycle
        ///    COSXML.Model.Tag.LifecycleConfiguration.Rule rule = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
        ///    rule.id = "lfiecycleConfigureId";
        ///    rule.status = "Enabled"; //Enabled，Disabled
        ///    
        ///    rule.filter = new COSXML.Model.Tag.LifecycleConfiguration.Filter();
        ///    rule.filter.prefix = "2/";
        ///    
        ///    rule.abortIncompleteMultiUpload = new COSXML.Model.Tag.LifecycleConfiguration.AbortIncompleteMultiUpload();
        ///    rule.abortIncompleteMultiUpload.daysAfterInitiation = 2;
        ///    
        ///    request.SetRule(rule);
        ///    
        ///    //执行请求
        ///    cosXml.PutBucketLifecycle(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutBucketLifecycleResult result = cosResult as PutBucketLifecycleResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucketLifecycle(PutBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket lifecycle for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketLifecycleRequest request = new GetBucketLifecycleRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetBucketLifecycleResult result = cosXml.GetBucketLifecycle(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketLifecycleRequest</param>
        /// <returns>GetBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketLifecycleResult GetBucketLifecycle(GetBucketLifecycleRequest request);

        /// <summary>
        /// get bucket lifecycle
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketLifecycleRequest request = new GetBucketLifecycleRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.GetBucketLifecycle(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		 GetBucketLifecycleResult result = cosResult as GetBucketLifecycleResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucketLifecycle(GetBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket lifecycle for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	DeleteBucketLifecycleRequest request = new DeleteBucketLifecycleRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	DeleteBucketLifecycleResult result = cosXml.DeleteBucketLifecycle(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">DeleteBucketLifecycleRequest</param>
        /// <returns>DeleteBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        DeleteBucketLifecycleResult DeleteBucketLifecycle(DeleteBucketLifecycleRequest request);

        /// <summary>
        /// delete bucket lifecycle
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    DeleteBucketLifecycleRequest request = new DeleteBucketLifecycleRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.DeleteBucketLifecycle(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		DeleteBucketLifecycleResult result = cosResult as DeleteBucketLifecycleResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteBucketLifecycle(DeleteBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket replication for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	PutBucketReplicationRequest request = new PutBucketReplicationRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    	//设置replication
        ///    	PutBucketReplicationRequest.RuleStruct ruleStruct = new PutBucketReplicationRequest.RuleStruct();
        ///    	ruleStruct.appid = "目标存储桶所在 APPID";
        ///    	ruleStruct.bucket = "目标存储桶名称"; //bucketName,不包含 '-appid'
        ///    	ruleStruct.region = "目标存储桶所在地域";
        ///    	ruleStruct.isEnable = true;
        ///    	ruleStruct.storageClass = "目标存储桶对象类型"; //可不填
        ///    	ruleStruct.id = "配置ID";
        ///    	ruleStruct.prefix = "指定复制对象的前缀";
        ///    	List&lt;PutBucketReplicationRequest.RuleStruct&gt;ruleStructs = new List&lt;PutBucketReplicationRequest.RuleStruct&gt;();
        ///    	ruleStructs.Add(ruleStruct);
        ///    	string subUin = "指定子账号的uin"；
        ///    	string ownerUin = "指定根账号的uin";
        ///    	request.SetReplicationConfiguration(ownerUin, subUin, ruleStructs);
        ///    
        ///    	//执行请求
        ///    	PutBucketReplicationResult result = cosXml.PutBucketReplication(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutBucketReplicationRequest</param>
        /// <returns>PutBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketReplicationResult PutBucketReplication(PutBucketReplicationRequest request);

        /// <summary>
        /// put bucket replication
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    PutBucketReplicationRequest request = new PutBucketReplicationRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    //设置replication
        ///    PutBucketReplicationRequest.RuleStruct ruleStruct = new PutBucketReplicationRequest.RuleStruct();
        ///    ruleStruct.appid = "目标存储桶所在 APPID";
        ///    ruleStruct.bucket = "目标存储桶名称"; //bucketName,不包含 '-appid'
        ///    ruleStruct.region = "目标存储桶所在地域";
        ///    ruleStruct.isEnable = true;
        ///    ruleStruct.storageClass = "目标存储桶对象类型"; //可不填
        ///    ruleStruct.id = "配置ID";
        ///    ruleStruct.prefix = "指定复制对象的前缀";
        ///    List&lt;PutBucketReplicationRequest.RuleStruct&gt;ruleStructs = new List&lt;PutBucketReplicationRequest.RuleStruct&gt;();
        ///    ruleStructs.Add(ruleStruct);
        ///    string subUin = "指定子账号的uin"；
        ///    string ownerUin = "指定根账号的uin";
        ///    request.SetReplicationConfiguration(ownerUin, subUin, ruleStructs);
        ///    
        ///    //执行请求
        ///    cosXml.PutBucketReplication(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutBucketReplicationResult result = cosResult as PutBucketReplicationResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucketReplication(PutBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket replication for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketReplicationRequest request = new GetBucketReplicationRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    	//执行请求
        ///    	GetBucketReplicationResult result = cosXml.GetBucketReplication(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketReplicationRequest</param>
        /// <returns>GetBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketReplicationResult GetBucketReplication(GetBucketReplicationRequest request);

        /// <summary>
        /// get bucket replication
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketReplicationRequest request = new GetBucketReplicationRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    //执行请求
        ///    cosXml.GetBucketReplication(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetBucketReplicationResult result = cosResult as GetBucketReplicationResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucketReplication(GetBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket replication for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	DeleteBucketReplicationRequest request = new DeleteBucketReplicationRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    	//执行请求
        ///    	DeleteBucketReplicationResult result = cosXml.DeleteBucketReplication(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">DeleteBucketReplicationRequest</param>
        /// <returns>DeleteBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        DeleteBucketReplicationResult DeleteBucketReplication(DeleteBucketReplicationRequest request);

        /// <summary>
        /// delete bucket replication
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    DeleteBucketReplicationRequest request = new DeleteBucketReplicationRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    //执行请求
        ///    cosXml.DeleteBucketReplication(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		DeleteBucketReplicationResult result = cosResult as DeleteBucketReplicationResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteBucketReplication(DeleteBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket versioning for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	PutBucketVersioningRequest request = new PutBucketVersioningRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//开启版本控制： true; 不开启： false
        ///    	request.IsEnableVersionConfig(true);
        ///    
        ///    	//执行请求
        ///    	PutBucketVersioningResult result = cosXml.PutBucketVersioning(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutBucketVersioningRequest</param>
        /// <returns>PutBucketVersioningResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketVersioningResult PutBucketVersioning(PutBucketVersioningRequest request);

        /// <summary>
        /// put bucket versioning
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    PutBucketVersioningRequest request = new PutBucketVersioningRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    //开启版本控制： true; 不开启： false
        ///    request.IsEnableVersionConfig(true);
        ///    
        ///    //执行请求
        ///    cosXml.PutBucketVersioning(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutBucketVersioningResult result = cosResult as PutBucketVersioningResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucketVersioning(PutBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket versioning for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	GetBucketVersioningRequest request = new GetBucketVersioningRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    	//执行请求
        ///    	GetBucketVersioningResult result = cosXml.GetBucketVersioning(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetBucketVersioningRequest</param>
        /// <returns>GetBucketVersioningResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketVersioningResult GetBucketVersioning(GetBucketVersioningRequest request);

        /// <summary>
        /// get bucket versions
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    GetBucketVersioningRequest request = new GetBucketVersioningRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    //执行请求
        ///    cosXml.GetBucketVersioning(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetBucketVersioningResult result = cosResult as GetBucketVersioningResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucketVersioning(GetBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// list bucket versions for cos
        /// </summary>
        /// <param name="request">ListBucketVersionsRequest</param>
        /// <returns>ListBucketVersionsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        ListBucketVersionsResult ListBucketVersions(ListBucketVersionsRequest request);

        void ListBucketVersions(ListBucketVersionsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// list multiUploads for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    	ListMultiUploadsRequest request = new ListMultiUploadsRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	ListMultiUploadsResult result = cosXml.ListMultiUploads(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">ListMultiUploadsRequest</param>
        /// <returns>ListMultiUploadsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        ListMultiUploadsResult ListMultiUploads(ListMultiUploadsRequest request);

        /// <summary>
        /// list multiUploads of bucket
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //格式：bucketname-appid
        ///    ListMultiUploadsRequest request = new ListMultiUploadsRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.ListMultiUploads(request, 
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		ListMultiUploadsResult result = cosResult as ListMultiUploadsResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void ListMultiUploads(ListMultiUploadsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put object to cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string srcPath = @"F:\test.txt"；//本地文件路径
        ///    	PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置进度回调
        ///    	request.SetCosProgressCallback(delegate(long completed, long total)
        ///    	{
        ///    		Console.WriteLine(String.Format("progress = {1:##.##}%", completed * 100.0 / total));
        ///    	});
        ///    	//执行请求
        ///    	PutObjectResult result = cosXml.PutObject(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutObjectRequest</param>
        /// <returns>PutObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutObjectResult PutObject(PutObjectRequest request);

        /// <summary>
        /// put object to cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string srcPath = @"F:\test.txt";  //本地文件路径
        ///    PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置进度回调
        ///    request.SetCosProgressCallback(delegate(long completed, long total)
        ///    {
        ///    	Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
        ///    });
        ///    //执行请求
        ///    cosXml.PutObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutObjectResult result = cosResult as PutObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutObject(PutObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// head object for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	HeadObjectRequest request = new HeadObjectRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	HeadObjectResult result = cosXml.HeadObject(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">HeadObjectRequest</param>
        /// <returns>HeadObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        HeadObjectResult HeadObject(HeadObjectRequest request);

        /// <summary>
        /// head object
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    HeadObjectRequest request = new HeadObjectRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.HeadObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		HeadObjectResult result = cosResult as HeadObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void HeadObject(HeadObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get object for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string localDir = @"F:\"；//下载到本地指定文件夹
        ///    	string localFileName = "test.txt"; //指定本地保存的文件名
        ///    	GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置进度回调
        ///    	request.SetCosProgressCallback(delegate(long completed, long total)
        ///    	{
        ///    		Console.WriteLine(String.Format("progress = {1:##.##}%", completed * 100.0 / total));
        ///    	});
        ///    	//执行请求
        ///    	GetObjectResult result = cosXml.GetObject(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///     	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetObjectRequest</param>
        /// <returns>GetObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetObjectResult GetObject(GetObjectRequest request);

        /// <summary>
        /// get object
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string localDir = @"F:\"；//下载到本地指定文件夹
        ///    string localFileName = "test.txt"; //指定本地保存的文件名
        ///    GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置进度回调
        ///    request.SetCosProgressCallback(delegate(long completed, long total)
        ///    {
        ///    	Console.WriteLine(String.Format("progress = {1:##.##}%", completed * 100.0 / total));
        ///    });
        ///    //执行请求
        ///    cosXml.GetObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetObjectResult result = cosResult as GetObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetObject(GetObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get object bytes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetObjectBytesResult GetObject(GetObjectBytesRequest request);

        void GetObject(GetObjectBytesRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put object acl for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	PutObjectACLRequest request = new PutObjectACLRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置私有读写权限 
        ///    	request.SetCosACL(CosACL.PRIVATE); 
        ///    	//授予1131975903账号读权限 
        ///    	COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount(); 
        ///    	readAccount.AddGrantAccount("1131975903", "1131975903"); 
        ///    	request.setXCosGrantRead(readAccount);
        ///    	//执行请求
        ///    	PutObjectACLResult result = cosXml.PutObjectACL(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PutObjectACLRequest</param>
        /// <returns>PutObjectACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutObjectACLResult PutObjectACL(PutObjectACLRequest request);

        /// <summary>
        /// put object acl
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    PutObjectACLRequest request = new PutObjectACLRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置私有读写权限 
        ///    request.SetCosACL(CosACL.PRIVATE); 
        ///    //授予1131975903账号读权限 
        ///    COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount(); 
        ///    readAccount.AddGrantAccount("1131975903", "1131975903"); 
        ///    request.setXCosGrantRead(readAccount);
        ///    //执行请求
        ///    cosXml.PutObjectACL(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PutObjectACLResult result = cosResult as PutObjectACLResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutObjectACL(PutObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get object acl for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	GetObjectACLRequest request = new GetObjectACLRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	GetObjectACLResult result = cosXml.GetObjectACL(request);
        ///    	//请求成功
        ///        Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">GetObjectACLRequest</param>
        /// <returns>GetObjectACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetObjectACLResult GetObjectACL(GetObjectACLRequest request);

        /// <summary>
        /// get object acl
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    GetObjectACLRequest request = new GetObjectACLRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.GetObjectACL(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		GetObjectACLResult result = cosResult as GetObjectACLResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetObjectACL(GetObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete object for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	DeleteObjectRequest request = new DeleteObjectRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	DeleteObjectResult result = cosXml.DeleteObject(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">DeleteObjectRequest</param>
        /// <returns>DeleteObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteObjectResult DeleteObject(DeleteObjectRequest request);

        /// <summary>
        /// delete object
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    DeleteObjectRequest request = new DeleteObjectRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.DeleteObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		DeleteObjectResult getObjectResult = result as DeleteObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteObject(DeleteObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete multi objects for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置返回结果形式
        ///    	request.SetDeleteQuiet(false);
        ///    	//删除多个对象
        ///    	List&lt;string&gt; keys = new List&lt;string&gt;();
        ///    	keys.Add("test1.txt");
        ///    	keys.Add("test2.txt");
        ///    	request.SetObjectKeys(keys);
        ///    	//执行请求
        ///    	DeleteMultiObjectResult result = cosXml.DeleteMultiObjects(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">DeleteMultiObjectRequest</param>
        /// <returns>DeleteMultiObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteMultiObjectResult DeleteMultiObjects(DeleteMultiObjectRequest request);

        /// <summary>
        /// delete multi objects
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置返回结果形式
        ///    request.SetDeleteQuiet(false);
        ///    //删除多个对象
        ///    	List&lt;string&gt; keys = new List&lt;string&gt;();
        ///    keys.Add("test1.txt");
        ///    keys.Add("test2.txt");
        ///    request.SetObjectKeys(keys);
        ///    
        ///    //执行请求
        ///    cosXml.DeleteMultiObjects(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		DeleteMultiObjectResult result = cosResult as DeleteMultiObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteMultiObjects(DeleteMultiObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// init multiupload for a object to cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	InitMultipartUploadRequest request = new InitMultipartUploadRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	InitMultipartUploadResult result = cosXml.InitMultipartUpload(request);
        ///    	//请求成功
        ///    	string uploadId = result.initMultipartUpload.uploadId; //用于后续分片上传的 uploadId
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        InitMultipartUploadResult InitMultipartUpload(InitMultipartUploadRequest request);

        /// <summary>
        /// init init multiupload for a object 
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    InitMultipartUploadRequest request = new InitMultipartUploadRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.InitMultipartUpload(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		InitMultipartUploadResult result = cosResult as InitMultipartUploadResult;
        ///    		string uploadId = result.initMultipartUpload.uploadId; //用于后续分片上传的 uploadId
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void InitMultipartUpload(InitMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// list all has been uploaded parts of a object for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    	ListPartsRequest request = new ListPartsRequest(bucket, key, uploadId);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	ListPartsResult result = cosXml.ListParts(request);
        ///    	//请求成功
        ///    	//列举已上传的分片块
        ///    	List&lt;COSXML.Model.Tag.ListParts.Part&gt; alreadyUploadParts = result.listParts.parts;
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">ListPartsRequest</param>
        /// <returns>ListPartsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        ListPartsResult ListParts(ListPartsRequest request);

        /// <summary>
        /// list all has been uploaded parts of a object for cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    ListPartsRequest request = new ListPartsRequest(bucket, key, uploadId);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.ListParts(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		ListPartsResult result = cosResult as ListPartsResult;
        ///    		//列举已上传的分片块
        ///    		List&lt;COSXML.Model.Tag.ListParts.Part&gt; alreadyUploadParts = result.listParts.parts;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	 {	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void ListParts(ListPartsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// upload a part of a object to cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    	int partNumber = 1; //分片块编号，必须从1开始递增
        ///    	string srcPath = @"F:\test.txt"; //本地文件路径
        ///    	UploadPartRequest request = new UploadPartRequest(bucket, key, partNumber, uploadId, srcPath);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置进度回调
        ///    	request.SetCosProgressCallback(delegate(long completed, long total)
        ///    	{
        ///    		Console.WriteLine(String.Format("progress = {0:##.##}%",  completed * 100.0 / total));
        ///    	});
        ///    	//执行请求
        ///    	UploadPartResult result = cosXml.UploadPart(request);
        ///    	//请求成功
        ///    	//获取返回分片块的eTag,用于后续CompleteMultiUploads
        ///    	string eTag = result.eTag;
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">UploadPartRequest</param>
        /// <returns>UploadPartResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        UploadPartResult UploadPart(UploadPartRequest request);

        /// <summary>
        /// upload a part of a object
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    int partNumber = 1; //分片块编号，必须从1开始递增
        ///    string srcPath = @"F:\test.txt"; //本地文件路径
        ///    UploadPartRequest request = new UploadPartRequest(bucket, key, partNumber, uploadId, srcPath);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置进度回调
        ///    request.SetCosProgressCallback(delegate(long completed, long total)
        ///    {
        ///    	Console.WriteLine(String.Format("progress = {0:##.##}%",  completed * 100.0 / total));
        ///    });
        ///    //执行请求
        ///    cosXml.UploadPart(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		UploadPartResult result = cosResult as UploadPartResult;
        ///    		//获取返回分片块的eTag,用于后续CompleteMultiUploads
        ///    		string eTag = result.eTag;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void UploadPart(UploadPartRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// complete all parts of a object for cos 
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    	CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(bucket, key, uploadId);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置已上传的parts,必须有序，按照partNumber递增
        ///    	request.SetPartNumberAndETag(1, "partNumber1 eTag");
        ///    	//执行请求
        ///    	CompleteMultipartUploadResult result = cosXml.CompleteMultiUpload(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">CompleteMultiUploadRequest</param>
        /// <returns>CompleteMultiUploadResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        CompleteMultipartUploadResult CompleteMultiUpload(CompleteMultipartUploadRequest request);

        /// <summary>
        /// complete all parts of a object
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(bucket, key, uploadId);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置已上传的parts,必须有序，按照partNumber递增
        ///    request.SetPartNumberAndETag(1, "partNumber1 eTag");
        ///    //执行请求
        ///    cosXml.CompleteMultiUpload(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		CompleteMultipartUploadResult result = result as CompleteMultipartUploadResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///       {	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void CompleteMultiUpload(CompleteMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// abort has been parts of a object in cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    	AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//执行请求
        ///    	AbortMultipartUploadResult result = cosXml.AbortMultiUpload(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">AbortMultiUploadRequest</param>
        /// <returns>AbortMultiUploadResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        AbortMultipartUploadResult AbortMultiUpload(AbortMultipartUploadRequest request);

        /// <summary>
        /// abort has been parts of a object
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string uploadId ="xxxxxxxx"; //初始化分片上传返回的uploadId
        ///    AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //执行请求
        ///    cosXml.AbortMultiUpload(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		AbortMultipartUploadResult result = result as AbortMultipartUploadResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void AbortMultiUpload(AbortMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// copy object to another object for cos
        ///    try
        ///    {
        ///    	string sourceAppid = "1253960454"; //账号 appid
        ///    	string sourceBucket = "source-1253960454"; //"源对象所在的存储桶
        ///    	string sourceRegion = "ap-beijing"; //源对象的存储桶所在的地域
        ///    	string sourceKey = "test.txt"; //源对象键
        ///    	//构造源对象属性
        ///    	COSXML.Model.Tag.CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
        ///    
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "copy_test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	CopyObjectRequest request =  new CopyObjectRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置拷贝源
        ///    	request.SetCopySource(copySource);
        ///    	//设置是否拷贝还是更新,此处是拷贝
        ///    	request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.COPY);
        ///    	//执行请求
        ///    	CopyObjectResult result = cosXml.CopyObject(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">CopyObjectRequest</param>
        /// <returns>CopyObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        CopyObjectResult CopyObject(CopyObjectRequest request);

        /// <summary>
        /// copy object
        ///    //异步方法
        ///    string sourceAppid = "1253960454"; //账号 appid
        ///    string sourceBucket = "source-1253960454"; //"源对象所在的存储桶
        ///    string sourceRegion = "ap-beijing"; //源对象的存储桶所在的地域
        ///    string sourceKey = "test.txt"; //源对象键
        ///    //构造源对象属性
        ///    COSXML.Model.Tag.CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
        ///    
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "copy_test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    CopyObjectRequest request =  new CopyObjectRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置拷贝源
        ///    request.SetCopySource(copySource);
        ///    //设置是否拷贝还是更新,此处是拷贝
        ///    request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.COPY);
        ///    //执行请求
        ///    cosXml.CopyObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		CopyObjectResult result = cosResult as CopyObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    	});
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void CopyObject(CopyObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// copy part object for cos
        ///    try
        ///    {
        ///    	string sourceAppid = "1253960454"; //账号 appid
        ///    	string sourceBucket = "source-1253960454"; //"源对象所在的存储桶
        ///    	string sourceRegion = "ap-beijing"; //源对象的存储桶所在的地域
        ///    	string sourceKey = "test.txt"; //源对象键
        ///    	//构造源对象属性
        ///    	COSXML.Model.Tag.CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
        ///    
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "copy_test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string uploadId = "1505706248ca8373f8a5cd52cb129f4bcf85e11dc8833df34f4f5bcc456c99c42cd1ffa2f9 "； //初始化分片上传的 uploadId
        ///    	int partNumber = 1; // partNumber >= 1
        ///    	UploadPartCopyRequest request = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置拷贝源
        ///    	request.SetCopySource(copySource);
        ///    	//设置复制分片块
        ///    	request.SetCopyRange(0, 1024 * 1024);
        ///    	//执行请求
        ///    	UploadPartCopyResult result = cosXml.PartCopy(request);
        ///    	//请求成功
        ///    	//获取该分片块返回的eTag,用于CompleteMultiUpload
        ///    	string eTag = result.copyObject.eTag;
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">UploadPartCopyRequest</param>
        /// <returns>UploadPartCopyResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        UploadPartCopyResult PartCopy(UploadPartCopyRequest request);

        /// <summary>
        /// copy part object
        ///    //异步方法
        ///    string sourceAppid = "1253960454"; //账号 appid
        ///    string sourceBucket = "source-1253960454"; //"源对象所在的存储桶
        ///    string sourceRegion = "ap-beijing"; //源对象的存储桶所在的地域
        ///    string sourceKey = "test.txt"; //源对象键
        ///    //构造源对象属性
        ///    COSXML.Model.Tag.CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
        ///    
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "copy_test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string uploadId = "1505706248ca8373f8a5cd52cb129f4bcf85e11dc8833df34f4f5bcc456c99c42cd1ffa2f9 "； //初始化分片上传的 uploadId
        ///    int partNumber = 1; // partNumber >= 1
        ///    UploadPartCopyRequest request = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置拷贝源
        ///    request.SetCopySource(copySource);
        ///    //设置是否拷贝还是更新,此处是拷贝
        ///    request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.COPY);
        ///    //执行请求
        ///    cosXml.PartCopy(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		UploadPartCopyResult getObjectResult = result as UploadPartCopyResult;
        ///    		//获取该分片块返回的eTag,用于CompleteMultiUpload
        ///    		string eTag = result.copyObject.eTag;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PartCopy(UploadPartCopyRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// option object for cos
        ///    try
        ///    {
        ///     string origin = "http://cloud.tencent.com";
        ///     string accessMthod = "PUT";
        ///     OptionObjectRequest request = new OptionObjectRequest(bucket, key, origin, accessMthod);
        ///     //设置签名有效时长
        ///     request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///     //执行请求
        ///     OptionObjectResult result = cosXml.OptionObject(request);
        ///    
        ///     Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {
        ///     Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///     Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        OptionObjectResult OptionObject(OptionObjectRequest request);

        /// <summary>
        /// option object
        ///    异步方法
        ///    string origin = "http://cloud.tencent.com";
        ///    string accessMthod = "PUT";
        ///    OptionObjectRequest request = new OptionObjectRequest(bucket, key, origin, accessMthod);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    
        ///    cosXml.OptionObject(request,
        ///    delegate (CosResult cosResult)
        ///    {
        ///     OptionObjectResult result = cosResult as OptionObjectResult;
        ///     Console.WriteLine(result.GetResultInfo());
        ///    },
        ///    delegate (CosClientException clientEx, CosServerException serverEx)
        ///    {
        ///     if (clientEx != null)
        ///     {
        ///        Console.WriteLine("CosClientException: " + clientEx.Message);
        ///     }
        ///     if (serverEx != null)
        ///     {
        ///        Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///     }
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void OptionObject(OptionObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// post object to cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	string srcPath = @"F:\test.txt"；//本地文件路径
        ///    	PostObjectRequest request = new PostObjectRequest(bucket, key, srcPath);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//设置进度回调
        ///    	request.SetCosProgressCallback(delegate(long completed, long total)
        ///    	{
        ///    		Console.WriteLine(String.Format("progress = {1:##.##}%", completed * 100.0 / total));
        ///    	});
        ///    	//执行请求
        ///    	PostObjectResult result = cosXml.PostObject(request);
        ///    	//请求成功
        ///    	Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">PostObjectRequest</param>
        /// <returns>PostObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PostObjectResult PostObject(PostObjectRequest request);

        /// <summary>
        /// post obejct
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    string srcPath = @"F:\test.txt";  //本地文件路径
        ///    PostObjectRequest request = new PostObjectRequest(bucket, key, srcPath);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //设置进度回调
        ///    request.SetCosProgressCallback(delegate(long completed, long total)
        ///    {
        ///    	Console.WriteLine(String.Format("progress = {1:##.##}%", completed * 100.0 / total));
        ///    });
        ///    //执行请求
        ///    cosXml.PostObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		PostObjectResult result = cosResult as PostObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PostObject(PostObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// restore object for cos
        ///    try
        ///    {
        ///    	string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    	string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    	RestoreObjectRequest request = new RestoreObjectRequest(bucket, key);
        ///    	//设置签名有效时长
        ///    	request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    	//恢复时间
        ///    	request.SetExpireDays(3);
        ///    	request.SetTier(COSXML.Model.Tag.RestoreConfigure.Tier.Bulk);
        ///    
        ///    	//执行请求
        ///    	RestoreObjectResult result = cosXml.RestoreObject(request);
        ///    	//请求成功
        ///        Console.WriteLine(result.GetResultInfo());
        ///    }
        ///    catch (COSXML.CosException.CosClientException clientEx)
        ///    {	
        ///    	//请求失败
        ///    	Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    }
        ///    catch (COSXML.CosException.CosServerException serverEx)
        ///    {
        ///    	//请求失败
        ///    	Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    }
        /// </summary>
        /// <param name="request">RestoreObjectRequest</param>
        /// <returns>RestoreObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        RestoreObjectResult RestoreObject(RestoreObjectRequest request);

        /// <summary>
        /// restore object for cos
        ///    //异步方法
        ///    string bucket = "test-1253960454"; //存储桶，格式：bucketname-appid
        ///    string key = "test.txt"; //对象在存储桶中的位置，即称对象键.
        ///    RestoreObjectRequest request = new RestoreObjectRequest(bucket, key);
        ///    //设置签名有效时长
        ///    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
        ///    //恢复时间
        ///    request.SetExpireDays(3);
        ///    request.SetTier(COSXML.Model.Tag.RestoreConfigure.Tier.Bulk);
        ///    //执行请求
        ///    cosXml.RestoreObject(request,
        ///    	delegate(COSXML.Model.CosResult cosResult)
        ///    	{
        ///    		//请求成功
        ///    		RestoreObjectResult result = cosResult as RestoreObjectResult;
        ///    		Console.WriteLine(result.GetResultInfo());
        ///    
        ///    	}, 
        ///    	delegate(COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
        ///    	{	
        ///    		//请求失败
        ///    		if (clientEx != null)
        ///    		{
        ///    			Console.WriteLine("CosClientException: " + clientEx.Message);
        ///    		}
        ///    		else if (serverEx != null)
        ///    		{
        ///    			Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        ///    		}
        ///    });
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void RestoreObject(RestoreObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        void Cancel(CosRequest cosRequest);
    }
}
