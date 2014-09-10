namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Services;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing LogService class.
    /// </summary>
    [TestFixture]
    public class LogServiceTest
    {
        private ISettingRepository _settingRepository;
        private ILogService _logService;

        [SetUp]
        public void SetUp()
        {
            _settingRepository = Substitute.For<ISettingRepository>();
            _logService = Substitute.For<LogService>(_settingRepository);
        }

        [Test]
        public void LogService_WhenCalled_NewDefaultValue()
        {
            // Action
            _logService = new LogService();
        }

        /// <summary>
        /// Logs the automatic insert account_ when called_ call write log.
        /// </summary>
        [Test]
        public void LogAutoInsertAccount_WhenCalled_CallWriteLog()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();

            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogAutoInsertAccount(settingContext, gameSetting);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.SpName == new LogAutoInsertAccount().SpName));
        }

        /// <summary>
        /// Logs the automatic insert account_ target role id_ return action identifier.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        [TestCase(CustomerRole.Master, LogActions.AutoInsert_Master)]
        [TestCase(CustomerRole.Agent, LogActions.AutoInsert_Agent)]
        [TestCase(CustomerRole.Member, LogActions.AutoInsert_Member)]
        public void LogAutoInsertAccount_TargetRoleId_ReturnActionId(int targetRoleId, int actionId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.TargetRoleId = targetRoleId;
            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogAutoInsertAccount(settingContext, gameSetting);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.ActionId == actionId));
        }

        /// <summary>
        /// Logs the insert from existing account_ when called_ call write log.
        /// </summary>
        [Test]
        public void LogInsertFromExistingAccount_WhenCalled_CallWriteLog()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();

            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogInsertFromExistingAccount(settingContext, gameSetting);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.SpName == new LogInsertFromExistingAccount().SpName));
        }

        /// <summary>
        /// Logs the insert from existing account_ target role id_ return action identifier.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        [TestCase(CustomerRole.Master, LogActions.AutoInsertExisting_Master)]
        [TestCase(CustomerRole.Agent, LogActions.AutoInsertExisting_Agent)]
        [TestCase(CustomerRole.Member, LogActions.AutoInsertExisting_Member)]
        public void LogInsertFromExistingAccount_TargetRoleId_ReturnActionId(int targetRoleId, int actionId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.TargetRoleId = targetRoleId;
            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogInsertFromExistingAccount(settingContext, gameSetting);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.ActionId == actionId));
        }

        /// <summary>
        /// Logs the manual insert account_ when called_ call write log.
        /// </summary>
        [Test]
        public void LogManualInsertAccount_WhenCalled_CallWriteLog()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();

            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogManualInsertAccount(settingContext);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.SpName == new LogManualInsertAccount().SpName));
        }

        /// <summary>
        /// Logs the manual insert account_ target role id_ return action identifier.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        [TestCase(CustomerRole.Super, LogActions.ManualInsert_Super)]
        [TestCase(CustomerRole.Master, LogActions.ManualInsert_Master)]
        [TestCase(CustomerRole.Agent, LogActions.ManualInsert_Agent)]
        [TestCase(CustomerRole.Member, LogActions.ManualInsert_Member)]
        public void LogManualInsertAccount_TargetRoleId_ReturnActionId(int targetRoleId, int actionId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();

            settingContext.TargetRoleId = targetRoleId;
            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogManualInsertAccount(settingContext);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.ActionId == actionId));
        }

        /// <summary>
        /// Logs the manual update stake level_ old game setting is null_ call write log.
        /// </summary>
        [Test]
        public void LogManualUpdateStakeLevel_OldGameSettingIsNull_CallWriteLog()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            GameType oldGameType = null;
            var newGameType = Substitute.For<GameType>();

            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogManualUpdateStakeLevel(settingContext, oldGameType, newGameType);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.SpName == new LogUpdateStakeLevel().SpName));
        }

        /// <summary>
        /// Logs the manual update stake level_ old game setting equal new game setting_ do not call write log.
        /// </summary>
        [Test]
        public void LogManualUpdateStakeLevel_OldGameSettingEqualNewGameSetting_DoNotCallWriteLog()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var oldGameType = Substitute.For<GameType>();
            var newGameType = Substitute.For<GameType>();

            oldGameType.BettingLimit = new List<StakeLevel>() { new StakeLevel { Id = 1, IsChecked = true, Range = "1-1000" } };
            newGameType.BettingLimit = new List<StakeLevel>() { new StakeLevel { Id = 1, IsChecked = true, Range = "1-1000" } };

            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogManualUpdateStakeLevel(settingContext, oldGameType, newGameType);

            // Assert
            _settingRepository.DidNotReceive().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.SpName == new LogUpdateStakeLevel().SpName));
        }

        /// <summary>
        /// Logs the manual update stake level_ target role id_ return action identifier.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        /// <param name="actionId">The action identifier.</param>
        [TestCase(CustomerRole.Super, LogActions.UpdateStakeLevel_Super)]
        [TestCase(CustomerRole.Master, LogActions.UpdateStakeLevel_Master)]
        [TestCase(CustomerRole.Agent, LogActions.UpdateStakeLevel_Agent)]
        [TestCase(CustomerRole.Member, LogActions.UpdateStakeLevel_Member)]
        public void LogManualUpdateStakeLevel_TargetRoleId_ReturnActionId(int targetRoleId, int actionId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            GameType oldGameType = null;
            var newGameType = Substitute.For<GameType>();

            settingContext.TargetRoleId = targetRoleId;
            _settingRepository.InsertOrUpdate(Arg.Any<InsertLogUserRunSPCriteria>()).Returns(new CommandResult());

            // Action
            _logService.LogManualUpdateStakeLevel(settingContext, oldGameType, newGameType);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<InsertLogUserRunSPCriteria>(x => x.ActionId == actionId));
        }
    }
}