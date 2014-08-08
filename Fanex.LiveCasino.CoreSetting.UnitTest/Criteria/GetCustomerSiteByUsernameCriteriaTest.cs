namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using NUnit.Framework;

    /// <summary>
    /// Testing the GetCustomerSiteByUsernameCriteria class.
    /// </summary>
    [TestFixture]
    public class GetCustomerSiteByUsernameCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [username is empty return false].
        /// </summary>
        [Test]
        public void IsValid_UsernameIsEmpty_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Username = string.Empty;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [username is null return false].
        /// </summary>
        [Test]
        public void IsValid_UsernameIsNull_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Username = null;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [username is valid return true].
        /// </summary>
        [Test]
        public void IsValid_ValidUsername_ReturnTrue()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Username = "username";

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
            const string SETTINGKEY = "Acc_CustomerSelSiteByUserName";
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
        [TestCase("username")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameter)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(parameters.ContainsKey(parameter));
        }

        private GetCustomerSiteByUsernameCriteria CreateNewCriteria()
        {
            return new GetCustomerSiteByUsernameCriteria();
        }
    }
}
