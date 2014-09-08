namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Update Agent Setting Criteria Test.
    /// </summary>
    [TestFixture]
    public class UpdateAgentSettingCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [master recommend id is zero then return false].
        /// </summary>
        [Test]
        public void IsValid_MRecommendIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.MRecommend = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [master recommend is negative return false].
        /// </summary>
        [Test]
        public void IsValid_MRecommendIdIsNegative_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.MRecommend = -1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

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
            Assert.AreEqual("Acc_CustomerUpdAgentLiveCS_All", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("MRecommend")]
        [TestCase("V162mpositiontaking_1101")]
        [TestCase("Auto_v162mpositiontaking_1101")]
        [TestCase("IsAuto_v162mpositiontaking_1101")]
        [TestCase("V162apositiontaking_1101")]
        [TestCase("Min_v162apositiontaking_1101")]
        [TestCase("V162discount_1101")]
        [TestCase("V162UpdatePositiontaking_1101")]
        [TestCase("V162UpdateAutoPositiontaking_1101")]
        [TestCase("V162UpdateMinPositiontaking_1101")]
        [TestCase("V162UpdateCommission_1101")]
        [TestCase("UpdateAllMembers")]
        [TestCase("NewMPositionTaking")]
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
            criteria.MRecommend = 100;
            criteria.M2 = 0.2M;
            criteria.Um2 = 0.3M;
            criteria.Im2 = true;
            criteria.A2 = 0.5M;
            criteria.N2 = 0.6M;
            criteria.AgentCommission = 0.9M;
            criteria.NewMasterPtOnMember = 0.8M;
            criteria.SubAccID = 0;
            criteria.UserId = 2;
            criteria.IP = "10.19.10.54";
            criteria.Language = "E";

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'Acc_CustomerUpdAgentLiveCS_All'\nCustId = '1'\nMRecommend = '100'\nM2 = '0.2'\nUm2 = '0.3'\nIm2 = 'True'\nA2 = '0.5'\nN2 = '0.6'\nAgentCommission = '0.9'\nNewMPositionTaking = '0.8'\nSubAccID = '0'\nIP = '10.19.10.54'\nUserId = '2'\nLanguage = 'E'";
            Assert.AreEqual(expected, actual);
        }

        private UpdateAgentSettingCriteria CreateNewCriteria()
        {
            return new UpdateAgentSettingCriteria();
        }
    }
}