using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.CosException;
using COSXML.Utils;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 提交文档转码任务
    /// <see href="https://cloud.tencent.com/document/product/460/46942"/>
    /// </summary>
    public sealed class CreateDocProcessJobsRequest : CIRequest
    {
        public DocumentProcessJobInfo documentProcessJobInfo;
        public CreateDocProcessJobsRequest(string bucket)
            : base(bucket)
        {
            documentProcessJobInfo = new DocumentProcessJobInfo();
            this.method = CosRequestMethod.POST;
            this.SetRequestPath("/doc_jobs");
            this.SetRequestHeader("Content-Type", "application/xml");
            documentProcessJobInfo.input = new DocumentProcessJobInfo.Input();
            documentProcessJobInfo.operation = new DocumentProcessJobInfo.Operation();
            documentProcessJobInfo.operation.output = new DocumentProcessJobInfo.Output();
            documentProcessJobInfo.operation.docProcess = new DocumentProcessJobInfo.DocProcess();
        }

        public void SetTag(string tag)
        {
            documentProcessJobInfo.tag = tag;
        }

        public void SetInputObject(string inputObject)
        {
            documentProcessJobInfo.input.inputObject = inputObject;
        }

        public void SetOutputRegion(string outputRegion)
        {
            documentProcessJobInfo.operation.output.region = outputRegion;
        }

        public void SetOutputBucket(string outputBucket)
        {
            documentProcessJobInfo.operation.output.bucket = outputBucket;
        }
        public void SetOutputObject(string outputObject)
        {
            documentProcessJobInfo.operation.output.outPutObject = outputObject;
        }
        public void SetSrcType(string srcType)
        {
            documentProcessJobInfo.operation.docProcess.srcType = srcType;
        }
        public void SetTgtType(string tgtType)
        {
            documentProcessJobInfo.operation.docProcess.tgtType = tgtType;
        }
        public void SetStartPage(string startPage)
        {
            documentProcessJobInfo.operation.docProcess.startPage = startPage;
        }
        public void SetEndPage(string endPage)
        {
            documentProcessJobInfo.operation.docProcess.endPage = endPage;
        }
        public void SetSheetId(string sheetId)
        {
            documentProcessJobInfo.operation.docProcess.sheetId = sheetId;
        }
        public void SetPaperDirection(string paperDirection)
        {
            documentProcessJobInfo.operation.docProcess.paperDirection = paperDirection;
        }
        public void SetPaperSize(string paperSize)
        {
            documentProcessJobInfo.operation.docProcess.paperSize = paperSize;
        }

        public void SetImageParams(string imageParams)
        {
            documentProcessJobInfo.operation.docProcess.imageParams = imageParams;
        }

        public void SetQuality(string quality)
        {
            documentProcessJobInfo.operation.docProcess.quality = quality;
        }

        public void SetZoom(string zoom)
        {
            documentProcessJobInfo.operation.docProcess.zoom = zoom;
        }

        public void SetImageDpi(string imageDpi)
        {
            documentProcessJobInfo.operation.docProcess.imageDpi = imageDpi;
        }

        public void SetPicPagination(string picPagination)
        {
            documentProcessJobInfo.operation.docProcess.picPagination = picPagination;
        }


        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(documentProcessJobInfo);
        }

    }
}
