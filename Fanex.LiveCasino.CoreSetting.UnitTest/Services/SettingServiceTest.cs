namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
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
    public class SettingServiceTest
    {
        private ICommonUtil _commonUtil;
        private ISettingService _settingService;
        private ISettingRepository _settingRepository;

        [SetUp]
        public void SetUp()
        {
            _settingRepository = Substitute.For<ISettingRepository>();
            _commonUtil = Substitute.For<ICommonUtil>();
            _settingService = Substitute.For<SettingService>(_settingRepository, _commonUtil);
        }

        /// <summary>
        /// Gets the current win limit_ valid customer id_ return current member win.
        /// </summary>
        [Test]
        public void GetCurrentWinLimit_ValidCustId_ReturnCurrentMemberWin()
        {
            // Arrange
            var custId = 1;
            var expectedCurrentMemberWin = new CurrentMemberWin
            {
                Balance = 1,
                MaxWinCredit = 1,
                MaxWinLiveCasino = 1,
                WinPercent = 1
            };

            _settingRepository.GetCurrentMemberWin(Arg.Is<GetCustomerWinLimitCriteria>(x => x.IsValid()))
                              .Returns(CloneCurrentMemberWin(expectedCurrentMemberWin));

            // Action
            var actualValue = _settingService.GetCurrentWinLimit(custId);

            // Assert
            AssertCurrentWinLimit(expectedCurrentMemberWin, actualValue);
        }

        /// <summary>
        /// Updates the win limit_ invalid customer id_ return error command.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        [TestCase(-1)]
        [TestCase(0)]
        public void UpdateWinLimit_UpdateFail_ReturnErrorCommand(int custId)
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var memberWinLimit = Substitute.For<MemberWinLimit>();

            settingContext.TargetCustId = custId;
            _settingRepository.InsertOrUpdate(Arg.Is<UpdateCustomerWinLimitCriteria>(x => !x.IsValid()))
                              .Returns(new CommandResult { Code = ErrCode.InvalidCriteria });

            // Action
            var actualCommand = _settingService.UpdateWinLimit(settingContext, memberWinLimit);

            // Assert
            Assert.IsFalse(actualCommand.IsSuccessful);
        }

        /// <summary>
        /// Updates the win limit_ update success_ return success message.
        /// </summary>
        [Test]
        public void UpdateWinLimit_UpdateSuccess_ReturnSuccessMessage()
        {
            // Arrange
            var custId = 1;
            var settingContext = Substitute.For<SettingContext>();
            var memberWinLimit = Substitute.For<MemberWinLimit>();

            settingContext.TargetCustId = custId;
            _settingRepository.InsertOrUpdate(Arg.Is<UpdateCustomerWinLimitCriteria>(x => x.IsValid()))
                              .Returns(new CommandResult { Code = 0 });

            // Action
            var actualCommand = _settingService.UpdateWinLimit(settingContext, memberWinLimit);

            // Assert
            Assert.AreEqual(LiveCasinoLang.UpdatedWinLimitSuccessfully, actualCommand.Results.FirstOrDefault().Message);
        }

        /// <summary>
        /// Updates the win limit multiple_ valid customer info_ call update single.
        /// </summary>
        [Test]
        public void UpdateWinLimitMultiple_ValidCustInfo_CallUpdateSingle()
        {
            // Arrange
            var settingContext = Substitute.For<SettingContext>();
            var memberWinLimit = Substitute.For<MemberWinLimit>();
            memberWinLimit.Current = 0;
            memberWinLimit.IsAutoReset = true;
            memberWinLimit.Max = 0;
            memberWinLimit.Upline = new MemberWinLimit();

            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<Customer>()
                       {
                           new Customer { CustId = 1, UserName = "master1" },
                           new Customer { CustId = 2, UserName = "master2" },
                           new Customer { CustId = 3, UserName = "master3" }
                       });
            _settingRepository.InsertOrUpdate(Arg.Is<UpdateCustomerWinLimitCriteria>(x => x.IsValid()))
                             .Returns(new CommandResult { Code = 0 });

            // Action
            var updateCommand = _settingService.UpdateWinLimitMultiple(settingContext, memberWinLimit);

            // Assert
            Assert.AreEqual(3, updateCommand.CommandResultsList.Count);
            Assert.AreEqual(1, updateCommand.CommandResultsList[0].Id);
            Assert.AreEqual(2, updateCommand.CommandResultsList[1].Id);
            Assert.AreEqual(3, updateCommand.CommandResultsList[2].Id);
        }

        /// <summary>
        /// Get first customer id when edit direct down line.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_WhenEditDirectDownLine_ReturnFirstCustomerId()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustomerRole.Super,
                TargetRoleId = (int)CustomerRole.Master
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<Customer>()
                       {
                           new Customer { CustId = 1, UserName = "master1" },
                           new Customer { CustId = 2, UserName = "master2" }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(1, representativeCustId);
        }

        /// <summary>
        /// Get first customer id when edit direct down line.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_WhenHasNoCustomer_Return0()
        {
            //// Arrange
            var settingContext = new SettingContext();
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<Customer>());

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(0, representativeCustId);
        }

        /// <summary>
        /// Get representative customer id when edit non direct down line customer and first customer edit is synched
        /// return first synched customer id.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_EditNonDirectDownlineAndFirstMemberSynced_ReturnCustomerIdSyned()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustomerRole.Super,
                TargetRoleId = (int)CustomerRole.Member
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<Customer>()
                       {
                           new Customer { CustId = 1, IsSyncLiveCasino = false },
                           new Customer { CustId = 2, IsSyncLiveCasino = true }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(2, representativeCustId);
        }

        /// <summary>
        /// Get representative customer id when edit non direct down line and no member is synched live casino,
        /// and return 0.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_EditNonDirectDownlineAndNoMemberSynced_Return0()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustomerRole.Super,
                TargetRoleId = (int)CustomerRole.Member
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<Customer>()
                       {
                           new Customer { CustId = 1, IsSyncLiveCasino = false },
                           new Customer { CustId = 2, IsSyncLiveCasino = false }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(0, representativeCustId);
        }

        /// <summary>
        /// Get first customer id when edit direct down line.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_EditDirectDownLine_ReturnFirstCustomerId()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustomerRole.Super,
                TargetRoleId = (int)CustomerRole.Master
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<Customer>()
                       {
                           new Customer { CustId = 1, UserName = "master1" },
                           new Customer { CustId = 2, UserName = "master2" }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(1, representativeCustId);
        }

        #region Postion Taking Test

        #region Get Customer Commission and Position Taking setting

        /// <summary>
        /// Get Position taking and commission setting when invalid criteria then return default position taking and commission setting default.
        /// </summary>
        /// <param name="uplineRoleId">Up line role ID for getting setting.</param>
        /// <param name="targetRoleId">Target role ID for getting setting.</param>
        [TestCase(0, (int)CustomerRole.Super)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
        public void GetPtAndComSetting_whenInvalidCriteria_ReturnDefaultPtAndCommSetting(int uplineRoleId, int targetRoleId)
        {
            var settingContext = new SettingContext
            {
                UplineRoleId = uplineRoleId,
                TargetRoleId = targetRoleId
            };
            var settingService = new SettingService(new SettingRepository(), Substitute.For<CommonUtil>());

            var positionTakingAndCommSetting = settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.IsNull(positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting Edit Super Return Super Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenEditSuper_ReturnSuperSetting()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                TargetRoleId = (int)CustomerRole.Super
            };
            var superPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = (float)0.2
                },
                PositionTaking = new PositionTaking
                {
                    S4 = (float)0.2,
                    N4 = (float)0.2
                }
            };
            _settingRepository.GetSettingForEditSuper(Arg.Any<GetSettingForEditSuperCriteria>()).Returns(superPtAndComSetting);
            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.AreEqual(superPtAndComSetting, positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting Edit master Return master Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenEditMaster_ReturnMasterSetting()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                TargetRoleId = (int)CustomerRole.Master
            };
            var masterPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = 0.2f
                },
                PositionTaking = new PositionTaking
                {
                    S3 = (float)0.2
                }
            };
            _settingRepository.GetSettingForEditMaster(Arg.Any<GetSettingForEditMasterCriteria>()).Returns(masterPtAndComSetting);
            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.AreEqual(masterPtAndComSetting, positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting Edit agent Return agent Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenEditAgent_ReturnAgentSetting()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                TargetRoleId = (int)CustomerRole.Agent
            };
            var agentPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = (float)0.2
                },
                PositionTaking = new PositionTaking
                {
                    M2 = (float)0.2
                }
            };
            _settingRepository.GetSettingForEditAgent(Arg.Any<GetSettingForEditAgentCriteria>()).Returns(agentPtAndComSetting);
            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.AreEqual(agentPtAndComSetting, positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting for agent Edit member Return member Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenAgentEditMember_ReturnMemberSetting()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                TargetRoleId = (int)CustomerRole.Member,
                UplineRoleId = (int)CustomerRole.Agent
            };
            var memberPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = (float)0.2
                },
                PositionTaking = new PositionTaking
                {
                    A1 = (float)0.1
                }
            };
            _settingRepository.GetSettingForEditMember(Arg.Any<GetSettingForEditMemberCriteria>()).Returns(memberPtAndComSetting);
            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.AreEqual(memberPtAndComSetting, positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting for super Edit member Return member Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenSuperEditMember_ReturnMemberSetting()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                TargetRoleId = (int)CustomerRole.Member,
                UplineRoleId = (int)CustomerRole.Super
            };
            var memberPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = (float)0.2
                },
                PositionTaking = new PositionTaking
                {
                    S1 = (float)0.1
                }
            };
            _settingRepository.GetSettingForSuperEditMember(Arg.Any<GetSettingForSuperEditMemberCriteria>())
                .Returns(memberPtAndComSetting);
            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.AreEqual(memberPtAndComSetting, positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting for master Edit member Return member Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenMasterEditMember_ReturnMemberSetting()
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                TargetRoleId = (int)CustomerRole.Member,
                UplineRoleId = (int)CustomerRole.Master
            };
            var memberPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = (float)0.2
                },
                PositionTaking = new PositionTaking
                {
                    M1 = (float)0.1
                }
            };
            _settingRepository.GetSettingForMasterEditMember(Arg.Any<GetSettingForMasterEditMemberCriteria>())
                .Returns(memberPtAndComSetting);
            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.AreEqual(memberPtAndComSetting, positionTakingAndCommSetting);
        }

        /// <summary>
        /// Get PT And Commission Setting for agent Edit member Return member Setting Criteria.
        /// </summary>
        [Test]
        public void GetPtAndComSetting_WhenEditUnknownCustomer_ReturnNull()
        {
            //// Arrange
            var settingContext = new SettingContext();
            var memberPtAndComSetting = new PtAndComSetting();

            //// Act
            var positionTakingAndCommSetting = _settingService.GetPtAndComSetting(settingContext);

            //// Assert
            Assert.IsNull(positionTakingAndCommSetting);
        }

        #endregion Get Customer Commission and Position Taking setting

        #region Update cusomter position taking and commission setting

        /// <summary>
        /// Update customer position taking and commission setting return command results.
        /// </summary>
        /// <param name="uplineRoleId">Up line role id for testing.</param>
        /// <param name="targetRoleId">Target role id for testing.</param>
        [TestCase(0, (int)CustomerRole.Super)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
        public void UpdatePtAndComSetting_UpdateCustomerSetting_CommandResults(int uplineRoleId, int targetRoleId)
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                UplineRoleId = uplineRoleId,
                TargetRoleId = targetRoleId
            };
            var custPtAndComSetting = new PtAndComSetting
            {
                Commission = new Commission
                {
                    Value = 0.1f,
                    Upline = new Commission
                    {
                        Value = 0.1f
                    }
                },
                PositionTaking = GetDummyPositionTaking()
            };
            var updateResult = new List<ICommandResult>
            {
                new PtAndComCommandResult
                {
                    Code = 1,
                    Message = string.Empty
                }
            };
            var updatedResult = new CommandResult
            {
                Code = "1",
                Message = string.Empty
            };
            _settingRepository.InsertOrUpdate(Arg.Any<ICriteria>()).Returns(updatedResult);

            //// Act
            var mainResult = _settingService.UpdatePtAndComSetting(settingContext, custPtAndComSetting);

            //// Assert
            Assert.AreEqual(updateResult[0].Code, mainResult.Results[0].Code);
        }

        /// <summary>
        /// Update customer position taking and commission setting return command results.
        /// </summary>
        /// <param name="uplineRoleId">Up line role id for testing.</param>
        /// <param name="targetRoleId">Target role id for testing.</param>
        [TestCase(0, (int)CustomerRole.Super)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
        public void UpdatePtAndComSetting_UpdateCustomerSetting_CommandResultsWithMessage(int uplineRoleId, int targetRoleId)
        {
            //// Arrange
            var settingContext = new SettingContext
            {
                UplineRoleId = uplineRoleId,
                TargetRoleId = targetRoleId
            };
            var custPtAndComSetting = new PtAndComSetting
            {
                PositionTaking = GetDummyPositionTaking()
            };
            var updateResult = new List<ICommandResult>
            {
                new PtAndComCommandResult
                {
                    Code = -101,
                    Message = "Commission: Updated successfully."
                }
            };
            var updatedResult = new CommandResult
            {
                Code = "-101",
                Message = string.Empty
            };
            _settingRepository.InsertOrUpdate(Arg.Any<ICriteria>()).Returns(updatedResult);

            //// Act
            var mainResult = _settingService.UpdatePtAndComSetting(settingContext, custPtAndComSetting);

            //// Assert
            Assert.AreEqual(updateResult[0].Code, mainResult.Results[0].Code);
            Assert.AreEqual(updateResult[0].Message, mainResult.Results[0].Message);
        }

        /// <summary>
        /// Update multiple customer position taking and commission setting, return multiple command result.
        /// </summary>
        /// <param name="uplineRoleId">Up line role id for updating.</param>
        /// <param name="targetRoleId">Target role id for updating.</param>
        [TestCase(0, (int)CustomerRole.Super)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
        public void UpdatePtAndComSettingMultiple_SettingContextAndPtComSetting_MultipleCommandResults(int uplineRoleId, int targetRoleId)
        {
            //// Arrange
            var custInfoList = new List<Customer>()
                       {
                           new Customer { CustId = 1, UserName = "name 1" },
                           new Customer { CustId = 2, UserName = "name 2" }
                       };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(custInfoList);
            var settingContext = new SettingContext
            {
                UplineRoleId = uplineRoleId,
                TargetRoleId = targetRoleId
            };
            var custPtAndComSetting = new PtAndComSetting
            {
                PositionTaking = GetDummyPositionTaking()
            };
            var multipleResults = GetDummyMultipleCommand(custInfoList);

            _settingRepository.InsertOrUpdate(Arg.Any<ICriteria>()).Returns(new CommandResult
            {
                Code = "1",
                Message = string.Empty
            });

            //// Act
            var mainResult = _settingService.UpdatePtAndComSettingMultiple(settingContext, custPtAndComSetting);

            //// Assert
            Assert.AreEqual(mainResult.CommandResultsList.Count, multipleResults.CommandResultsList.Count);
            Assert.AreEqual(mainResult.CommandResultsList[0].Id, multipleResults.CommandResultsList[0].Id);
            Assert.AreEqual(mainResult.CommandResultsList[0].Name, multipleResults.CommandResultsList[0].Name);
        }

        #endregion Update cusomter position taking and commission setting

        #endregion Postion Taking Test

        private void AssertCurrentWinLimit(CurrentMemberWin expectedValue, CurrentMemberWin actualValue)
        {
            Assert.AreEqual(expectedValue.Balance, actualValue.Balance);
            Assert.AreEqual(expectedValue.MaxWinCredit, actualValue.MaxWinCredit);
            Assert.AreEqual(expectedValue.WinPercent, actualValue.WinPercent);
            Assert.AreEqual(expectedValue.MaxWinLiveCasino, actualValue.MaxWinLiveCasino);
        }

        private CurrentMemberWin CloneCurrentMemberWin(CurrentMemberWin currentMemberWin)
        {
            var result = new CurrentMemberWin();

            result.Balance = currentMemberWin.Balance;
            result.MaxWinCredit = currentMemberWin.MaxWinCredit;
            result.MaxWinLiveCasino = currentMemberWin.MaxWinLiveCasino;
            result.WinPercent = currentMemberWin.WinPercent;

            return result;
        }

        /// <summary>
        /// Get dummy position taking for testing.
        /// </summary>
        /// <returns>Return position taking object for testing.</returns>
        private PositionTaking GetDummyPositionTaking()
        {
            return new PositionTaking
            {
                M4 = (float)0.1,
                A1 = (float)0.1,
                A2 = (float)0.1,
                A4 = (float)0.1,
                L2 = (float)0.1,
                L3 = (float)0.1,
                L4 = (float)0.1,
                M1 = (float)0.1,
                M1Max = (float)0.1,
                M1Min = (float)0.1,
                M2 = (float)0.1,
                M3 = (float)0.1,
                N1 = (float)0.1,
                N2 = (float)0.1,
                N3 = (float)0.1,
                N4 = (float)0.1,
                NewPresetOnMember = (float)0.1,
                S1 = (float)0.1,
                S1Max = (float)0.1,
                S1Min = (float)0.1,
                S3 = (float)0.1,
                S4 = (float)0.1,
                Um1 = (float)0.1,
                Um2 = (float)0.1,
                Us1 = (float)0.1,
                Us3 = (float)0.1,
                UpdateAllMembers = true,
                IsDisableAuto = true,
                Is3 = true,
                Is1 = true,
                Im2 = true,
                Im1 = true
            };
        }

        /// <summary>
        /// Get dummy multiple result for testing.
        /// </summary>
        /// <param name="custInfoList">Customer information list for get results.</param>
        /// <returns>Multiple command results from multiple customer.</returns>
        private IMultipleCommandResults GetDummyMultipleCommand(List<Customer> custInfoList)
        {
            return new MultipleCommandResults
            {
                CommandResultsList = new List<ICommandResults>
                {
                    new CommandResults
                    {
                        Id = custInfoList[0].CustId,
                        Name = custInfoList[0].UserName,
                        Results = new List<ICommandResult>
                        {
                            new CommandResult
                            {
                                Code = "1",
                                Message = string.Empty
                            }
                        }
                    },
                    new CommandResults
                    {
                        Id = custInfoList[1].CustId,
                        Name = custInfoList[1].UserName,
                        Results = new List<ICommandResult>
                        {
                            new CommandResult
                            {
                                Code = "1",
                                Message = string.Empty
                            }
                        }
                    }
                }
            };
        }
    }
}