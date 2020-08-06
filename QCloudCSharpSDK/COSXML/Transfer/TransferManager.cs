using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/29/2018 5:09:07 PM
* bradyxiao
*/
namespace COSXML.Transfer
{
    /// <summary>
    /// 高级传输任务设置
    /// </summary>
    public sealed class TransferConfig
    {
        internal long divisionForCopy = 5242880; // 5M

        internal long sliceSizeForCopy = 2097152; // 2M

        internal long divisionForUpload = 5242880; // 5M

        internal long sliceSizeForUpload = 1048576; // 1M

        /// <summary>
        /// 多大的文件会自动使用分片拷贝
        /// </summary>
        /// <value>默认是 5MB</value>
        public long DdivisionForCopy { get { return divisionForCopy; } set { divisionForCopy = value; } }

        /// <summary>
        /// 多大的文件会自动使用分片上传
        /// </summary>
        /// <value>默认是 2MB</value>
        public long DivisionForUpload { get { return divisionForUpload; } set { divisionForUpload = value; } }

        /// <summary>
        /// 每个分片拷贝任务的分片大小
        /// </summary>
        /// <value>默认是 5MB</value>
        public long SliceSizeForCopy { get { return sliceSizeForCopy; } set { sliceSizeForCopy = value; } }

        /// <summary>
        /// 每个分片上传任务的分片大小
        /// </summary>
        /// <value>默认是 1MB</value>
        public long SliceSizeForUpload { get { return sliceSizeForUpload; } set { sliceSizeForUpload = value; } }
    }

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
            if (cosXmlServer == null) throw new ArgumentNullException("CosXmlServer = null");
            if (transferConfig == null) throw new ArgumentNullException("TransferConfig = null");
            this.transferConfig = transferConfig;
            //COSXMLTask.InitCosXmlServer(cosXmlServer);
            this.cosXml = cosXmlServer;
        }

        /// <summary>
        /// 上传对象，封装了简单上传、分片上传功能。
        /// </summary>
        /// <param name="uploader"></param>
        public void Upload(COSXMLUploadTask uploader)
        {
            uploader.InitCosXmlServer(cosXml);
            uploader.SetDivision(transferConfig.divisionForUpload, transferConfig.sliceSizeForUpload);
            uploader.Upload();
        }

        /// <summary>
        /// 下载对象
        /// </summary>
        /// <param name="downloader"></param>
        public void Download(COSXMLDownloadTask downloader)
        {
            downloader.InitCosXmlServer(cosXml);
            downloader.Download();
        }

        /// <summary>
        /// 拷贝对象，封装了简单拷贝、分片拷贝功能。
        /// </summary>
        /// <param name="copy"></param>
        public void Copy(COSXMLCopyTask copy)
        {
            copy.InitCosXmlServer(cosXml);
            copy.SetDivision(transferConfig.DdivisionForCopy, transferConfig.sliceSizeForCopy);
            copy.Copy();
        }
    }
}
