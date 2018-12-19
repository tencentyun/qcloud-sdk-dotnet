using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 11:36:54 AM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    public sealed class Delete
    {
        public bool quiet;
        public List<DeleteObject> deleteObjects;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{Delete:\n");
            stringBuilder.Append("Quiet:").Append(quiet).Append("\n");
            if (deleteObjects != null)
            {
                foreach(DeleteObject deleteObject in deleteObjects)
                {
                    stringBuilder.Append(deleteObject.GetInfo());
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public sealed class DeleteObject
        {
            public string key;
            public string versionId;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Object:\n");
                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }
    }
}
