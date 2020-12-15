using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using COSXML.Model;
using COSXML.CosException;


namespace COSXML.Transfer
{

    /// <summary>
    /// 高级传输，提供更方便的对象上传、下载、拷贝功能
    /// </summary>
    public sealed class TransferManager
    {
        private TransferConfig transferConfig;

        private CosXml cosXml;

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="cosXmlServer">COSXML服务</param>
        /// <param name="transferConfig">高级传输设置</param>
        public TransferManager(CosXml cosXmlServer, TransferConfig transferConfig)
        {

            if (cosXmlServer == null)
            {
                throw new ArgumentNullException("CosXmlServer = null");
            }

            if (transferConfig == null)
            {
                throw new ArgumentNullException("TransferConfig = null");
            }

            this.transferConfig = transferConfig;
            //COSXMLTask.InitCosXmlServer(cosXmlServer);
            this.cosXml = cosXmlServer;
        }

        /// <summary>
        /// 上传对象，封装了简单上传、分片上传功能。
        /// </summary>
        /// <param name="uploader"></param>
        [Obsolete("方法已废弃，请使用 UploadAsync 方法实现异步调用。")]
        public void Upload(COSXMLUploadTask uploader)
        {
            uploader.InitCosXmlServer(cosXml);
            uploader.SetDivision(transferConfig.DivisionForUpload, transferConfig.SliceSizeForUpload);
            uploader.Upload();
        }

        /// <summary>
        /// 异步上传对象，封装了简单上传、分片上传功能。
        /// </summary>
        /// <param name="uploader"></param>
        /// <returns></returns>
        public Task<COSXMLUploadTask.UploadTaskResult> UploadAsync(COSXMLUploadTask uploader)
        {
            var task = uploader.AsyncTask<COSXMLUploadTask.UploadTaskResult>();
            Upload(uploader);
            return task;
        }

        /// <summary>
        /// 下载对象
        /// </summary>
        /// <param name="downloader"></param>
        [Obsolete("方法已废弃，请使用 DownloadAsync 方法实现异步调用。")]
        public void Download(COSXMLDownloadTask downloader)
        {
            downloader.InitCosXmlServer(cosXml);
            downloader.Download();
        }

        /// <summary>
        /// 异步下载对象
        /// </summary>
        /// <param name="downloader"></param>
        /// <returns></returns>
        public Task<COSXMLDownloadTask.DownloadTaskResult> DownloadAsync(COSXMLDownloadTask downloader)
        {
            var task = downloader.AsyncTask<COSXMLDownloadTask.DownloadTaskResult>();
            Download(downloader);
            return task;
        }

        /// <summary>
        /// 拷贝对象，封装了简单拷贝、分片拷贝功能。
        /// </summary>
        /// <param name="copy"></param>
        [Obsolete("方法已废弃，请使用 CopyAsync 方法实现异步调用。")]
        public void Copy(COSXMLCopyTask copy)
        {
            copy.InitCosXmlServer(cosXml);
            copy.SetDivision(transferConfig.DdivisionForCopy, transferConfig.SliceSizeForCopy);
            copy.Copy();
        }

        /// <summary>
        /// 异步拷贝对象，封装了简单拷贝、分片拷贝功能。
        /// </summary>
        /// <param name="copyTask"></param>
        /// <returns></returns>
        public Task<COSXMLCopyTask.CopyTaskResult> CopyAsync(COSXMLCopyTask copyTask)
        {
            var task = copyTask.AsyncTask<COSXMLCopyTask.CopyTaskResult>();
            Copy(copyTask);
            return task;
        }
    }

    /// <summary>
    /// 高级传输任务设置
    /// </summary>
    public sealed class TransferConfig
    {
        // 5M
        private long divisionForCopy = 5242880;

        // 2M
        private long sliceSizeForCopy = 2097152;

        // 5M
        private long divisionForUpload = 5242880;

        // 1M
        private long sliceSizeForUpload = 1048576;

        /// <summary>
        /// 多大的文件会自动使用分片拷贝
        /// </summary>
        /// <value>默认是 5MB</value>
        public long DdivisionForCopy
        {
            get
            {
                return divisionForCopy;
            }
            set { divisionForCopy = value; }
        }

        /// <summary>
        /// 多大的文件会自动使用分片上传
        /// </summary>
        /// <value>默认是 2MB</value>
        public long DivisionForUpload
        {
            get
            {
                return divisionForUpload;
            }
            set { divisionForUpload = value; }
        }

        /// <summary>
        /// 每个分片拷贝任务的分片大小
        /// </summary>
        /// <value>默认是 5MB</value>
        public long SliceSizeForCopy
        {
            get
            {
                return sliceSizeForCopy;
            }
            set { sliceSizeForCopy = value; }
        }

        /// <summary>
        /// 每个分片上传任务的分片大小
        /// </summary>
        /// <value>默认是 1MB</value>
        public long SliceSizeForUpload
        {
            get
            {
                return sliceSizeForUpload;
            }
            set { sliceSizeForUpload = value; }
        }
    }
}
