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
        }

        [Test()]
        public void GetFormatTimeTest()
        {

        }
    }
}
