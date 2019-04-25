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
    public sealed class TransferConfig
    {
        internal long divisionForCopy = 5242880; // 5M

        internal long sliceSizeForCopy = 2097152; // 2M

        internal long divisionForUpload = 5242880; // 5M

        internal long sliceSizeForUpload = 1048576; // 1M


        public long DdivisionForCopy { get { return divisionForCopy; } set { divisionForCopy = value; } }

        public long DivisionForUpload { get { return divisionForUpload; } set { divisionForUpload = value; } }

        public long SliceSizeForCopy { get { return sliceSizeForCopy; } set { sliceSizeForCopy = value; } }

        public long SliceSizeForUpload { get { return sliceSizeForUpload; } set { sliceSizeForUpload = value; } }
    }

    public sealed class TransferManager
    {
        private TransferConfig transferConfig;
        private CosXml cosXml;
       
        public TransferManager(CosXml cosXmlServer, TransferConfig transferConfig)
        {
            if (cosXmlServer == null) throw new ArgumentNullException("CosXmlServer = null");
            if (transferConfig == null) throw new ArgumentNullException("TransferConfig = null");
            this.transferConfig = transferConfig;
            //COSXMLTask.InitCosXmlServer(cosXmlServer);
            this.cosXml = cosXmlServer;
        }

        public void Upload(COSXMLUploadTask uploader)
        {
            uploader.InitCosXmlServer(cosXml);
            uploader.SetDivision(transferConfig.divisionForUpload, transferConfig.sliceSizeForUpload);
            uploader.Upload();
        }

        public void Download(COSXMLDownloadTask downloader)
        {
            downloader.InitCosXmlServer(cosXml);
            downloader.Download();
        }

        public void Copy(COSXMLCopyTask copy)
        {
            copy.InitCosXmlServer(cosXml);
            copy.SetDivision(transferConfig.DdivisionForCopy, transferConfig.sliceSizeForCopy);
            copy.Copy();
        }
    }
}
