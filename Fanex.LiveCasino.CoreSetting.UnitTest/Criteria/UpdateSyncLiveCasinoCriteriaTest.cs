namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using NUnit.Framework;

    /// <summary>
    /// Testing the UpdateSyncLiveCasinoCriteria class.
    /// </summary>
    [TestFixture]
    public class UpdateSyncLiveCasinoCriteriaTest
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
            const string SETTINGKEY = "Acc_CustomerUpdSyncLiveCS_Cust";
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
            var expertedCount = 6;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("custid")]
        [TestCase("synclivecs")]
        [TestCase("userid")]
        [TestCase("subaccid")]
        [TestCase("ip")]
        [TestCase("language")]
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
            criteria.CustId = 1;
            criteria.Ip = "10.19.10.54";
            criteria.Language = "E";
            criteria.SubAccId = 0;
            criteria.SyncLiveCS = true;
            criteria.UserId = 2;

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'Acc_CustomerUpdSyncLiveCS_Cust'\nCustId = '1'\nSyncLiveCS = 'True'\nUserID = '2'\nSubAccID = '0'\nIP = '10.19.10.54'\nLanguage = 'E'";
            Assert.AreEqual(expected, actual);
        }

        private UpdateSyncLiveCasinoCriteria CreateNewCriteria()
        {
            return new UpdateSyncLiveCasinoCriteria();
        }
    }
}