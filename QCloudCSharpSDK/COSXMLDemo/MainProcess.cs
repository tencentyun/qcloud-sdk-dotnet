
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
                // BucketPolicy.BucketPolicyMain();
                // DownloadObject.DownloadObjectMain();
                // UploadObject.UploadObjectMain();
                // GetObjectUrlDemo.GetObjectUrlDemoMain();

                // DeleteObjectModel.DeleteObjectModelMain();
                // SelectObjectDemo.SelectObjectMain();

                // ListObjectModel.ListObjectModelMain();
                // DoesObjectExistModel.DoesObjectExistMain();
                // HeadObjectModel.HeadObjectMain();
                // ObjectRestoreModel.ObjectRestoreModelMain();
                GetObjectUrlDemo.GetObjectUrlDemoMain();
            }
         
            public void SetEnvironmentVariable()
            {
                Environment.SetEnvironmentVariable("BUCKET", "");
                Environment.SetEnvironmentVariable("UIN", "");
                Environment.SetEnvironmentVariable("APPID", "");
                Environment.SetEnvironmentVariable("COS_REGION", "");
                Environment.SetEnvironmentVariable("SECRET_ID", "");
                Environment.SetEnvironmentVariable("SECRET_KEY", "");
            }

        }
}