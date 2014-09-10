namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
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