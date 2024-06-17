using System;
using COSXML.Network;
using NUnit.Framework;

namespace COSXML.Utils.Tests;
[TestFixture]
public class ResponseTest
{


    [Test]
    public void HandleResponseHeader_IsCalled()
    {
        var response = new Response();
        response.HandleResponseHeader();

    }

    [Test]
    public void OnFinish_IsCalled()
    {
        var response = new Response();
        var exception = new Exception();
        response.OnFinish(true,exception);
    }
}