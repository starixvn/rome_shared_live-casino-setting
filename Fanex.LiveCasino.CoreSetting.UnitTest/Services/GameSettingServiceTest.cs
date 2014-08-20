namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using Fanex.NAP.Framework.Data;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing GameSettingService class.
    /// </summary>
    [TestFixture]
    public class GameSettingServiceTest
    {
        private ICommonUtil _commonUtil;
        private IGameSettingService _gameSettingService;
        private ILogService _logService;
        private ISettingRepository _settingRepository;

        [SetUp]
        public void SetUp()
        {
            _settingRepository = Substitute.For<ISettingRepository>();
            _logService = Substitute.For<ILogService>();
            _commonUtil = Substitute.For<ICommonUtil>();
            _gameSettingService = new GameSettingService(_settingRepository, _logService, _commonUtil);
        }

        /// <summary>
        /// Gets the customer site by username when input invalid username then return empty.
        /// </summary>
        /// <param name="username">The username.</param>
        [TestCase("")]
        [TestCase(null)]
        public void GetCustomerSiteByUsername_InvalidUsername_ReturnEmpty(string username)
        {
            // Action
            string actualSiteName = _gameSettingService.GetCustomerSiteByUsername(username);

            // Assert
            Assert.IsEmpty(actualSiteName);
        }

        /// <summary>
        /// Gets the name of the customer site by username valid username return site.
        /// </summary>
        [Test]
        public void GetCustomerSiteByUsername_ValidUsername_ReturnSiteName()
        {
            // Arrange
            const string SiteName = "BEST_ODDS";
            var username = "supertest1";

            _settingRepository.Get<CustInfo>(Arg.Is<GetCustomerSiteByUsernameCriteria>(x => x.IsValid()))
                              .Returns(new CustInfo { Site = SiteName });

            // Action
            string actualSiteName = _gameSettingService.GetCustomerSiteByUsername(username);

            // Assert
            Assert.AreEqual(SiteName, actualSiteName);
        }

        /// <summary>
        /// Gets the customer info when called return customer information.
        /// </summary>
        [Test]
        public void GetCustomerInfo_WhenCalled_ReturnCustInfo()
        {
            // Arrange
            var custId = 1;
            var expectedCustInfo = new CustInfo
            {
                CurrencyId = 20,
                CustId = 1,
                UserName = "supertest1"
            };

            _settingRepository.GetCustomerInfo(new GetCustomerInfoCriteria()).ReturnsForAnyArgs(expectedCustInfo);

            // Action
            CustInfo actualCustInfo = _gameSettingService.GetCustomerInfo(custId);

            // Assert            
            AssertCustInfo(expectedCustInfo, actualCustInfo);
        }

        /// <summary>
        /// Gets the game setting by customer id when called return game setting.
        /// </summary>
        [Test]
        public void GetGameSettingByCustomerId_WhenCalled_ReturnGameSetting()
        {
            // Arrange
            var custId = 1;
            var expectedGameSetting = new List<GameType>();
            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000" },
                    new StakeLevel { Id = 2, Range = "10-1000" }
                }
            });

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid()), hasCheckedColumn: true)
                              .Returns(expectedGameSetting);

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSettingByCustomerId(custId);

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Gets the game setting by exchange when called return game setting.
        /// </summary>
        [Test]
        public void GetGameSettingByExchange_WhenCalled_ReturnGameSetting()
        {
            // Arrange
            var exchangeId = 1;
            var expectedGameSetting = new List<GameType>();
            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000" },
                    new StakeLevel { Id = 2, Range = "10-1000" }
                }
            });

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByExchangeCriteria>(x => x.IsValid()), hasCheckedColumn: false)
                              .Returns(expectedGameSetting);

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSettingByExchange(exchangeId, Arg.Any<string>());

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Gets the game setting when super is not synced then return game setting by exchange and checked all stake level.
        /// </summary>
        [Test]
        public void GetGameSetting_SuperIsNotSynced_ReturnGameSettingByExchangeAndCheckedAllStakeLevel()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnGameSetting = new List<GameType>();

            settingContext.TargetRoleId = (int)CustRole.Super;

            custInfo.UserName = "supertest1";
            custInfo.Site = "BEST-ODDS";
            custInfo.CurrencyId = 20;
            custInfo.IsSyncLiveCasino = false;

            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = true },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = true }
                }
            });

            returnGameSetting = CloneGameSetting(expectedGameSetting);
            returnGameSetting[0].BettingLimit[0].IsChecked = false;
            returnGameSetting[0].BettingLimit[1].IsChecked = false;

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByExchangeCriteria>(x => x.IsValid()), hasCheckedColumn: false)
                              .Returns(returnGameSetting);
            _settingRepository.Get<CustInfo>(Arg.Is<GetCustomerSiteByUsernameCriteria>(x => x.IsValid()))
                              .Returns(new CustInfo { Site = custInfo.Site });

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Gets the game setting when target customer is not synced then return direct up line game setting.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        public void GetGameSetting_TargetCustIsNotSynced_ReturnDirectUplineGameSetting(int targetRoleId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnGameSetting = new List<GameType>();

            settingContext.DirectUplineId = 1;
            settingContext.TargetRoleId = targetRoleId;
            custInfo.IsSyncLiveCasino = false;

            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = true },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = true }
                }
            });

            returnGameSetting = CloneGameSetting(expectedGameSetting);

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), hasCheckedColumn: true)
                              .Returns(returnGameSetting);

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Gets the game setting when target customer is not synced then return game setting be removed game type has all stake level unchecked.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        public void GetGameSetting_TargetCustIsNotSynced_ReturnGameSettingBeRemovedGameTypeHasAllStakeLevelUnchecked(int targetRoleId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnGameSetting = new List<GameType>();

            settingContext.DirectUplineId = 1;
            settingContext.TargetRoleId = targetRoleId;
            custInfo.IsSyncLiveCasino = false;
            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = true },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = true }
                }
            });

            returnGameSetting = CloneGameSetting(expectedGameSetting);
            returnGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = false },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = false }
                }
            });

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), hasCheckedColumn: true)
                              .Returns(returnGameSetting);

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Gets the game setting when member is not synced then return direct up line game setting with lowest stake level is checked.
        /// </summary>
        [Test]
        public void GetGameSetting_MemberIsNotSynced_ReturnDirectUplineGameSettingWithLowestStakeLevelIsChecked()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnGameSetting = new List<GameType>();

            settingContext.DirectUplineId = 1;
            settingContext.TargetRoleId = (int)CustRole.Member;
            custInfo.IsSyncLiveCasino = false;
            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = true }
                }
            });

            returnGameSetting = CloneGameSetting(expectedGameSetting);
            returnGameSetting[0].BettingLimit.Insert(0, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = false });
            returnGameSetting[0].BettingLimit.Insert(1, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = false });
            returnGameSetting[0].BettingLimit.Insert(3, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = true });
            returnGameSetting[0].BettingLimit.Insert(4, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = true });
            returnGameSetting[0].BettingLimit.Insert(5, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = true });

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), hasCheckedColumn: true)
                              .Returns(returnGameSetting);

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Gets the game setting when target customer is synced then return game setting.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Super)]
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        [TestCase(CustRole.Member)]
        public void GetGameSetting_TargetCustIsSynced_ReturnGameSetting(int targetRoleId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnGameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            settingContext.TargetRoleId = targetRoleId;
            custInfo.IsSyncLiveCasino = true;
            expectedGameSetting.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = true },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = true },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = false }
                }
            });

            returnGameSetting = CloneGameSetting(expectedGameSetting);

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), hasCheckedColumn: true)
                              .Returns(returnGameSetting);

            // Action
            List<GameType> actualGameSetting = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualGameSetting);
        }

        /// <summary>
        /// Inserts the automatic account when target customer is super then return invalid customer command result.
        /// </summary>
        [Test]
        public void InsertAutoAccount_TargetCustIsSuper_ReturnInvalidCustomerCommandResult()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();

            settingContext.TargetRoleId = (int)CustRole.Super;

            // Action
            ICommandResult actualCommandResult = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            var expectedCommandResult = new CommandResult
            {
                Code = ErrCode.InvalidCustomer,
                Message = LiveCasinoLang.InvalidCustomer
            };

            AssertCommandResult(expectedCommandResult, actualCommandResult);
        }

        /// <summary>
        /// Inserts the automatic account_ insert success and game setting not empty_ call write log.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        [TestCase(CustRole.Member)]
        public void InsertAutoAccount_InsertSuccessAndGameSettingNotEmpty_CallWriteLog(int targetRoleId)
        {
            _settingRepository = Substitute.For<ISettingRepository>();
            _logService = Substitute.For<ILogService>();
            _commonUtil = Substitute.For<ICommonUtil>();
            _gameSettingService = Substitute.For<GameSettingService>(_settingRepository, _logService, _commonUtil);

            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            settingContext.TargetRoleId = targetRoleId;

            _commonUtil.LiveCasinoSettings.ReturnsForAnyArgs(new LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(new InsertAutoAccountCriteria(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>() 
                                  { 
                                      new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } 
                                  };
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });

            _settingRepository.InsertOrUpdate(new UpdateSyncLiveCasinoCriteria())
                              .ReturnsForAnyArgs(new CommandResult { Code = 1 });
            _logService.LogAutoInsertAccount(new SettingContext(), new List<GameType>());
            _commonUtil.Sleep(0);

            // Action
            ICommandResult actualCommandResult = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert                 
            _logService.ReceivedWithAnyArgs().LogAutoInsertAccount(new SettingContext(), new List<GameType>());
        }

        /// <summary>
        /// Inserts the automatic account_ insert success and game setting empty_ asynchronous call get game setting.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        [TestCase(CustRole.Member)]
        public void InsertAutoAccount_InsertSuccessAndGameSettingEmpty_AsyncCallGetGameSetting(int targetRoleId)
        {
            // Arrange
            var settingContext = new SettingContext();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            settingContext.TargetRoleId = targetRoleId;

            _commonUtil.LiveCasinoSettings.ReturnsForAnyArgs(new LiveCasinoSettings { SystemId = 1 });
            var agr0 = Arg.Any<InsertAutoAccountCriteria>();
            var agr1 = Arg.Any<InsertAutoAccountCriteria>();
            _settingRepository.InsertGameSettingAccount(agr0, ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>();
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });
            _settingRepository.GetGameSetting(new GetGameSettingByCustomerIdCriteria(), hasCheckedColumn: true)
                              .ReturnsForAnyArgs(new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } });
            _settingRepository.InsertOrUpdate(new UpdateSyncLiveCasinoCriteria())
                              .ReturnsForAnyArgs(new CommandResult { Code = 1 });
            _commonUtil.Sleep(0);

            // Action
            var actualValue = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            _settingRepository.ReceivedWithAnyArgs().GetGameSetting(new GetGameSettingByCustomerIdCriteria { CustId = 1 }, hasCheckedColumn: true);
        }

        /// <summary>
        /// Inserts the automatic account_ insert success and return game setting list empty_ call get game setting asynchronous and get default.
        /// </summary>
        [Test]
        public void InsertAutoAccount_InsertSuccessAndReturnGameSettingListEmpty_CallGetGameSettingAsyncAndGetDefault()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            settingContext.ExchangeId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertAutoAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>();
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });
            _settingRepository.GetGameSetting(Arg.Any<GetGameSettingByCustomerIdCriteria>(), Arg.Any<bool>()).Returns(new List<GameType>());
            _settingRepository.GetGameSetting(Arg.Any<GetGameSettingByExchangeCriteria>(), Arg.Any<bool>())
                              .Returns(new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).ReturnsForAnyArgs(new CommandResult { Code = 0 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            _settingRepository.Received().GetGameSetting(Arg.Any<GetGameSettingByExchangeCriteria>(), false);
        }

        /// <summary>
        /// Inserts the automatic account_ insert success_ call update synchronize live casino.
        /// </summary>
        [Test]
        public void InsertAutoAccount_InsertSuccess_CallUpdateSyncLiveCasino()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertAutoAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });

            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 1 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<UpdateSyncLiveCasinoCriteria>(x => x.SyncLiveCS));
        }

        /// <summary>
        /// Inserts the automatic account_ insert success_ call update synchronize live casino success call update live casino status.
        /// </summary>
        [Test]
        public void InsertAutoAccount_InsertSuccess_CallUpdateSyncLiveCasinoSucessCallUpdateLiveCasinoStatus()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertAutoAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });

            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>());
        }

        /// <summary>
        /// Inserts the automatic account_ insert fail_ do nothing.
        /// </summary>
        [Test]
        public void InsertAutoAccount_InsertFail_DoNothing()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Is<InsertAutoAccountCriteria>(x => x.IsValid()), ref gameSetting)
                              .Returns(x =>
                              {
                                  x[1] = new List<GameType>();
                                  return new GameSettingCommandResult
                                  {
                                      Code = "E0001",
                                      Message = "CustID is null"
                                  };
                              });

            // Action
            var actualValue = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            Assert.IsFalse(actualValue.IsSuccessful);
        }

        /// <summary>
        /// Inserts the automatic account_ when called_ call insert game setting account.
        /// </summary>
        [Test]
        public void InsertAutoAccount_WhenCalled_CallInsertGameSettingAccount()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertAutoAccountCriteria>(), ref gameSetting)
                             .ReturnsForAnyArgs(x =>
                             {
                                 x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                 return new GameSettingCommandResult
                                 {
                                     Code = "1"
                                 };
                             });

            // Action
            var actualCommandResult = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            _settingRepository.ReceivedWithAnyArgs().InsertGameSettingAccount(Arg.Any<InsertAutoAccountCriteria>(), ref gameSetting);
        }

        /// <summary>
        /// Inserts the automatic account_ insert success_ call write log.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_InsertSuccessAndReturnGameSettingList_CallWriteLog()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });

            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).ReturnsForAnyArgs(new CommandResult { Code = 0 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            _logService.Received().LogInsertFromExistingAccount(Arg.Any<SettingContext>(), Arg.Any<List<GameType>>());
        }

        /// <summary>
        /// Inserts from existing account_ insert success and return game setting list empty_ call get game setting asynchronous.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_InsertSuccessAndReturnGameSettingListEmpty_CallGetGameSettingAsync()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>();
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });
            _settingRepository.GetGameSetting(Arg.Any<GetGameSettingByCustomerIdCriteria>(), Arg.Any<bool>())
                              .ReturnsForAnyArgs(new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).ReturnsForAnyArgs(new CommandResult { Code = 0 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            _settingRepository.Received().GetGameSetting(Arg.Any<GetGameSettingByCustomerIdCriteria>(), true);
        }

        /// <summary>
        /// Inserts from existing account_ insert success and return game setting list empty_ call get game setting asynchronous and get default.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_InsertSuccessAndReturnGameSettingListEmpty_CallGetGameSettingAsyncAndGetDefault()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            settingContext.ExchangeId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>();
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });
            _settingRepository.GetGameSetting(Arg.Any<GetGameSettingByCustomerIdCriteria>(), Arg.Any<bool>()).Returns(new List<GameType>());
            _settingRepository.GetGameSetting(Arg.Any<GetGameSettingByExchangeCriteria>(), Arg.Any<bool>())
                              .Returns(new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).ReturnsForAnyArgs(new CommandResult { Code = 0 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            _settingRepository.Received().GetGameSetting(Arg.Any<GetGameSettingByExchangeCriteria>(), false);
        }

        /// <summary>
        /// Inserts from existing account_ insert success_ call update synchronize live casino.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_InsertSuccess_CallUpdateSyncLiveCasino()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });

            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 1 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Is<UpdateSyncLiveCasinoCriteria>(x => x.SyncLiveCS));
        }

        /// <summary>
        /// Inserts from existing account_ insert success_ call update synchronize live casino success call update live casino status.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_InsertSuccess_CallUpdateSyncLiveCasinoSucessCallUpdateLiveCasinoStatus()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting)
                              .ReturnsForAnyArgs(x =>
                              {
                                  x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                  return new GameSettingCommandResult
                                  {
                                      Code = string.Empty
                                  };
                              });

            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });
            _commonUtil.Sleep(Arg.Any<int>());

            // Action
            var actualValue = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>());
        }

        /// <summary>
        /// Inserts the automatic account_ insert fail_ do nothing.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_InsertFail_DoNothing()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            settingContext.TargetCustId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Is<InsertFromExistingAccountCriteria>(x => x.IsValid()), ref gameSetting)
                              .Returns(x =>
                              {
                                  x[1] = new List<GameType>();
                                  return new GameSettingCommandResult
                                  {
                                      Code = "E0001",
                                      Message = "CustID is null"
                                  };
                              });

            // Action
            var actualValue = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            Assert.IsFalse(actualValue.IsSuccessful);
        }

        /// <summary>
        /// Inserts from existing account_ when called_ call insert game setting account.
        /// </summary>
        [Test]
        public void InsertFromExistingAccount_WhenCalled_CallInsertGameSettingAccount()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = new List<GameType>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting)
                             .ReturnsForAnyArgs(x =>
                             {
                                 x[1] = new List<GameType>() { new GameType { Id = 1, BetTypeId = 1, BettingLimit = new List<StakeLevel>() } };
                                 return new GameSettingCommandResult
                                 {
                                     Code = "1"
                                 };
                             });

            // Action
            var actualCommandResult = _gameSettingService.InsertFromExistingAccount(settingContext);

            // Assert
            _settingRepository.ReceivedWithAnyArgs().InsertGameSettingAccount(Arg.Any<InsertFromExistingAccountCriteria>(), ref gameSetting);
        }

        /// <summary>
        /// Updates the live casino status_ insert success_ return success command result.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoStatus_InsertSuccess_ReturnSuccessCommandResult()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            settingContext.TargetCustId = 1;

            _settingRepository.InsertOrUpdate(Arg.Is<UpdateLiveCasinoStatusCriteria>(x => x.IsValid()))
                              .Returns(new CommandResult { Code = 0 });

            // Action
            var updateResult = _gameSettingService.UpdateLiveCasinoStatus(settingContext, Arg.Any<bool>());

            // Assert
            Assert.AreEqual(updateResult.Message, LiveCasinoLang.UpdateLiveCasinoStatusSuccess);
        }

        /// <summary>
        /// Updates the live casino status_ update fail_ return error command result.
        /// </summary>
        [Test]
        public void UpdateLiveCasinoStatus_UpdateFail_ReturnErrorCommandResult()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            settingContext.TargetCustId = -1;

            _settingRepository.InsertOrUpdate(Arg.Is<UpdateLiveCasinoStatusCriteria>(x => !x.IsValid()))
                              .Returns(new CommandResult { Code = ErrCode.InvalidCriteria });

            // Action
            var updateResult = _gameSettingService.UpdateLiveCasinoStatus(settingContext, Arg.Any<bool>());

            // Assert
            Assert.IsFalse(updateResult.IsSuccessful);
        }

        /// <summary>
        /// Updates the manual_ customer is synced_ call get game setting before update.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsSynced_CallGetGameSettingBeforeUpdate()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();

            settingContext.TargetCustId = 1;
            custInfo.IsSyncLiveCasino = true;

            // Action
            _gameSettingService.UpdateManual(settingContext, custInfo, Arg.Any<List<GameType>>());

            // Assert
            _gameSettingService.Received().GetGameSettingByCustomerId(settingContext.TargetCustId);
        }

        /// <summary>
        /// Updates the manual_ when called_ call update stake level.
        /// </summary>
        [Test]
        public void UpdateManual_WhenCalled_CallUpdateStakeLevel()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();

            settingContext.TargetCustId = 1;
            custInfo.IsSyncLiveCasino = true;

            // Action
            _gameSettingService.UpdateManual(settingContext, custInfo, Arg.Any<List<GameType>>());

            // Assert
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert fail return do not call write log.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertFailReturnDoNotCallWriteLog()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 1 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _logService.DidNotReceive().LogManualInsertAccount(Arg.Any<SettingContext>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert fail return do not update synchronize live casino.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertFailReturnDoNotUpdateSyncLiveCasino()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 1 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.DidNotReceive().InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert fail return do not update live casino status.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertFailReturnDoNotUpdateLiveCasinoStatus()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 1 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.DidNotReceive().InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert success call write log.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertSuccessCallWriteLog()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _logService.Received().LogManualInsertAccount(Arg.Any<SettingContext>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert success call update synchronize live casino.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertSuccessCallUpdateSyncLiveCasino()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert success 
        /// and update synchronize live casino fail dot not call update live casino status.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertSuccessAndUpdateSyncLiveCasinoFailDotNotCallUpdateLiveCasinoStatus()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.DidNotReceive().InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced_ call insert manual account and insert success 
        /// and update synchronize live casino success call update live casino status.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSynced_CallInsertManualAccountAndInsertSuccessAndUpdateSyncLiveCasinoSuccessCallUpdateLiveCasinoStatus()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.Received().InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced and not super_ call insert manual account 
        /// and insert success and update synchronize live casino success call update live casino status_ will cal get customer information.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSyncedAndNotSuper_CallInsertManualAccountAndSuccessAndUpdateSyncLCSuccessCallUpdateLCStatus_WillCalGetCustomerInfo()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.TargetRoleId = (int)CustRole.Member;
            settingContext.DirectUplineId = 1;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.GetCustomerInfo(Arg.Any<GetCustomerInfoCriteria>()).Returns(new CustInfo());

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.Received().GetCustomerInfo(Arg.Any<GetCustomerInfoCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced and is super_ call insert manual account 
        /// and insert success and update synchronize live casino success call update live casino status_ will not cal get customer information.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSyncedAndIsSuper_CallInsertManualAccountAndSuccessAndUpdateSyncLCSuccessCallUpdateLCStatus_WillNotCalGetCustomerInfo()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.TargetRoleId = (int)CustRole.Super;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.GetCustomerInfo(Arg.Any<GetCustomerInfoCriteria>()).Returns(new CustInfo());

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.DidNotReceive().GetCustomerInfo(Arg.Any<GetCustomerInfoCriteria>());
        }

        /// <summary>
        /// Updates the manual_ customer is not synced and game setting is null_ call get customer info_ direct up line.
        /// </summary>
        [Test]
        public void UpdateManual_CustomerIsNotSyncedAndGameSettingIsNull_CallGetCustomerInfo_DirectUpline()
        {
            // Arragen
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var gameSetting = Substitute.For<List<GameType>>();

            settingContext.TargetRoleId = (int)CustRole.Super;
            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<InsertManualAccountCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.InsertOrUpdate(Arg.Any<UpdateLiveCasinoStatusCriteria>()).Returns(new CommandResult { Code = 0 });
            _settingRepository.GetCustomerInfo(Arg.Any<GetCustomerInfoCriteria>()).Returns(new CustInfo { IsSyncLiveCasino = true });

            // Action
            var updateCommand = _gameSettingService.UpdateManual(settingContext, custInfo, gameSetting);

            // Assert
            _settingRepository.Received().GetCustomerInfo(Arg.Any<GetCustomerInfoCriteria>());
        }

        /// <summary>
        /// Updates the manual multiple_ list customer information is empty_ do nothing.
        /// </summary>
        [Test]
        public void UpdateManualMultiple_ListCustInfoIsEmpty_DoNothing()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var gameSetting = Substitute.For<List<GameType>>();
            var custInfo = Substitute.For<CustInfo>();

            _commonUtil.LiveCasinoSettings.Returns(new CoreSetting.Configuration.LiveCasinoSettings { SystemId = 1 });
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<CustInfo>()
                                {
                                      new CustInfo { CustId = 1, UserName = "master1", IsSyncLiveCasino = false },
                                      new CustInfo { CustId = 2, UserName = "master2", IsSyncLiveCasino = false },
                                      new CustInfo { CustId = 3, UserName = "master3", IsSyncLiveCasino = false }
                                });

            // Action
            _gameSettingService.UpdateManualMultiple(settingContext, gameSetting);

            // Assert
            _gameSettingService.Received(4).UpdateManual(settingContext, custInfo, gameSetting);
        }

        /// <summary>
        /// Updates the synchronize live casino_ when called_ update synced status.
        /// </summary>
        [Test]
        public void UpdateSyncLiveCasino_WhenCalled_UpdateSyncedStatus()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();

            _settingRepository.InsertOrUpdate(Arg.Any<UpdateSyncLiveCasinoCriteria>())
                              .Returns(new CommandResult { Code = 0 });

            // Action
            var updateResult = _gameSettingService.UpdateSyncLiveCasino(settingContext, Arg.Any<bool>());

            // Assert
            Assert.IsTrue(updateResult.IsSuccessful);
        }

        /// <summary>
        /// Asserts the customer information.
        /// </summary>
        /// <param name="expectedCustInfo">The expected customer information.</param>
        /// <param name="actualCustInfo">The actual customer information.</param>
        private void AssertCustInfo(CustInfo expectedCustInfo, CustInfo actualCustInfo)
        {
            Assert.AreEqual(expectedCustInfo.CurrencyId, actualCustInfo.CurrencyId);
            Assert.AreEqual(expectedCustInfo.CustId, actualCustInfo.CustId);
            Assert.AreEqual(expectedCustInfo.DisableLiveCasino, actualCustInfo.DisableLiveCasino);
            Assert.AreEqual(expectedCustInfo.Fax, actualCustInfo.Fax);
            Assert.AreEqual(expectedCustInfo.FirstName, actualCustInfo.FirstName);
            Assert.AreEqual(expectedCustInfo.IsSyncLiveCasino, actualCustInfo.IsSyncLiveCasino);
            Assert.AreEqual(expectedCustInfo.LastName, actualCustInfo.LastName);
            Assert.AreEqual(expectedCustInfo.MemberWinLimit, actualCustInfo.MemberWinLimit);
            Assert.AreEqual(expectedCustInfo.Mobile, actualCustInfo.Mobile);
            Assert.AreEqual(expectedCustInfo.NickName, actualCustInfo.NickName);
            Assert.AreEqual(expectedCustInfo.Phone, actualCustInfo.Phone);
            Assert.AreEqual(expectedCustInfo.RoleId, actualCustInfo.RoleId);
            Assert.AreEqual(expectedCustInfo.Site, actualCustInfo.Site);
            Assert.AreEqual(expectedCustInfo.UplineDisableLiveCasino, actualCustInfo.UplineDisableLiveCasino);
            Assert.AreEqual(expectedCustInfo.UserName, actualCustInfo.UserName);
        }

        /// <summary>
        /// Asserts the game setting.
        /// </summary>
        /// <param name="expectedGameSetting">The expected game setting.</param>
        /// <param name="actualGameSetting">The actual game setting.</param>
        private void AssertGameSetting(List<GameType> expectedGameSetting, List<GameType> actualGameSetting)
        {
            Assert.AreEqual(expectedGameSetting.Count, actualGameSetting.Count);

            for (int i = 0; i < expectedGameSetting.Count; i++)
            {
                var expectedGameType = expectedGameSetting[i];
                var actualGameType = actualGameSetting[i];

                Assert.AreEqual(expectedGameType.Id, actualGameType.Id);
                Assert.AreEqual(expectedGameType.BetTypeId, actualGameType.BetTypeId);

                for (int j = 0; j < expectedGameType.BettingLimit.Count; j++)
                {
                    var expectedBettingLimit = expectedGameType.BettingLimit[j];
                    var actualBettingLimit = actualGameType.BettingLimit[j];

                    Assert.AreEqual(expectedBettingLimit.Id, actualBettingLimit.Id);
                    Assert.AreEqual(expectedBettingLimit.Range, actualBettingLimit.Range);
                    Assert.AreEqual(expectedBettingLimit.IsChecked, actualBettingLimit.IsChecked);
                }
            }
        }

        /// <summary>
        /// Clones the game setting.
        /// </summary>
        /// <param name="gameSetting">The game setting.</param>
        /// <returns>Game setting.</returns>
        private List<GameType> CloneGameSetting(List<GameType> gameSetting)
        {
            var list = new List<GameType>();

            foreach (var itemGameType in gameSetting)
            {
                var listBettingLimit = new List<StakeLevel>();
                foreach (var itemBettingLimit in itemGameType.BettingLimit)
                {
                    listBettingLimit.Add(new StakeLevel
                    {
                        Id = itemBettingLimit.Id,
                        IsChecked = itemBettingLimit.IsChecked,
                        Range = itemBettingLimit.Range
                    });
                }

                var gameType = new GameType
                {
                    Id = itemGameType.Id,
                    BetTypeId = itemGameType.BetTypeId,
                    BettingLimit = listBettingLimit
                };

                list.Add(gameType);
            }

            return list;
        }

        private void AssertCommandResult(ICommandResult expectedCommandResult, ICommandResult actualCommandResult)
        {
            Assert.AreEqual(expectedCommandResult.Code, actualCommandResult.Code);
            Assert.AreEqual(expectedCommandResult.Message, actualCommandResult.Message);
        }
    }
}