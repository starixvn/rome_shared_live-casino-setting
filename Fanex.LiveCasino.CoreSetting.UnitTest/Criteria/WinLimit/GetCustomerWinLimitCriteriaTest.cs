namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.WinLimit
{
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using NUnit.Framework;

    /// <summary>
    /// Testing GetCustomerWinLimitCriteria class.
    /// </summary>
    public class GetCustomerWinLimitCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [customer identifier is zero return false].
        /// </summary>
        [Test]
        public void IsValid_CustIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [customer identifier is less than zero return false].
        /// </summary>
        [Test]
        public void IsValid_CustIdLessThanZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = -1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [customer identifier is greater than zero return true].
        /// </summary>
        [Test]
        public void IsValid_CustIdGreaterThanZero_ReturnTrue()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = 1;

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
            const string SETTINGKEY = "Acc_CustomerSelWinLimitLiveCS";
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
            var expertedCount = 1;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("custid")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameter)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(parameters.ContainsKey(parameter));
        }

        private GetCustomerWinLimitCriteria CreateNewCriteria()
        {
            return new GetCustomerWinLimitCriteria();
        }
    }
}