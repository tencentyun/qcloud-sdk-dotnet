using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Text;
using System.Web;
using COSXML.Auth;
using COSXML.Common;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.CosException;
using COSXML.Utils;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 文档转 html 同步请求
    /// <see href="https://cloud.tencent.com/document/product/460/52518"/>
    /// </summary>
    public sealed class CreateDocPreviewRequest : ObjectRequest
    {
        public CreateDocPreviewRequest(string bucket, string key)
            : base(bucket,key){

            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("ci-process", "doc-preview");
            this.queryParameters.Add("dstType","html");
        }
        public class CommonOptions
        {
            public bool IsShowTopArea { get; set; }
            public bool IsShowHeader { get; set; }
            public string Language { get; set; }
            public bool isBrowserViewFullscreen { get; set; }
            public bool isIframeViewFullscreen { get; set; }
        }

        public class WordOptions
        {
            public bool isShowDocMap { get; set; }
            public bool isBestScale { get; set; }
        }
        public class PdfOptions
        {
           public bool isShowComment { get; set; }
           public bool isInSafeMode { get; set; }
           public bool isShowBottomStatusBar { get; set; }
        }
        public class PptOptions
        {
            public bool isShowBottomStatusBar { get; set; }
        }
        public class CommanBars
        {
            public string cmbId { get; set; }
            public Attributes attributes { get; set; }
        }
        public class Attributes
        {
            public bool visible { get; set; }
            public bool enable { get; set; }
        }
        public class HtmlParams
        {
            public CommonOptions CommonOptions { get; set; }
            public WordOptions WordOptions { get; set; }
            public PdfOptions PdfOptions { get; set; }
            public PptOptions PptOptions { get; set; }
            public CommanBars CommanBars { get; set; }
        }
        public void SetSrcType(string srcType){
            this.queryParameters.Add("srcType", srcType);
        }

        public void SetCopyable(string copyable){
            this.queryParameters.Add("copyable", copyable);
        }

        public void SetHtmlParams(string htmlParams){
            this.queryParameters.Add("htmlParams", htmlParams);
        }

        public void SetHtmlWaterword(string htmlwaterword){
            this.queryParameters.Add("htmlwaterword", htmlwaterword);
        }

        public void SetHtmlFillstyle(string htmlfillstyle){
            this.queryParameters.Add("htmlfillstyle", htmlfillstyle);
        }

        public void SetHtmlFront(string htmlfront){
            this.queryParameters.Add("htmlfront", htmlfront);
        }
        public void SetHtmlRotate(string htmlrotate){
            this.queryParameters.Add("htmlrotate", htmlrotate);
        }
        public void SetHtmlHorizontal(string htmlhorizontal){
            this.queryParameters.Add("htmlhorizontal", htmlhorizontal);
        }
        public void SetHtmlVertical(string htmlvertical){
            this.queryParameters.Add("htmlvertical", htmlvertical);
        }
        public void SetHtmlTitle(string htmltitle){
            this.queryParameters.Add("htmltitle", htmltitle);
        }

        private int signExpired;
        public void SetSignExpired(int signExpired)
        {
            signExpired = this.signExpired;
        }

        public int GetSignExpired()
        {
            return signExpired;
        }
    }
}
