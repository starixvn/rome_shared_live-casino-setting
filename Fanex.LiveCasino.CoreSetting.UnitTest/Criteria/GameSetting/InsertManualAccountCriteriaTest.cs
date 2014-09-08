namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.GameSetting
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using NUnit.Framework;

    /// <summary>
    /// Testing the InsertManualAccountCriteria class.
    /// </summary>
    [TestFixture]
    public class InsertManualAccountCriteriaTest
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
            const string SETTINGKEY = "TP_User_Customer_ManualInsertAccount";
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
            var expertedCount = 13;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("custid")]
        [TestCase("casinono")]
        [TestCase("exchangeid")]
        [TestCase("scustid")]
        [TestCase("mcustid")]
        [TestCase("acustid")]
        [TestCase("roleid")]
        [TestCase("signupip")]
        [TestCase("signupby")]
        [TestCase("signupfrom")]
        [TestCase("modifyusername")]
        [TestCase("username")]
        [TestCase("custsite")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameter)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(parameters.ContainsKey(parameter));
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
            criteria.CustSite = "ODD-BETS";
            criteria.CustId = 4;
            criteria.AgentId = 3;
            criteria.CasinoNo = "01";
            criteria.ExchangeId = 20;
            criteria.MasterId = 2;
            criteria.ModifyUsername = "sa";
            criteria.RoleId = 1;
            criteria.SignupBy = 8;
            criteria.SignupFrom = 1;
            criteria.SignupIp = "10.19.10.54";
            criteria.SuperId = 1;
            criteria.Username = "member";

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'TP_User_Customer_ManualInsertAccount'\nCustId = '4'\nCasinoNo = '01'\nExchangeId = '20'\nSuperId = '1'\nMasterId = '2'\nAgentId = '3'\nRoleId = '1'\nSignupIp = '10.19.10.54'\nSignupBy = '8'\nSignupFrom = '1'\nModifyUsername = 'sa'\nUsername = 'member'\nCustSite = 'ODD-BETS'";
            Assert.AreEqual(expected, actual);
        }

        private InsertManualAccountCriteria CreateNewCriteria()
        {
            return new InsertManualAccountCriteria();
        }
    }
}