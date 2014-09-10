namespace Fanex.LiveCasino.CoreSetting.UnitTest.Controllers
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Controllers;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing LiveCSSettingController class.
    /// </summary>
    [TestFixture]
    public class LiveCSSettingControllerTest
    {
        private ISettingService _settingService;
        private IGameSettingService _gameSettingService;
        private IPTComSettingService _positionTakingAndCommissionSettingService;
        private IWinLimitSettingService _winLimitSettingService;
        private LiveCSSettingController _controller;

        [SetUp]
        public void SetUp()
        {
            _settingService = Substitute.For<ISettingService>();
            _positionTakingAndCommissionSettingService = Substitute.For<IPTComSettingService>();
            _gameSettingService = Substitute.For<IGameSettingService>();
            _winLimitSettingService = Substitute.For<IWinLimitSettingService>();
            _controller = Substitute.For<LiveCSSettingController>(_settingService, _gameSettingService);
        }

        /// <summary>
        /// Gets the current win limit_ when called_ call get current win limit.
        /// </summary>
        [Test]
        public void GetCurrentWinLimit_WhenCalled_CallGetCurrentWinLimit()
        {
            // Arrange
            var custId = 1;

            // Action
            _controller.GetCurrentWinLimit(custId);

            // Assert
            _winLimitSettingService.Received().GetCurrentWinLimit(custId);
        }

        /// <summary>
        /// Updates the win limit_ single update_ call update win limit.
        /// </summary>
        [Test]
        public void UpdateWinLimit_SingleUpdate_CallUpdateWinLimit()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var memberWinLimit = Substitute.For<MemberWinLimit>();

            settingContext.IsEditMultiple = false;

            // Action
            _controller.UpdateWinLimit(settingContext, memberWinLimit);

            // Assert
            _winLimitSettingService.Received().UpdateWinLimit(settingContext, memberWinLimit);
        }

        /// <summary>
        /// Updates the win limit_ multiple update_ call update win limit multiple.
        /// </summary>
        [Test]
        public void UpdateWinLimit_MultipleUpdate_CallUpdateWinLimitMultiple()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var memberWinLimit = Substitute.For<MemberWinLimit>();

            settingContext.IsEditMultiple = true;

            // Action
            _controller.UpdateWinLimit(settingContext, memberWinLimit);

            // Assert
            _winLimitSettingService.Received().UpdateWinLimitMultiple(settingContext, memberWinLimit);
        }

        /// <summary>
        /// Updates the game setting_ single update_ call update manual.
        /// </summary>
        [Test]
        public void UpdateGameSetting_SingleUpdate_CallUpdateManual()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.IsEditMultiple = false;

            // Action
            _controller.UpdateGameSetting(settingContext, gameSetting);

            // Assert
            _gameSettingService.Received().UpdateManual(settingContext, Arg.Any<Customer>(), gameSetting);
        }

        /// <summary>
        /// Updates the game setting_ multiple update_ call update manual multiple.
        /// </summary>
        [Test]
        public void UpdateGameSetting_MultipleUpdate_CallUpdateManualMultiple()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.IsEditMultiple = true;

            // Action
            _controller.UpdateGameSetting(settingContext, gameSetting);

            // Assert
            _gameSettingService.Received().UpdateManualMultiple(settingContext, gameSetting);
        }

        /// <summary>
        /// Updates the live casino setting_ update single and PT COM fail_ call update PT and COM setting.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoSetting_UpdateSingleAndPTComFail_CallUpdatePtAndComSetting()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var customer = Substitute.For<Customer>();

            settingContext.IsEditMultiple = false;

            _positionTakingAndCommissionSettingService.UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>())
                           .Returns(new CommandResults
                           {
                               Results = new List<ICommandResult>() { new CommandResult { Code = 1 } }
                           });

            // Action
            _controller.UpdateLiveCasinoSetting(settingContext, customer);

            // Assert
            _positionTakingAndCommissionSettingService.Received().UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>());
        }

        /// <summary>
        /// Updates the live casino setting_ update single and PT COM fail_ dot not call update win limit.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoSetting_UpdateSingleAndPTComFail_DotNotCallUpdateWinLimit()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var customer = Substitute.For<Customer>();

            settingContext.IsEditMultiple = false;

            _positionTakingAndCommissionSettingService.UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>())
                           .Returns(new CommandResults
                           {
                               Results = new List<ICommandResult>() { new CommandResult { Code = 1 } }
                           });

            // Action
            _controller.UpdateLiveCasinoSetting(settingContext, customer);

            // Assert
            _winLimitSettingService.DidNotReceive().UpdateWinLimit(Arg.Any<SettingContext>(), Arg.Any<MemberWinLimit>());
        }

        /// <summary>
        /// Updates the live casino setting_ update single and PT COM fail_ dot not call update manual.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoSetting_UpdateSingleAndPTComFail_DotNotCallUpdateManual()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var customer = Substitute.For<Customer>();

            settingContext.IsEditMultiple = false;

            _positionTakingAndCommissionSettingService.UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>())
                           .Returns(new CommandResults
                           {
                               Results = new List<ICommandResult>() { new CommandResult { Code = 1 } }
                           });

            // Action
            _controller.UpdateLiveCasinoSetting(settingContext, customer);

            // Assert
            _gameSettingService.DidNotReceive().UpdateManual(Arg.Any<SettingContext>(), Arg.Any<Customer>(), Arg.Any<List<GameType>>());
        }

        /// <summary>
        /// Updates the live casino setting_ update single and PT COM success_ call update manual.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoSetting_UpdateSingleAndPTComSuccess_CallUpdateManual()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var customer = Substitute.For<Customer>();

            customer.MemberWinLimit = new MemberWinLimit();
            settingContext.IsEditMultiple = false;
            _positionTakingAndCommissionSettingService.UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>())
                         .Returns(new CommandResults
                         {
                             Results = new List<ICommandResult>() { new CommandResult { Code = 0 } }
                         });
            _winLimitSettingService.UpdateWinLimit(Arg.Any<SettingContext>(), Arg.Any<MemberWinLimit>())
                            .Returns(new CommandResults
                            {
                                Results = new List<ICommandResult>() { new CommandResult { Code = string.Empty } }
                            });
            _gameSettingService.UpdateManual(Arg.Any<SettingContext>(), Arg.Any<Customer>(), Arg.Any<List<GameType>>())
                               .Returns(new CommandResults
                               {
                                   Results = new List<ICommandResult>() { new CommandResult { Code = string.Empty } }
                               });

            // Action
            _controller.UpdateLiveCasinoSetting(settingContext, customer);

            // Assert
            _gameSettingService.Received().UpdateManual(Arg.Any<SettingContext>(), Arg.Any<Customer>(), Arg.Any<List<GameType>>());
        }

        /// <summary>
        /// Updates the live casino setting_ update single and PT COM success_ call update win limit.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoSetting_UpdateSingleAndPTComSuccess_CallUpdateWinLimit()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var customer = Substitute.For<Customer>();

            customer.MemberWinLimit = new MemberWinLimit();
            settingContext.IsEditMultiple = false;
            _positionTakingAndCommissionSettingService.UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>())
                           .Returns(new CommandResults
                           {
                               Results = new List<ICommandResult>() { new CommandResult { Code = 0 } }
                           });
            _winLimitSettingService.UpdateWinLimit(Arg.Any<SettingContext>(), Arg.Any<MemberWinLimit>())
                            .Returns(new CommandResults
                            {
                                Results = new List<ICommandResult>() { new CommandResult { Code = string.Empty } }
                            });
            _gameSettingService.UpdateManual(Arg.Any<SettingContext>(), Arg.Any<Customer>(), Arg.Any<List<GameType>>())
                               .Returns(new CommandResults
                                {
                                    Results = new List<ICommandResult>() { new CommandResult { Code = string.Empty } }
                                });

            // Action
            _controller.UpdateLiveCasinoSetting(settingContext, customer);

            // Assert
            _winLimitSettingService.Received().UpdateWinLimit(Arg.Any<SettingContext>(), Arg.Any<MemberWinLimit>());
        }

        /// <summary>
        /// Updates the live casino setting_ update single and is edit member not synced and up line enable l c_ call update live casino status.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoSetting_UpdateSingleAndIsEditMemberNotSyncedAndUplineEnableLC_CallUpdateLiveCasinoStatus()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var customer = Substitute.For<Customer>();

            customer.IsSyncLiveCasino = true;
            customer.UplineDisableLiveCasino = false;
            settingContext.IsEditMultiple = false;
            settingContext.TargetRoleId = (int)CustomerRole.Member;
            settingContext.UplineRoleId = (int)CustomerRole.Agent;
            _positionTakingAndCommissionSettingService.UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>())
                           .Returns(new CommandResults
                           {
                               Results = new List<ICommandResult>() { new CommandResult { Code = 0 } }
                           });
            _winLimitSettingService.UpdateWinLimit(Arg.Any<SettingContext>(), Arg.Any<MemberWinLimit>())
                            .Returns(new CommandResults
                            {
                                Results = new List<ICommandResult>() { new CommandResult { Code = string.Empty } }
                            });
            _gameSettingService.UpdateManual(Arg.Any<SettingContext>(), Arg.Any<Customer>(), Arg.Any<List<GameType>>())
                               .Returns(new CommandResults
                               {
                                   Results = new List<ICommandResult>() { new CommandResult { Code = string.Empty } }
                               });

            // Action
            _controller.UpdateLiveCasinoSetting(settingContext, customer);

            // Assert
            _gameSettingService.Received().UpdateLiveCasinoStatus(Arg.Any<SettingContext>(), Arg.Any<bool>());
        }

        /// <summary>
        /// Update PT And COM Setting When Call Update Single Called Update Single Customer PT And COM Setting.
        /// </summary>
        [Test]
        public void UpdatePtAndComSetting_WhenCallUpdateSingle_CalledUpdateSingleCustomerPtAndComSetting()
        {
            //// Arrange
            var settingContext = Substitute.For<SettingContext>();
            var positionTakingAndComSetting = Substitute.For<PTComSetting>();

            //// Act
            _controller.UpdatePtAndComSetting(settingContext, positionTakingAndComSetting);

            //// Assert
            _positionTakingAndCommissionSettingService.Received().UpdatePtAndComSetting(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>());
        }

        /// <summary>
        /// Update PT And COM Setting When Call Update multiple Called Update multiple Customer PT And COM Setting.
        /// </summary>
        [Test]
        public void UpdatePtAndComSettingMultiple_WhenCallUpdateMultiple_CalledUpdateMultipleCustomerPtAndComSetting()
        {
            //// Arrange
            var settingContext = Substitute.For<SettingContext>();
            var positionTakingAndComSetting = Substitute.For<PTComSetting>();
            settingContext.IsEditMultiple = true;

            //// Act
            _controller.UpdatePtAndComSetting(settingContext, positionTakingAndComSetting);

            //// Assert
            _positionTakingAndCommissionSettingService.Received().UpdatePtAndComSettingMultiple(Arg.Any<SettingContext>(), Arg.Any<PTComSetting>());
        }

        [Test]
        public void UpdateMultipleCustomerAllSetting_WhenCalled_CallUpdateSingleCustomerAllSetting()
        {
        }
    }
}