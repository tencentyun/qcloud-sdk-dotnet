using NUnit.Framework;
using COSXML.Utils;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Utils.Tests
{
    [TestFixture()]
    public class TimeUtilsTests
    {
        [Test()]
        public void GetCurrentTimeTest()
        {
            long days = TimeUtils.GetCurrentTime(TimeUnit.Days);
            long hours = TimeUtils.GetCurrentTime(TimeUnit.Hours);
            long minutes = TimeUtils.GetCurrentTime(TimeUnit.Minutes);
            long seconds = TimeUtils.GetCurrentTime(TimeUnit.Seconds);
            long milliseconds = TimeUtils.GetCurrentTime(TimeUnit.Milliseconds);
            
            
        }

        [Test()]
        public void GetFormatTimeTest()
        {
            long time = 10;
            TimeUtils.GetFormatTime("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", time, TimeUnit.Days);
            TimeUtils.GetFormatTime("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", time, TimeUnit.Hours);
            TimeUtils.GetFormatTime("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", time, TimeUnit.Minutes);
            TimeUtils.GetFormatTime("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", time, TimeUnit.Seconds);
        }
        

        [Test()]
        public void getMermSizeTest()
        {
            SystemUtils.getMemorySize();
        }
    }
    
}
