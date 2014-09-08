namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Test class for agent update member setting criteria.
    /// </summary>
    [TestFixture]
    public class AgentUpdateMemberSettingCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [recommend id is zero then return false].
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
        /// Determines whether is valid criteria [recommend is negative return false].
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
            Assert.AreEqual("Acc_CustomerUpdMemberLiveCS_All", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("Recommend")]
        [TestCase("V162apositiontaking_1101")]
        [TestCase("V162playerdiscount_1101")]
        [TestCase("V162UpdatePositiontaking_1101")]
        [TestCase("V162UpdateCommission_1101")]
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
            criteria.Recommend = 2;
            criteria.MemberCommission = 0.1M;
            criteria.SubAccID = 0;
            criteria.UserId = 2;
            criteria.IP = "10.19.10.54";
            criteria.Language = "E";

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'Acc_CustomerUpdMemberLiveCS_All'\nCustId = '1'\nRecommend = '2'\nMemberCommission = '0.1'\nSubAccID = '0'\nUserId = '2'\nIP = '10.19.10.54'\nLanguage = 'E'";
            Assert.AreEqual(expected, actual);
        }

        private AgentUpdateMemberSettingCriteria CreateNewCriteria()
        {
            return new AgentUpdateMemberSettingCriteria();
        }
    }
}