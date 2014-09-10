namespace Fanex.LiveCasino.CoreSetting.UnitTest.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLimitSettingServiceTest
    {
        private IWinLimitSettingService _winLimitSettingService;
        private ISettingRepository _settingRepository;

        [SetUp]
        public void SetUp()
        {
            _settingRepository = Substitute.For<ISettingRepository>();
            _winLimitSettingService = Substitute.For<WinLimitSettingService>(_settingRepository);
        }

        /// <summary>
        /// Gets the current win limit_ valid customer id_ return current member win.
        /// </summary>
        [Test]
        public void GetCurrentWinLimit_ValidCustId_ReturnCurrentMemberWin()
        {
            // Arrange
            var custId = 1;
            var expectedCurrentMemberWin = new CurrentMemberWinDTO
            {
                Balance = 1,
                MaxWinCredit = 1,
                MaxWinLiveCasino = 1,
                WinPercent = 1
            };

            _settingRepository.GetCurrentMemberWin(Arg.Is<GetCustomerWinLimitCriteria>(x => x.IsValid()))
                              .Returns(CloneCurrentMemberWin(expectedCurrentMemberWin));

            // Action
            var actualValue = _winLimitSettingService.GetCurrentWinLimit(custId);

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
                              .Returns(new CommandResultDTO { Code = ErrCode.InvalidCriteria });

            // Action
            var actualCommand = _winLimitSettingService.UpdateWinLimit(settingContext, memberWinLimit);

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
                              .Returns(new CommandResultDTO { Code = 0 });

            // Action
            var actualCommand = _winLimitSettingService.UpdateWinLimit(settingContext, memberWinLimit);

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

            _settingRepository.InsertOrUpdate(Arg.Is<UpdateCustomerWinLimitCriteria>(x => x.IsValid()))
                             .Returns(new CommandResultDTO { Code = 0 });

            // Action
            var updateCommand = _winLimitSettingService.UpdateWinLimitMultiple(settingContext, memberWinLimit);

            // Assert
            Assert.AreEqual(3, updateCommand.CommandResultsList.Count);
            Assert.AreEqual(1, updateCommand.CommandResultsList[0].Id);
            Assert.AreEqual(2, updateCommand.CommandResultsList[1].Id);
            Assert.AreEqual(3, updateCommand.CommandResultsList[2].Id);
        }

        private void AssertCurrentWinLimit(CurrentMemberWinDTO expectedValue, CurrentMemberWinDTO actualValue)
        {
            Assert.AreEqual(expectedValue.Balance, actualValue.Balance);
            Assert.AreEqual(expectedValue.MaxWinCredit, actualValue.MaxWinCredit);
            Assert.AreEqual(expectedValue.WinPercent, actualValue.WinPercent);
            Assert.AreEqual(expectedValue.MaxWinLiveCasino, actualValue.MaxWinLiveCasino);
        }

        private CurrentMemberWinDTO CloneCurrentMemberWin(CurrentMemberWinDTO currentMemberWin)
        {
            var result = new CurrentMemberWinDTO();

            result.Balance = currentMemberWin.Balance;
            result.MaxWinCredit = currentMemberWin.MaxWinCredit;
            result.MaxWinLiveCasino = currentMemberWin.MaxWinLiveCasino;
            result.WinPercent = currentMemberWin.WinPercent;

            return result;
        }
    }
}