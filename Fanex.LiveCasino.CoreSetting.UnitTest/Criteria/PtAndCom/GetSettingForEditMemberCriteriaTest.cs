namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Get Setting For Edit Member Criteria Test.
    /// </summary>
    [TestFixture]
    public class GetSettingForEditMemberCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [agent recommend id is zero then return false].
        /// </summary>
        [Test]
        public void IsValid_RecommendIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Recommend = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [agent recommend is negative return false].
        /// </summary>
        [Test]
        public void IsValid_RecommendIdIsNegative_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Recommend = -1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [agent recommend is positive return true].
        /// </summary>
        [Test]
        public void IsValid_RecommendIdIsPositiveAndCustIdIsPositive_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.Recommend = 1;
            criteria.CustId = 1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsTrue(actualValue);
        }

        /// <summary>
        /// Determines whether criteria return setting [GetSettingKey function return setting key].
        /// </summary>
        [Test]
        public void GetSettingKey_WhenCalled_ReturnSettingKey()
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Acc_CustomerSelEditMemberLiveCS", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("Recommend")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameterName)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var paramters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(paramters.ContainsKey(parameterName.ToLower()));
        }

        private GetSettingForEditMemberCriteria CreateNewCriteria()
        {
            return new GetSettingForEditMemberCriteria();
        }
    }
}