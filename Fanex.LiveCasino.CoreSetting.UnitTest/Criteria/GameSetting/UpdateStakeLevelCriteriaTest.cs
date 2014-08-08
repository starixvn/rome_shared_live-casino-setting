namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.GameSetting
{
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using NUnit.Framework;

    /// <summary>
    /// Testing the UpdateStakeLevelCriteria class.
    /// </summary>
    [TestFixture]
    public class UpdateStakeLevelCriteriaTestTest
    {
        /// <summary>
        /// Determines whether is valid criteria [customer identifier is zero return false].
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="gameTypeId">The game type identifier.</param>
        [TestCase(-1, 0, 0)]
        [TestCase(0, 0, 0)]
        [TestCase(0, -1, 0)]
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, -1)]
        [TestCase(0, 0, 0)]
        public void IsValid_InvalidCustIdAndRoleIdAndGameTypeId_ReturnFalse(int custId, int roleId, int gameTypeId)
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = custId;
            criteria.RoleId = roleId;
            criteria.GameTypeId = gameTypeId;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [customer identifier is greater than zero return true].
        /// </summary>
        [Test]
        public void IsValid_CustIdAndRoleIdAndGameTypeIdGreaterThanZero_ReturnTrue()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = 1;
            criteria.RoleId = 1;
            criteria.GameTypeId = 1;

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
            const string SETTINGKEY = "TP_User_CustGameType_UpdStakeLevel";
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
            var expertedCount = 4;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("custid")]
        [TestCase("roleid")]
        [TestCase("gametypeid")]
        [TestCase("stakelevellist")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameter)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(parameters.ContainsKey(parameter));
        }

        private UpdateStakeLevelCriteria CreateNewCriteria()
        {
            return new UpdateStakeLevelCriteria();
        }
    }
}