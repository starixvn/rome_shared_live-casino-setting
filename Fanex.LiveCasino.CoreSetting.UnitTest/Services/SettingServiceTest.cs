namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using NSubstitute;
    using NUnit.Framework;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

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
        /// Gets the current win limit_ invalid customer id_ return new object.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        [TestCase(-1)]
        [TestCase(0)]
        public void GetCurrentWinLimit_InvalidCustId_ReturnNewObject(int custId)
        {
            // Action
            var actualValue = _settingService.GetCurrentWinLimit(custId);

            // Assert
            var expectedValue = new CurrentMemberWin();
            AssertCurrentWinLimit(expectedValue, actualValue);
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

            _settingRepository.Get<CurrentMemberWin>(Arg.Is<GetCustomerWinLimitCriteria>(x => x.IsValid()))
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
                       .Returns(new List<CustInfo>()
                       {
                           new CustInfo { CustId = 1, UserName = "master1" },
                           new CustInfo { CustId = 2, UserName = "master2" },
                           new CustInfo { CustId = 3, UserName = "master3" }
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
        /// Get first customer id when edit direct downline.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_WhenEditDirectDownLine_ReturnFirstCustomerId()
        {
            //// Arrange 
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustRole.Super,
                TargetRoleId = (int)CustRole.Master
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<CustInfo>()
                       {
                           new CustInfo { CustId = 1, UserName = "master1" },
                           new CustInfo { CustId = 2, UserName = "master2" }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(1, representativeCustId);
        }

        /// <summary>
        /// Get first customer id when edit direct downline.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_WhenHasNoCustomer_Return0()
        {
            //// Arrange 
            var settingContext = new SettingContext();
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<CustInfo>());

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(0, representativeCustId);
        }

        /// <summary>
        /// Get reprenset customer id when edit non direct downline customer and first customer edit is synched 
        /// return first synched customer id.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_EditNonDirectDownlineAndFirstMemberSynced_ReturnCustomerIdSyned()
        {
            //// Arrange 
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustRole.Super,
                TargetRoleId = (int)CustRole.Member
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<CustInfo>()
                       {
                           new CustInfo { CustId = 1, IsSyncLiveCasino = false },
                           new CustInfo { CustId = 2, IsSyncLiveCasino = true }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(2, representativeCustId);
        }

        /// <summary>
        /// Get representative customer id when edit non direct downline and no member is synched live casino,
        /// and return 0.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_EditNonDirectDownlineAndNoMemberSynced_Return0()
        {
            //// Arrange 
            var settingContext = new SettingContext
            {
                UplineRoleId = (int)CustRole.Super,
                TargetRoleId = (int)CustRole.Member
            };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<CustInfo>()
                       {
                           new CustInfo { CustId = 1, IsSyncLiveCasino = false },
                           new CustInfo { CustId = 2, IsSyncLiveCasino = false }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(0, representativeCustId);
        }



        #region Postion Taking Test
        /// <summary>
        /// Get first customer id when edit direct downline.
        /// </summary>
        [Test]
        public void GetRepresentativeCustIdForEditingMultiple_EditDirectDownLine_ReturnFirstCustomerId()
        {
            //// Arrange 
            var settingContext = new SettingContext
                {
                    UplineRoleId = (int)CustRole.Super,
                    TargetRoleId = (int)CustRole.Master
                };
            _commonUtil.GetCustomerInfo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                       .Returns(new List<CustInfo>()
                       {
                           new CustInfo { CustId = 1, UserName = "master1" },
                           new CustInfo { CustId = 2, UserName = "master2" }
                       });

            //// Act
            var representativeCustId = _settingService.GetRepresentativeCustIdForEditingMultiple(settingContext);

            //// Assert
            Assert.AreEqual(1, representativeCustId);
        }

        #endregion

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
    }
}