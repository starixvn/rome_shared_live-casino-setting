namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Get setting for editing master criteria test.
    /// </summary>
    [TestFixture]
    public class GetSettingForEditMasterCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [super recommend id is zero then return false].
        /// </summary>
        [Test]
        public void IsValid_SRecommendIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SRecommend = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [super recommend is negative return false].
        /// </summary>
        [Test]
        public void IsValid_SRecommendIdIsNegative_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SRecommend = -1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [super recommend is positive and customer id positive return true].
        /// </summary>
        [Test]
        public void IsValid_SRecommendIdIsPositiveAndCustIdIsPositive_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SRecommend = 1;
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
            Assert.AreEqual("Acc_CustomerSelEditMasterLiveCS", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("SRecommend")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameterName)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var paramters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(paramters.ContainsKey(parameterName.ToLower()));
        }

        private GetSettingForEditMasterCriteria CreateNewCriteria()
        {
            return new GetSettingForEditMasterCriteria();
        }
    }
}