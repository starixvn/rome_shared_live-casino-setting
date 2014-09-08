namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.Log
{
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using NUnit.Framework;

    /// <summary>
    /// Testing InsertLogUserRunSPCriteria class.
    /// </summary>
    public class InsertLogUserRunSPCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [SP name is empty return false].
        /// </summary>
        [Test]
        public void IsValid_SpNameIsEmpty_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SpName = string.Empty;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [SP name is valid return true].
        /// </summary>
        [Test]
        public void IsValid_SpName_ReturnTrue()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SpName = "sp name";

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
            const string SETTINGKEY = "WriteLog_UserLog_RunSP";
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
            var expertedCount = 8;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("userid")]
        [TestCase("custid")]
        [TestCase("spname")]
        [TestCase("doing")]
        [TestCase("actionid")]
        [TestCase("trackingvalue")]
        [TestCase("trackingvalue2")]
        [TestCase("duration")]
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
        public void ToString_WhenCalled_ReturnString()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.ActionId = 1;
            criteria.CustId = 2;
            criteria.SpName = "stored procedure name";
            criteria.TrackingValue = "<Id='1' Name='one'/>";
            criteria.TrackingValue2 = "<Id='2' Name='two'/>";
            criteria.UserId = 3;

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "UserId = '3'\n CustId = '2'\n SpName = 'stored procedure name'\n ActionId = '1'\n OldValue = ' < Id='1' Name='one'/>'\n NewValue = ' < Id='2' Name='two'/>'";
            Assert.AreEqual(expected, actual);
        }

        private InsertLogUserRunSPCriteria CreateNewCriteria()
        {
            return new InsertLogUserRunSPCriteria();
        }
    }
}