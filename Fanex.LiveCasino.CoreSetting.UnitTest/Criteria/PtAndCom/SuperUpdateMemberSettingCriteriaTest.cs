namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
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

        /// <summary>
        /// To the string_ when called_ return string.
        /// </summary>
        [Test]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public void ToString_WhenCalled_ReturnString()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = 1;
            criteria.S1 = 0.1M;
            criteria.Is1 = false;
            criteria.Us1 = 0.2M;
            criteria.IsUpdateSuperPt = true;
            criteria.SubAccID = 0;
            criteria.UserId = 2;
            criteria.IP = "10.19.10.54";
            criteria.Language = "E";

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'Acc_CustomerUpdMemberLiveCS_SAg'\nCustId = '1'\nS1 = '0.1'\nIs1 = 'False'\nUs1 = '0.2'\nIsUpdateSuperPt = 'True'\nSubAccID = '0'\nUserId = '2'\nIP = '10.19.10.54'\nLanguage = 'E'";
            Assert.AreEqual(expected, actual);
        }

        private SuperUpdateMemberSettingCriteria CreateNewCriteria()
        {
            return new SuperUpdateMemberSettingCriteria();
        }
    }
}