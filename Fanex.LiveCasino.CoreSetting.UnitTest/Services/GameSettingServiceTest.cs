namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
    using Fanex.NAP.Framework.Command;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing GameSettingService class.
    /// </summary>
    [TestFixture]
    public class GameSettingServiceTest
    {
        private ISettingRepository _settingRepository;
        private ICommonUtil _commonUtil;
        private ILogService _logService;
        private IGameSettingService _gameSettingService;

        [SetUp]
        public void SetUp()
        {
            _settingRepository = Substitute.For<ISettingRepository>();
            _logService = Substitute.For<ILogService>();
            _commonUtil = Substitute.For<ICommonUtil>();
            _gameSettingService = Substitute.For<GameSettingService>(_settingRepository, _logService, _commonUtil);
        }

        [TestCase("")]
        [TestCase(null)]
        public void GetCustomerSiteByUsername_InvalidUsername_ReturnEmpty(string username)
        {
            // Action
            string actualValue = _gameSettingService.GetCustomerSiteByUsername(username);

            // Assert
            Assert.IsEmpty(actualValue);
        }

        /// <summary>
        /// Gets the name of the customer site by username_ valid username_ return site.
        /// </summary>
        [Test]
        public void GetCustomerSiteByUsername_ValidUsername_ReturnSiteName()
        {
            // Arrange
            const string SITE = "BEST_ODDS";
            var username = "supertest1";
            _settingRepository.Get<CustInfo>(Arg.Is<GetCustomerSiteByUsernameCriteria>(x => !string.IsNullOrWhiteSpace(x.Username)))
                              .Returns(new CustInfo { Site = SITE });

            // Action
            var actualValue = _gameSettingService.GetCustomerSiteByUsername(username);

            // Assert
            Assert.AreEqual(SITE, actualValue);
        }

        /// <summary>
        /// Gets the customer info_ invalid customer id_ return empty customer information.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        [TestCase(-1)]
        [TestCase(0)]
        public void GetCustomerInfo_InvalidCustId_ReturnEmptyCustInfo(int custId)
        {
            // Arrange
            _settingRepository.GetCustomerInfo(Arg.Is<GetCustomerInfoCriteria>(x => !x.IsValid())).Returns(new CustInfo());

            // Action
            var actualValue = _gameSettingService.GetCustomerInfo(custId);

            // Assert
            var expectedCustInfo = new CustInfo();
            AssertCustInfo(expectedCustInfo, actualValue);
        }

        /// <summary>
        /// Gets the customer info_ valid customer id_ return customer information.
        /// </summary>
        [Test]
        public void GetCustomerInfo_ValidCustId_ReturnCustInfo()
        {
            // Arrange
            var custId = 1;
            var expectedCustInfo = new CustInfo
            {
                CurrencyId = 20,
                CustId = 1,
                UserName = "supertest1"
            };
            _settingRepository.GetCustomerInfo(Arg.Is<GetCustomerInfoCriteria>(x => x.IsValid())).Returns(expectedCustInfo);

            // Action
            var actualValue = _gameSettingService.GetCustomerInfo(custId);

            // Assert
            AssertCustInfo(expectedCustInfo, actualValue);
        }

        /// <summary>
        /// Gets the game setting by customer id_ invalid customer id_ return empty game setting.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        [TestCase(-1)]
        [TestCase(0)]
        public void GetGameSettingByCustomerId_InvalidCustId_ReturnEmptyGameSetting(int custId)
        {
            // Arrange
            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => !x.IsValid()), true).Returns(new List<GameType>());

            // Action
            var actualValue = _gameSettingService.GetGameSettingByCustomerId(custId);

            // Assert
            Assert.IsFalse(actualValue.Any());
        }

        /// <summary>
        /// Gets the game setting by customer id_ valid customer id_ return game setting.
        /// </summary>
        [Test]
        public void GetGameSettingByCustomerId_ValidCustId_ReturnGameSetting()
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

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid()), true).Returns(expectedGameSetting);

            // Action
            var actualValue = _gameSettingService.GetGameSettingByCustomerId(custId);

            // Assert
            AssertGameSetting(expectedGameSetting, actualValue);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetGameSettingByExchange_InvalidExchangeId_ReturnEmptyGameSetting(int exchangeId)
        {
            // Arrange
            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByExchangeCriteria>(x => !x.IsValid()), false).Returns(new List<GameType>());

            // Action
            var actualValue = _gameSettingService.GetGameSettingByExchange(exchangeId, Arg.Any<string>());

            // Assert
            Assert.IsFalse(actualValue.Any());
        }

        /// <summary>
        /// Gets the game setting by exchange_ valid exchange id_ return empty game setting.
        /// </summary>
        [Test]
        public void GetGameSettingByExchange_ValidExchangeId_ReturnEmptyGameSetting()
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
            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByExchangeCriteria>(x => x.IsValid()), false).Returns(expectedGameSetting);

            // Action
            var actualValue = _gameSettingService.GetGameSettingByExchange(exchangeId, Arg.Any<string>());

            // Assert
            AssertGameSetting(expectedGameSetting, actualValue);
        }

        /// <summary>
        /// Gets the game setting_ when super is not synced_ return default game setting.
        /// </summary>
        [Test]
        public void GetGameSetting_WhenSuperIsNotSynced_ReturnDefaultGameSetting()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnValue = new List<GameType>();

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

            returnValue = CloneGameSetting(expectedGameSetting);

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByExchangeCriteria>(x => x.IsValid()), false).Returns(returnValue);
            _settingRepository.Get<CustInfo>(Arg.Is<GetCustomerSiteByUsernameCriteria>(x => !string.IsNullOrWhiteSpace(x.Username)))
                              .Returns(new CustInfo { Site = custInfo.Site });

            // Action
            var actualvalue = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualvalue);
        }

        /// <summary>
        /// Gets the game setting_ customer is not synced_ return direct up line game setting.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        public void GetGameSetting_CustomerIsNotSynced_ReturnDirectUplineGameSetting(int targetRoleId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnValue = new List<GameType>();

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

            returnValue = CloneGameSetting(expectedGameSetting);

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), true).Returns(returnValue);

            // Action
            var actualvalue = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualvalue);
        }

        /// <summary>
        /// Gets the type of the game setting_ customer is not synced_ return direct up line game setting and remove unchecked bet.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        public void GetGameSetting_CustomerIsNotSynced_ReturnDirectUplineGameSettingAndRemoveUncheckedBetType(int targetRoleId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnValue = new List<GameType>();

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

            returnValue = CloneGameSetting(expectedGameSetting);
            returnValue.Add(new GameType
            {
                Id = 1,
                BetTypeId = 1101,
                BettingLimit = new List<StakeLevel>
                {
                    new StakeLevel { Id = 1, Range = "5-1000", IsChecked = false },
                    new StakeLevel { Id = 2, Range = "10-1000", IsChecked = false }
                }
            });

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), true).Returns(returnValue);

            // Action
            var actualvalue = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualvalue);
        }

        /// <summary>
        /// Gets the game setting_ member is not synced_ return direct up line game setting and lowest value.
        /// </summary>
        [Test]
        public void GetGameSetting_MemberIsNotSynced_ReturnDirectUplineGameSettingAndLowestValue()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnValue = new List<GameType>();

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

            returnValue = CloneGameSetting(expectedGameSetting);
            returnValue[0].BettingLimit.Insert(0, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = false });
            returnValue[0].BettingLimit.Insert(1, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = false });
            returnValue[0].BettingLimit.Insert(3, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = true });
            returnValue[0].BettingLimit.Insert(4, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = true });
            returnValue[0].BettingLimit.Insert(5, new StakeLevel { Id = 3, Range = "5-1000", IsChecked = true });

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), true).Returns(returnValue);

            // Action
            var actualvalue = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualvalue);
        }

        /// <summary>
        /// Gets the game setting_ customer is synced_ return game setting.
        /// </summary>
        /// <param name="targetRoleId">The target role identifier.</param>
        [TestCase(CustRole.Super)]
        [TestCase(CustRole.Master)]
        [TestCase(CustRole.Agent)]
        [TestCase(CustRole.Member)]
        public void GetGameSetting_CustomerIsSynced_ReturnGameSetting(int targetRoleId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var custInfo = Substitute.For<CustInfo>();
            var expectedGameSetting = new List<GameType>();
            var returnValue = new List<GameType>();

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

            returnValue = CloneGameSetting(expectedGameSetting);

            _settingRepository.GetGameSetting(Arg.Is<GetGameSettingByCustomerIdCriteria>(x => x.IsValid() && x.CustId == 1), true).Returns(returnValue);

            // Action
            var actualvalue = _gameSettingService.GetGameSetting(settingContext, custInfo);

            // Assert
            AssertGameSetting(expectedGameSetting, actualvalue);
        }

        /// <summary>
        /// Inserts the automatic account_ super_ return.
        /// </summary>
        [Test]
        public void InsertAutoAccount_Super_ReturnNull()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            settingContext.TargetRoleId = (int)CustRole.Super;

            // Action
            var actualCommandResult = _gameSettingService.InsertAutoAccount(settingContext);

            // Assert
            Assert.IsNull(actualCommandResult);
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
    }
}