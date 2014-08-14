namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Update Super Setting Criteria Test.
    /// </summary>
    [TestFixture]
    public class UpdateSuperSettingCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [user identifier is zero return false].
        /// </summary>
        [Test]
        public void IsValid_UserIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.UserId = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [user identifier is negative return false].
        /// </summary>
        [Test]
        public void IsValid_UserIdIsNegative_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.UserId = -1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
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
            Assert.AreEqual("Acc_CustomerUpdSuperLiveCS_All", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("V162spositiontaking_1101")]
        [TestCase("V162spositiontaking_1101_min")]
        [TestCase("V162discount_1101")]
        [TestCase("V162UpdatePositiontaking_1101")]
        [TestCase("V162UpdateMinPositiontaking_1101")]
        [TestCase("V162UpdateCommission_1101")]
        [TestCase("IP")]
        [TestCase("UserId")]
        [TestCase("Language")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameterName)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var paramters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(paramters.ContainsKey(parameterName.ToLower()));
        }

        private UpdateSuperSettingCriteria CreateNewCriteria()
        {
            return new UpdateSuperSettingCriteria();
        }
    }
}