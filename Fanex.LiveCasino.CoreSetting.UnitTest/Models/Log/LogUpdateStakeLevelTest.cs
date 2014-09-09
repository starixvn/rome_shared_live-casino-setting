namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using NUnit.Framework;

    /// <summary>
    /// Testing LogUpdateStakeLevel class.
    /// </summary>
    [TestFixture]
    public class LogUpdateStakeLevelTest
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" {2} GameTypeId=\"{3}\" BetTypeId=\"{4}\" StakeLevel=\"{5}\"" +
                                                 " Ip=\"{6}\" Lang=\"{7}\" RoleID=\"{8}\" ErrCode=\"{9}\" ErrMsg=\"{10}\" />";

        [Test]
        public void SpName_WhenCalled_ReturnName()
        {
            // Arrange
            var logModel = CreateNewInstance();

            // Assert
            const string SPNAME = "TP_User_CustGameType_UpdStakeLevel";
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
        public void NewValue_TargetRoleId_ReturnNewValueTracking(int roleId)
        {
            // Arrange
            var logModel = CreateNewInstance();

            logModel.SettingContext.TargetRoleId = roleId;
            logModel.NewGameType = new GameType();

            // Assert
            string expectedValue = string.Format(
                                        TrackingTemplate,
                                         logModel.SpName,
                                         0,
                                         logModel.TrackingSubAccount,
                                         0,
                                         0,
                                         string.Empty,
                                         string.Empty,
                                         string.Empty,
                                         roleId,
                                         string.Empty,
                                         string.Empty);
            Assert.AreEqual(expectedValue, logModel.NewValue);
        }

        /// <summary>
        /// News the value_ target role id_ return old value tracking.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        [TestCase((int)CustomerRole.Super)]
        [TestCase((int)CustomerRole.Master)]
        [TestCase((int)CustomerRole.Agent)]
        [TestCase((int)CustomerRole.Member)]
        public void NewValue_TargetRoleId_ReturnOldValueTracking(int roleId)
        {
            // Arrange
            var logModel = CreateNewInstance();

            logModel.SettingContext.TargetRoleId = roleId;
            logModel.OldGameType = new GameType();

            // Assert
            string expectedValue = string.Format(
                                        TrackingTemplate,
                                         logModel.SpName,
                                         0,
                                         logModel.TrackingSubAccount,
                                         0,
                                         0,
                                         string.Empty,
                                         string.Empty,
                                         string.Empty,
                                         roleId,
                                         string.Empty,
                                         string.Empty);
            Assert.AreEqual(expectedValue, logModel.OldValue);
        }

        private LogUpdateStakeLevel CreateNewInstance()
        {
            return new LogUpdateStakeLevel();
        }
    }
}