namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.GameSetting
{
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing GetGameSettingByExchangeCriteria class.
    /// </summary>
    public class GetGameSettingByExchangeCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [exchange identifier and site name].
        /// </summary>
        /// <param name="exchangeId">The exchange identifier.</param>
        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidExchangeId_ReturnFalse(int exchangeId)
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.ExchangeId = exchangeId;
            criteria.Site = Arg.Any<string>();

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [site is valid and exchange is valid return true].
        /// </summary>
        [Test]
        public void IsValid_ValidSiteAndExchangeId_ReturnTrue()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Site = "site name";
            criteria.ExchangeId = 1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsTrue(actualValue);
        }

        /// <summary>
        /// Gets the setting key when called.
        /// </summary>
        [Test]
        public void GetSettingKey_WhenCalled_ReturnSettingKey()
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            const string SETTINGKEY = "TP_Admin_StakeLevel_GetGameTypeStakeLevel";
            Assert.AreEqual(SETTINGKEY, actualSettingKey);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        [Test]
        public void GetSpParams_WhenCalled_ReturnParameters()
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            var expertedCount = 2;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("exchangeid")]
        [TestCase("site")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameter)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(parameters.ContainsKey(parameter));
        }

        private GetGameSettingByExchangeCriteria CreateNewCriteria()
        {
            return new GetGameSettingByExchangeCriteria();
        }
    }
}