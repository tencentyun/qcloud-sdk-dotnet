using NUnit.Framework;
using COSXML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COSXML.Utils.Tests
{
    [TestFixture()]
    public class TimeUtilsTests
    {
        [Test()]
        public void GetCurrentTimeTest()
        {
            long days = TimeUtils.GetCurrentTime(TimeUnit.DAYS);
            long hours = TimeUtils.GetCurrentTime(TimeUnit.HOURS);
            long minutes = TimeUtils.GetCurrentTime(TimeUnit.MINUTES);
            long seconds = TimeUtils.GetCurrentTime(TimeUnit.SECONDS);
            

        }

        [Test()]
        public void GetFormatTimeTest()
        {
            
        }
    }
}


/* ============================================================================== 
* Copyright 2016-2019 Tencent Cloud. All Rights Reserved.
* Auth：bradyxiao 
* Date：2019/1/22 17:32:45 
* ==============================================================================*/
