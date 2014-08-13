namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Log
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using NUnit.Framework;

    /// <summary>
    /// Testing LogAutoInsertAccount class.
    /// </summary>
    [TestFixture]
    public class LogAutoInsertAccountTest
    {
        private const string GameTypeTemplate = "<GameType GameTypeId=\"{0}\" BetTypeId=\"{1}\" StakeLevel=\"{2}\" />";

        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\"" +
                                                " SignupBy=\"{10}\" AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} >{16}</{17}>";

        [Test]
        public void SpName_WhenCalled_ReturnName()
        {
            // Arrange
            var logModel = CreateNewInstance();

            // Assert
            const string SPNAME = "TP_User_Customer_AutoInsertAccount";
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
        [Test]
        public void NewValue_GameSettingIsNull_ReturnGameSettingTrackingEmpty()
        {
            // Arrange
            var logModel = CreateNewInstance();
            logModel.GameSetting = null;

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
                                           string.Empty,
                                           string.Empty,
                                           logModel.SpName);
            Assert.AreEqual(expectedValue, logModel.NewValue);
        }

        /// <summary>
        /// News the value_ game setting_ return value.
        /// </summary>
        [Test]
        public void NewValue_GameSetting_ReturnValue()
        {
            // Arrange
            var logModel = CreateNewInstance();
            logModel.GameSetting = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };

            // Assert
            string expectedGameTypeTracking = string.Format(GameTypeTemplate, 1, 1, string.Empty);

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
                                           string.Empty,
                                           expectedGameTypeTracking,
                                           logModel.SpName);
            Assert.AreEqual(expectedValue, logModel.NewValue);
        }

        private LogAutoInsertAccount CreateNewInstance()
        {
            return new LogAutoInsertAccount();
        }
    }
}