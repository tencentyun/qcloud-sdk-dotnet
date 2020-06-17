using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Utils;
using COSXML.Transfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace COSXMLTests
{
  [TestFixture()]
  public class RandomTest
  {

    [Test()]
    public void testRestoreHistoryObject()
    {
      try
      {
        RestoreObjectRequest request = new RestoreObjectRequest("000000000001",
          "2020-01-17.21-35-01.log");
        request.SetExpireDays(2);
        request.SetTier(RestoreConfigure.Tier.Standard);
        request.SetVersionId("MTg0NDUxNjQ1ODU5NjY2NDYyOTM");
        RestoreObjectResult result = QCloudServer.Instance().cosXml.RestoreObject(request);
        Console.WriteLine(result.GetResultInfo());
        Assert.True(true);
      }
      catch (COSXML.CosException.CosClientException clientEx)
      {
        Console.WriteLine("CosClientException: " + clientEx);
        Assert.True(false);
      }
      catch (COSXML.CosException.CosServerException serverEx)
      {
        Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        Assert.True(serverEx.statusCode == 404);
      }
    }
  }
}