namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using NUnit.Framework;

    /// <summary>
    /// Testing LogManualInsertAccount class.
    /// </summary>
    [TestFixture]
    public class LogManualInsertAccountTest
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\" SignupBy=\"{10}\"" +
                                                " AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} CustSite=\"{16}\" />";

        [Test]
        public void SpName_WhenCalled_ReturnName()
        {
            // Arrange
            var logModel = CreateNewInstance();

            // Assert
            const string SPNAME = "TP_User_Customer_ManualInsertAccount";
            Assert.AreEqual(SPNAME, logModel.SpName);
        }

        [Test]
        public void OldValue_WhenCalled_ReturnEmpty()
        {
            // Arrange
            var logModel = CreateNewInstance();

            // Assert
            Assert.IsNullOrEmpty(logModel.OldValue);
        }

        /// <summary>
        /// News the value_ game setting is null_ return game setting tracking empty.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        [TestCase((int)CustomerRole.Super)]
        [TestCase((int)CustomerRole.Master)]
        [TestCase((int)CustomerRole.Agent)]
        [TestCase((int)CustomerRole.Member)]
        public void NewValue_TargetRoleId_ReturnTracking(CustomerRole role)
        {
            // Arrange
            var logModel = CreateNewInstance();

            logModel.SettingContext.TargetRole = role;

            // Assert
            string expectedValue = string.Format(
                                           TrackingTemplate,
                                           logModel.SpName,
                                           0,
                                           string.Empty,
                                           string.Empty,
                                           0,
                                           0,
                                           0,
                                           0,
                                           0,
                                           string.Empty,
                                           0,
                                           0,
                                           string.Empty,
                                           string.Empty,
                                           string.Empty,
                                           logModel.TrackingSubAccount,
                                           string.Empty);
            Assert.AreEqual(expectedValue, logModel.NewValue);
        }

        private LogManualInsertAccount CreateNewInstance()
        {
            return new LogManualInsertAccount();
        }
    }
}