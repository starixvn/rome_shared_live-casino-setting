namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Super Update Member Setting Criteria Test.
    /// </summary>
    [TestFixture]
    public class SuperUpdateMemberSettingCriteriaTest
    {
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
            Assert.AreEqual("Acc_CustomerUpdMemberLiveCS_SAg", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("V162spositiontaking_1101")]
        [TestCase("IsAuto_v162spositiontaking_1101")]
        [TestCase("V162UpdatePositiontaking_1101")]
        [TestCase("V162UpdateAutoPositiontaking_1101")]
        [TestCase("SubAccID")]
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

        private SuperUpdateMemberSettingCriteria CreateNewCriteria()
        {
            return new SuperUpdateMemberSettingCriteria();
        }
    }
}