using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;

namespace Tests.Connections
{
    /// <summary>
    /// Tests for issue #225: it has to be possible to disable the RDP idle timeout
    /// (never disconnect an idle session) by setting it to 0. Previously any value
    /// below 10 was forced up to 10, so 0 and -1 silently became 10.
    /// </summary>
    [TestClass]
    public class RdpTimeOutOptionsTests
    {
        [TestMethod]
        public void IdleTimeoutZero_SetGet_StaysZeroToDisableTimeout()
        {
            var options = new RdpTimeOutOptions { IdleTimeout = 0 };
            Assert.AreEqual(0, options.IdleTimeout, "0 disables the idle timeout and must not be forced up to the minimum.");
        }

        [TestMethod]
        public void IdleTimeoutNegative_SetGet_NormalizedToZero()
        {
            var options = new RdpTimeOutOptions { IdleTimeout = -5 };
            Assert.AreEqual(0, options.IdleTimeout, "A negative idle timeout is normalized to 0 (disabled).");
        }

        [TestMethod]
        public void IdleTimeoutBelowMinimumButPositive_SetGet_ClampedToMinimum()
        {
            var options = new RdpTimeOutOptions { IdleTimeout = 5 };
            Assert.AreEqual(10, options.IdleTimeout, "A positive value below the minimum is still clamped to 10; only 0 disables the timeout.");
        }

        [TestMethod]
        public void IdleTimeoutAboveMaximum_SetGet_ClampedToMaximum()
        {
            var options = new RdpTimeOutOptions { IdleTimeout = 1000 };
            Assert.AreEqual(240, options.IdleTimeout, "Idle timeout is clamped to the MsRdpClient maximum of 240 minutes.");
        }

        [TestMethod]
        public void IdleTimeoutWithinRange_SetGet_KeepsValue()
        {
            var options = new RdpTimeOutOptions { IdleTimeout = 120 };
            Assert.AreEqual(120, options.IdleTimeout, "A valid idle timeout has to be preserved.");
        }

        [TestMethod]
        public void DefaultIdleTimeout_IsTwoHundredForty()
        {
            var options = new RdpTimeOutOptions();
            Assert.AreEqual(240, options.IdleTimeout, "The default idle timeout is unchanged at 240 minutes.");
        }
    }
}
