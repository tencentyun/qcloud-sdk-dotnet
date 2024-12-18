using System;

namespace COSXMLDemo
{
     public class Process
     {
            static void Main(string[] args)
            {
                Process Process = new Process();
                Process.SetEnvironmentVariable();
                Process.DoSomething();
            }

            public void DoSomething()
            {      
                // BucketPolicyModel.BucketPolicyMain();
                // DownloadObject.DownloadObjectMain();
                UploadObject.UploadObjectMain();
                // GetObjectUrlDemo.GetObjectUrlDemoMain();

                // DeleteObjectModel.DeleteObjectModelMain();
                // SelectObjectDemo.SelectObjectMain();

                // ListObjectModel.ListObjectModelMain();
                // DoesObjectExistModel.DoesObjectExistMain();
                // HeadObjectModel.HeadObjectMain();
                // ObjectRestoreModel.ObjectRestoreModelMain();
                // GetObjectUrlDemo.GetObjectUrlDemoMain();

                // CreateBucketModel.CreateBucketModelMain();//
                // DeleteBucketModel.DeleteObjectModelMain();//
                // DoesBucketExistModel.DoesBucketExistModelMain();//
                // HeadBucketModel.HeadBucketModelMain();//
                // ListBucketModel.ListBucketModelMain();//
                // BucketPolicyModel.BucketPolicyMain();

                // BucketVersioningModel.BucketVersioningMain();//
                // BucketLifecycleModel.BucketLifecycleMain();
                // BucketReplicationModel.BucketReplicationMain();
                // BucketLoggingModel.BucketLoggingMain();

                // BucketTaggingModel.BucketTaggingMain();
                // ObjectTaggingModel.ObjectTaggingMain();
                // BucketInventoryModel.BucketInventoryMain();
                // BucketDomainModel.BucketDomainMain();
                // PutObjectACLModel.PutObjectACLMain();//
                // BucketRefererModel.BucketRefererMain();//
                // BucketWebsiteModel.BucketWebsiteMain();
            }
         
            public void SetEnvironmentVariable()
            {
                Environment.SetEnvironmentVariable("UIN", "");
                Environment.SetEnvironmentVariable("APPID", "");
                Environment.SetEnvironmentVariable("COS_REGION", "");
                Environment.SetEnvironmentVariable("SECRET_ID", "");
                Environment.SetEnvironmentVariable("SECRET_KEY", "");
                Environment.SetEnvironmentVariable("BUCKET", "");
            }

        }
}