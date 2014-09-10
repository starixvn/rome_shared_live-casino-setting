////namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
////{
////    using System.Collections.Generic;
////    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
////    using Fanex.LiveCasino.CoreSetting.DTO;
////    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
////    using Fanex.LiveCasino.CoreSetting.Enumerations;
////    using Fanex.LiveCasino.CoreSetting.Models;
////    using Fanex.LiveCasino.CoreSetting.Repositories;
////    using Fanex.LiveCasino.CoreSetting.Services;
////    using Fanex.LiveCasino.CoreSetting.Utilities;
////    using Fanex.NAP.Framework.Data;
////    using NSubstitute;
////    using NUnit.Framework;

////    /// <summary>
////    /// TODO: Update summary.
////    /// </summary>
////    [TestFixture]
////    public class PtAndComSettingServiceTest
////    {
////        private IPTComSettingService _positionTakingAndCommissionSettingService;
////        private ISettingRepository _settingRepository;

////        [SetUp]
////        public void SetUp()
////        {
////            _settingRepository = Substitute.For<ISettingRepository>();
////            _commonUtil = Substitute.For<ICommonUtil>();
////            _positionTakingAndCommissionSettingService = Substitute.For<PTComSettingService>(_settingRepository, _commonUtil);
////        }

////        /// <summary>
////        /// Get Position taking and Commission setting when invalid criteria then return default position taking and Commission setting default.
////        /// </summary>
////        /// <param name="uplineRoleId">Up line role ID for getting setting.</param>
////        /// <param name="targetRole">Target role ID for getting setting.</param>
////        [TestCase(0, (int)CustomerRole.Super)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
////        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
////        public void GetPtAndComSetting_whenInvalidCriteria_ReturnDefaultPtAndCommSetting(int uplineRoleId, int targetRole)
////        {
////            var settingContext = new SettingContext
////            {
////                UplineRole = uplineRoleId,
////                TargetRole = targetRole
////            };
////            var positionTakingAndCommissionSettingService = new PTComSettingService(new SettingRepository(), Substitute.For<CommonUtil>());

////            var positionTakingAndCommSetting = positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.IsNull(positionTakingAndCommSetting);
////        }

////        /////// <summary>
////        /////// Get PT And Commission Setting Edit Super Return Super Setting Criteria.
////        /////// </summary>
////        ////[Test]
////        ////public void GetPtAndComSetting_WhenEditSuper_ReturnSuperSetting()
////        ////{
////        ////    //// Arrange
////        ////    var settingContext = new SettingContext
////        ////    {
////        ////        TargetRole = (int)CustomerRole.Super
////        ////    };
////        ////    var superPtAndComSetting = new PtAndComSetting
////        ////    {
////        ////        Commission = new Commission
////        ////        {
////        ////            Value = (float)0.2
////        ////        },
////        ////        PositionTaking = new PositionTaking
////        ////        {
////        ////            S4 = (float)0.2,
////        ////            N4 = (float)0.2
////        ////        }
////        ////    };
////        ////    positionTakingAndCommissionSettingService.SetSuperPtAndCom(Arg.Any<GetSettingForEditSuperCriteria>()).Returns(superPtAndComSetting);
////        ////    //// Act
////        ////    var positionTakingAndCommSetting = _pTComSettingService.SetCustomerPtAndComSetting(settingContext);

////        ////    //// Assert
////        ////    Assert.AreEqual(superPtAndComSetting, positionTakingAndCommSetting);
////        ////}

////        /// <summary>
////        /// Get PT And Commission Setting Edit master Return master Setting Criteria.
////        /// </summary>
////        [Test]
////        public void GetPtAndComSetting_WhenEditMaster_ReturnMasterSetting()
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                TargetRole = (int)CustomerRole.Master
////            };
////            var masterPtAndComSetting = new PTComSetting
////            {
////                Commission = new Commission
////                {
////                    Value = 0.2f
////                },
////                PositionTaking = new PositionTaking
////                {
////                    S3 = (float)0.2
////                }
////            };
////            _settingRepository.GetSettingForEditMaster(Arg.Any<GetSettingForEditMasterCriteria>()).Returns(masterPtAndComSetting);
////            //// Act
////            var positionTakingAndCommSetting = _positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.AreEqual(masterPtAndComSetting, positionTakingAndCommSetting);
////        }

////        /// <summary>
////        /// Get PT And Commission Setting Edit agent Return agent Setting Criteria.
////        /// </summary>
////        [Test]
////        public void GetPtAndComSetting_WhenEditAgent_ReturnAgentSetting()
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                TargetRole = (int)CustomerRole.Agent
////            };
////            var agentPtAndComSetting = new PTComSetting
////            {
////                Commission = new Commission
////                {
////                    Value = (float)0.2
////                },
////                PositionTaking = new PositionTaking
////                {
////                    M2 = (float)0.2
////                }
////            };
////            _settingRepository.GetSettingForEditAgent(Arg.Any<GetSettingForEditAgentCriteria>()).Returns(agentPtAndComSetting);
////            //// Act
////            var positionTakingAndCommSetting = _positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.AreEqual(agentPtAndComSetting, positionTakingAndCommSetting);
////        }

////        /// <summary>
////        /// Get PT And Commission Setting for agent Edit member Return member Setting Criteria.
////        /// </summary>
////        [Test]
////        public void GetPtAndComSetting_WhenAgentEditMember_ReturnMemberSetting()
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                TargetRole = (int)CustomerRole.Member,
////                UplineRole = (int)CustomerRole.Agent
////            };
////            var memberPtAndComSetting = new PTComSetting
////            {
////                Commission = new Commission
////                {
////                    Value = (float)0.2
////                },
////                PositionTaking = new PositionTaking
////                {
////                    A1 = (float)0.1
////                }
////            };
////            _settingRepository.GetSettingForEditMember(Arg.Any<GetSettingForEditMemberCriteria>()).Returns(memberPtAndComSetting);
////            //// Act
////            var positionTakingAndCommSetting = _positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.AreEqual(memberPtAndComSetting, positionTakingAndCommSetting);
////        }

////        /// <summary>
////        /// Get PT And Commission Setting for super Edit member Return member Setting Criteria.
////        /// </summary>
////        [Test]
////        public void GetPtAndComSetting_WhenSuperEditMember_ReturnMemberSetting()
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                TargetRole = (int)CustomerRole.Member,
////                UplineRole = (int)CustomerRole.Super
////            };
////            var memberPtAndComSetting = new PTComSetting
////            {
////                Commission = new Commission
////                {
////                    Value = (float)0.2
////                },
////                PositionTaking = new PositionTaking
////                {
////                    S1 = (float)0.1
////                }
////            };
////            _settingRepository.GetSettingForSuperEditMember(Arg.Any<GetSettingForSuperEditMemberCriteria>())
////                .Returns(memberPtAndComSetting);
////            //// Act
////            var positionTakingAndCommSetting = _positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.AreEqual(memberPtAndComSetting, positionTakingAndCommSetting);
////        }

////        /// <summary>
////        /// Get PT And Commission Setting for master Edit member Return member Setting Criteria.
////        /// </summary>
////        [Test]
////        public void GetPtAndComSetting_WhenMasterEditMember_ReturnMemberSetting()
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                TargetRole = (int)CustomerRole.Member,
////                UplineRole = (int)CustomerRole.Master
////            };
////            var memberPtAndComSetting = new PTComSetting
////            {
////                Commission = new Commission
////                {
////                    Value = (float)0.2
////                },
////                PositionTaking = new PositionTaking
////                {
////                    M1 = (float)0.1
////                }
////            };
////            _settingRepository.GetSettingForMasterEditMember(Arg.Any<GetSettingForMasterEditMemberCriteria>())
////                .Returns(memberPtAndComSetting);
////            //// Act
////            var positionTakingAndCommSetting = _positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.AreEqual(memberPtAndComSetting, positionTakingAndCommSetting);
////        }

////        /// <summary>
////        /// Get PT And Commission Setting for agent Edit member Return member Setting Criteria.
////        /// </summary>
////        [Test]
////        public void GetPtAndComSetting_WhenEditUnknownCustomer_ReturnNull()
////        {
////            //// Arrange
////            var settingContext = new SettingContext();
////            var memberPtAndComSetting = new PTComSetting();

////            //// Act
////            var positionTakingAndCommSetting = _positionTakingAndCommissionSettingService.SetCustomerPtAndComSetting(settingContext);

////            //// Assert
////            Assert.IsNull(positionTakingAndCommSetting);
////        }

////        /// <summary>
////        /// Update customer position taking and Commission setting return command results.
////        /// </summary>
////        /// <param name="uplineRoleId">Up line role id for testing.</param>
////        /// <param name="targetRole">Target role id for testing.</param>
////        [TestCase(0, (int)CustomerRole.Super)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
////        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
////        public void UpdatePtAndComSetting_UpdateCustomerSetting_CommandResults(int uplineRoleId, int targetRole)
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                UplineRole = uplineRoleId,
////                TargetRole = targetRole
////            };
////            var custPtAndComSetting = new PTComSetting
////            {
////                Commission = new Commission
////                {
////                    Value = 0.1f,
////                    Upline = new Commission
////                    {
////                        Value = 0.1f
////                    }
////                },
////                PositionTaking = GetDummyPositionTaking()
////            };
////            var updateResult = new List<ICommandResult>
////            {
////                new PtAndComCommandResult
////                {
////                    Code = 1,
////                    Message = string.Empty
////                }
////            };
////            var updatedResult = new CommandResultDTO
////            {
////                Code = "1",
////                Message = string.Empty
////            };
////            _settingRepository.InsertOrUpdate(Arg.Any<ICriteria>()).Returns(updatedResult);

////            //// Act
////            var mainResult = _positionTakingAndCommissionSettingService.UpdatePtComSetting(settingContext, custPtAndComSetting);

////            //// Assert
////            Assert.AreEqual(updateResult[0].Code, mainResult.Results[0].Code);
////        }

////        /// <summary>
////        /// Update customer position taking and Commission setting return command results.
////        /// </summary>
////        /// <param name="uplineRoleId">Up line role id for testing.</param>
////        /// <param name="targetRole">Target role id for testing.</param>
////        [TestCase(0, (int)CustomerRole.Super)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
////        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
////        public void UpdatePtAndComSetting_UpdateCustomerSetting_CommandResultsWithMessage(int uplineRoleId, int targetRole)
////        {
////            //// Arrange
////            var settingContext = new SettingContext
////            {
////                UplineRole = uplineRoleId,
////                TargetRole = targetRole
////            };
////            var custPtAndComSetting = new PTComSetting
////            {
////                PositionTaking = GetDummyPositionTaking()
////            };
////            var updateResult = new List<ICommandResult>
////            {
////                new PtAndComCommandResult
////                {
////                    Code = -101,
////                    Message = "Commission: Updated successfully."
////                }
////            };
////            var updatedResult = new CommandResultDTO
////            {
////                Code = "-101",
////                Message = string.Empty
////            };
////            _settingRepository.InsertOrUpdate(Arg.Any<ICriteria>()).Returns(updatedResult);

////            //// Act
////            var mainResult = _positionTakingAndCommissionSettingService.UpdatePtComSetting(settingContext, custPtAndComSetting);

////            //// Assert
////            Assert.AreEqual(updateResult[0].Code, mainResult.Results[0].Code);
////            Assert.AreEqual(updateResult[0].Message, mainResult.Results[0].Message);
////        }

////        /// <summary>
////        /// Update multiple customer position taking and Commission setting, return multiple command result.
////        /// </summary>
////        /// <param name="uplineRoleId">Up line role id for updating.</param>
////        /// <param name="targetRole">Target role id for updating.</param>
////        [TestCase(0, (int)CustomerRole.Super)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Master)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Agent)]
////        [TestCase((int)CustomerRole.Agent, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Super, (int)CustomerRole.Member)]
////        [TestCase((int)CustomerRole.Master, (int)CustomerRole.Member)]
////        public void UpdatePtAndComSettingMultiple_SettingContextAndPtComSetting_MultipleCommandResults(int uplineRoleId, int targetRole)
////        {
////            //// Arrange
////            var custInfoList = new List<Customer>()
////                       {
////                           new Customer { CustId = 1, UserName = "name 1" },
////                           new Customer { CustId = 2, UserName = "name 2" }
////                       };
          
////            var settingContext = new SettingContext
////            {
////                UplineRole = uplineRoleId,
////                TargetRole = targetRole
////            };
////            var custPtAndComSetting = new PTComSetting
////            {
////                PositionTaking = GetDummyPositionTaking()
////            };
////            var multipleResults = GetDummyMultipleCommand(custInfoList);

////            _settingRepository.InsertOrUpdate(Arg.Any<ICriteria>()).Returns(new CommandResultDTO
////            {
////                Code = "1",
////                Message = string.Empty
////            });

////            //// Act
////            var mainResult = _positionTakingAndCommissionSettingService.UpdatePtComSettingMultiple(settingContext, custPtAndComSetting);

////            //// Assert
////            Assert.AreEqual(mainResult.CommandResultsList.Count, multipleResults.CommandResultsList.Count);
////            Assert.AreEqual(mainResult.CommandResultsList[0].Id, multipleResults.CommandResultsList[0].Id);
////            Assert.AreEqual(mainResult.CommandResultsList[0].Name, multipleResults.CommandResultsList[0].Name);
////        }

////        /// <summary>
////        /// Get dummy position taking for testing.
////        /// </summary>
////        /// <returns>Return position taking object for testing.</returns>
////        private PositionTaking GetDummyPositionTaking()
////        {
////            return new PositionTaking
////            {
////                M4 = (float)0.1,
////                A1 = (float)0.1,
////                A2 = (float)0.1,
////                A4 = (float)0.1,
////                L2 = (float)0.1,
////                L3 = (float)0.1,
////                L4 = (float)0.1,
////                M1 = (float)0.1,
////                M1Max = (float)0.1,
////                M1Min = (float)0.1,
////                M2 = (float)0.1,
////                M3 = (float)0.1,
////                N1 = (float)0.1,
////                N2 = (float)0.1,
////                N3 = (float)0.1,
////                N4 = (float)0.1,
////                NewPresetOnMember = (float)0.1,
////                S1 = (float)0.1,
////                S1Max = (float)0.1,
////                S1Min = (float)0.1,
////                S3 = (float)0.1,
////                S4 = (float)0.1,
////                Um1 = (float)0.1,
////                Um2 = (float)0.1,
////                Us1 = (float)0.1,
////                Us3 = (float)0.1,
////                UpdateAllMembers = true,
////                IsDisableAuto = true,
////                Is3 = true,
////                Is1 = true,
////                Im2 = true,
////                Im1 = true
////            };
////        }

////        /// <summary>
////        /// Get dummy multiple result for testing.
////        /// </summary>
////        /// <param name="custInfoList">Customer information list for get results.</param>
////        /// <returns>Multiple command results from multiple customer.</returns>
////        private IMultipleCommandResults GetDummyMultipleCommand(List<Customer> custInfoList)
////        {
////            return new MultipleCommandResults
////            {
////                CommandResultsList = new List<ICommandResults>
////                {
////                    new CommandResults
////                    {
////                        Id = custInfoList[0].CustId,
////                        Name = custInfoList[0].UserName,
////                        Results = new List<ICommandResult>
////                        {
////                            new CommandResultDTO
////                            {
////                                Code = "1",
////                                Message = string.Empty
////                            }
////                        }
////                    },
////                    new CommandResults
////                    {
////                        Id = custInfoList[1].CustId,
////                        Name = custInfoList[1].UserName,
////                        Results = new List<ICommandResult>
////                        {
////                            new CommandResultDTO
////                            {
////                                Code = "1",
////                                Message = string.Empty
////                            }
////                        }
////                    }
////                }
////            };
////        }
////    }
////}