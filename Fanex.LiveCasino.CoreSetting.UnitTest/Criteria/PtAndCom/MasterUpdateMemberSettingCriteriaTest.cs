namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Master Update Member Setting Criteria Test.
    /// </summary>
    [TestFixture]
    public class MasterUpdateMemberSettingCriteriaTest
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
            Assert.AreEqual("Acc_CustomerUpdMemberLiveCS_MAg", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("V162mpositiontaking_1101")]
        [TestCase("Auto_v162mpositiontaking_mem_1101")]
        [TestCase("IsAuto_v162mpositiontaking_mem_1101")]
        [TestCase("IsAuto_v162mpositiontaking_1101")]
        [TestCase("V162UpdatePositiontaking_1101")]
        [TestCase("V162UpdateAutoPositiontaking_1101")]
        [TestCase("UserId")]
        [TestCase("SubAccID")]
        [TestCase("Language")]
        [TestCase("IP")]
        [TestCase("Username")]
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
            criteria.M1 = 0.1M;
            criteria.Um1 = 0.2M;
            criteria.Im1 = false;
            criteria.Im2 = true;
            criteria.Username = "member";
            criteria.SubAccID = 0;
            criteria.UserId = 2;
            criteria.IP = "10.19.10.54";
            criteria.Language = "E";

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'Acc_CustomerUpdMemberLiveCS_MAg'\nCustId = '1'\nM1 = '0.1'\nUm1 = '0.2'\nIm1 = 'False'\nIm2 = 'True'\nUsername = 'member'\nSubAccID = '0'\nUserId = '2'\nIP = '10.19.10.54'\nLanguage = 'E'";
            Assert.AreEqual(expected, actual);
        }

        private MasterUpdateMemberSettingCriteria CreateNewCriteria()
        {
            return new MasterUpdateMemberSettingCriteria();
        }
    }
}