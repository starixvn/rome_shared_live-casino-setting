namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing LogAutoInsertAccount class.
    /// </summary>
    [TestFixture]
    public class LogDataBaseTest
    {
        private const string TrackingSubAcc = "SubAccId=\"{0}\" SubAccName=\"{1}\"";

        /// <summary>
        /// Logs the data base_ new instance_ return new setting context.
        /// </summary>
        [Test]
        public void LogDataBase_NewInstance_ReturnNewSettingContext()
        {
            // Action
            var logModel = CreateNew();
            logModel.CopiedFrom = string.Empty;
            logModel.NewValue = string.Empty;
            logModel.OldValue = string.Empty;
            logModel.SpName = string.Empty;

            // Assert
            Assert.IsNotNull(logModel.SettingContext);
        }

        /// <summary>
        /// Tracking the sub account_ is edit super_ return tracking.
        /// </summary>
        [Test]
        public void TrackingSubAccount_IsEditSuper_ReturnTracking()
        {
            // Arrange
            var logModel = CreateNew();
            var settingContext = Substitute.For<SettingContext>();

            logModel.SettingContext = settingContext;
            settingContext.TargetRole = CustomerRole.Super;

            // Assert
            string expectedSubAccountTracking = string.Format(TrackingSubAcc, 0, string.Empty);
            Assert.AreEqual(expectedSubAccountTracking, logModel.TrackingSubAccount);
        }

        /// <summary>
        /// Tracking the sub account_ is not super_ return empty.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        [TestCase(CustomerRole.Master)]
        [TestCase(CustomerRole.Agent)]
        [TestCase(CustomerRole.Member)]
        public void TrackingSubAccount_IsNotSuper_ReturnEmpty(CustomerRole role)
        {
            // Arrange
            var logModel = CreateNew();
            var settingContext = Substitute.For<SettingContext>();

            logModel.SettingContext = settingContext;
            settingContext.TargetRole = role;

            // Assert
            Assert.IsEmpty(logModel.TrackingSubAccount);
        }

        private LogDataBase CreateNew()
        {
            return new LogDataBase();
        }
    }
}