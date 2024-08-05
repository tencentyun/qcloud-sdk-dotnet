//  Create by COSSDKTOOLS;

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Model.Object;
using COSXML.Model.CI;
using COSXML.CosException;
using COSXML.Utils;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求
    /// <see href="https://cloud.tencent.com/document/product/460/83091"/>
    /// </summary>
    public sealed class CreateFileZipProcessJobsRequest : CIRequest
    {
        public CreateFileZipProcessJobs createFileZipProcessJobs;

        public CreateFileZipProcessJobsRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.POST;
            this.SetRequestPath("/"+"jobs");
            this.SetRequestHeader("Content-Type", "application/xml");
            createFileZipProcessJobs = new CreateFileZipProcessJobs();
            createFileZipProcessJobs.operation = new CreateFileZipProcessJobs.CreateFileZipProcessJobsOperation();
            createFileZipProcessJobs.operation.fileCompressConfig = new CreateFileZipProcessJobs.FileCompressConfig();
            createFileZipProcessJobs.operation.fileCompressConfig.keyConfig = new List<CreateFileZipProcessJobs.KeyConfig>();
            createFileZipProcessJobs.operation.output = new CreateFileZipProcessJobs.CreateFileZipProcessJobsOutput();
            createFileZipProcessJobs.callBackMqConfig = new CreateFileZipProcessJobs.CallBackMqConfig();
        }


        public void SetTag(string tag){
            createFileZipProcessJobs.tag = tag;
        }

        public void SetFlatten(string flatten){
            createFileZipProcessJobs.operation.fileCompressConfig.flatten = flatten;
        }

        public void SetFormat(string format){
            createFileZipProcessJobs.operation.fileCompressConfig.format = format;
        }

        public void SetType(string type){
            createFileZipProcessJobs.operation.fileCompressConfig.type = type;
        }

        public void SetCompressKey(string compressKey){
            createFileZipProcessJobs.operation.fileCompressConfig.compressKey = compressKey;
        }

        public void SetUrlList(string urlList){
            createFileZipProcessJobs.operation.fileCompressConfig.urlList = urlList;
        }

        public void SetPrefix(string prefix){
            createFileZipProcessJobs.operation.fileCompressConfig.prefix = prefix;
        }

        public void SetIgnoreError (string ignoreError ){
            createFileZipProcessJobs.operation.fileCompressConfig.ignoreError  = ignoreError ;
        }

        // public void SetKey(string key){
        //     createFileZipProcessJobs.operation.fileCompressConfig.keyConfig.key = key;
        // }
        //
        // public void SetRename(string rename){
        //     createFileZipProcessJobs.operation.fileCompressConfig.keyConfig.rename = rename;
        // }
        //
        // public void SetImageParams(string imageParams){
        //     createFileZipProcessJobs.operation.fileCompressConfig.keyConfig.imageParams = imageParams;
        // }

        public void setKeyConfig(CreateFileZipProcessJobs.KeyConfig keyConfig)
        {
            if (keyConfig != null)
            {
                createFileZipProcessJobs.operation.fileCompressConfig.keyConfig.Add(keyConfig);
            }
        }
        public void SetUserData(string userData){
            createFileZipProcessJobs.operation.userData = userData;
        }

        public void SetRegion(string region){
            createFileZipProcessJobs.operation.output.region = region;
        }

        public void SetBucket(string bucket){
            createFileZipProcessJobs.operation.output.bucket = bucket;
        }

        public void SetObjectInfo(string objectInfo){
            createFileZipProcessJobs.operation.output.objectInfo = objectInfo;
        }

        public void SetCallBackFormat(string callBackFormat){
            createFileZipProcessJobs.callBackFormat = callBackFormat;
        }

        public void SetCallBackType(string callBackType){
            createFileZipProcessJobs.callBackType = callBackType;
        }

        public void SetCallBack(string callBack){
            createFileZipProcessJobs.callBack = callBack;
        }

        public void SetMqRegion(string mqRegion){
            createFileZipProcessJobs.callBackMqConfig.mqRegion = mqRegion;
        }

        public void SetMqMode(string mqMode){
            createFileZipProcessJobs.callBackMqConfig.mqMode = mqMode;
        }

        public void SetMqName(string mqName){
            createFileZipProcessJobs.callBackMqConfig.mqName = mqName;
        }

		public override Network.RequestBody GetRequestBody()
		{
			return GetXmlRequestBody(createFileZipProcessJobs);
		}
    }
}
