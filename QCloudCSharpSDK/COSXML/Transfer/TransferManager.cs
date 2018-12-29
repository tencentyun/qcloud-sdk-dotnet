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
        internal int divisionForCopy = 5242880; // 5M

        internal int sliceSizeForCopy = 2097152; // 2M

        internal int divisionForUpload = 5242880; // 5M

        internal int sliceSizeForUpload = 1048576; // 1M


        public int DdivisionForCopy { get { return divisionForCopy; } set { divisionForCopy = value; } }

        public int DivisionForUpload { get { return divisionForUpload; } set { divisionForUpload = value; } }

        public int SliceSizeForCopy { get { return sliceSizeForCopy; } set { sliceSizeForCopy = value; } }

        public int SliceSizeForUpload { get { return sliceSizeForUpload; } set { sliceSizeForUpload = value; } }
    }

    public sealed class TransferManager
    {
        private TransferConfig transferConfig;
       
        public TransferManager(CosXml cosXmlServer, TransferConfig transferConfig)
        {
            if (cosXmlServer == null) throw new ArgumentNullException("CosXmlServer = null");
            if (transferConfig == null) throw new ArgumentNullException("TransferConfig = null");
            this.transferConfig = transferConfig;
            COSXMLTask.InitCosXmlServer(cosXmlServer);
        }

        public void Upload(COSXMLUploadTask uploader)
        {
            uploader.SetDivision(transferConfig.divisionForUpload, transferConfig.sliceSizeForUpload);
            uploader.Upload();
        }

        public void Download(COSXMLDownloadTask downloader)
        {
            downloader.Download();
        }

        public void Copy(COSXMLCopyTask copy)
        {
            copy.SetDivision(transferConfig.DdivisionForCopy, transferConfig.sliceSizeForCopy);
            copy.Copy();
        }
    }
}
