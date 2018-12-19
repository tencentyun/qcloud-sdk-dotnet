using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.CosException;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/29/2018 5:08:32 PM
* bradyxiao
*/
namespace COSXML.Transfer
{
    public abstract class COSXMLTask
    {
        public COSXML.Callback.OnProgressCallback progressCallback;

        public COSXML.Callback.OnSuccessCallback<CosResult> successCallback;

        public COSXML.Callback.OnFailedCallback failCallback;

        public OnInternalHandleBeforExcute onInternalHandle;

        protected static CosXml cosXmlServer;

        protected string bucket;

        protected string region;

        protected string key;

        protected bool isNeedMd5 = true;

        public static void InitCosXmlServer(CosXml cosXml)
        {
            cosXmlServer = cosXml;
        }

        public COSXMLTask(string bucket, string region, string key)
        {
            this.bucket = bucket;
            this.region = region;
            this.key = key;
        }


    }

    internal class SliceStruct
    {
        public int partNumber;
        public bool isAlreadyUpload;
        public long sliceStart;
        public long sliceEnd;
        public long sliceLength;
        public string eTag;
    }

    public enum TaskState
    {

    }

    public delegate void OnInternalHandleBeforExcute(CosRequest cosRequest);

    internal interface OnMultipartUploadStateListener
    {
        void OnInit();
        void OnPart();
        void OnCompleted(CompleteMultiUploadResult result);
        void OnFailed(CosClientException clientEx, CosServerException serverEx);
    }

}
