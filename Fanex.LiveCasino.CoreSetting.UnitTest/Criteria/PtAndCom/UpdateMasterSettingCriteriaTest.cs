namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Update Master Setting Criteria Test.
    /// </summary>
    [TestFixture]
    public class UpdateMasterSettingCriteriaTest
    {
        /// <summary>
        /// Determines whether is valid criteria [super recommend id is zero then return false].
        /// </summary>
        [Test]
        public void IsValid_SRecommendIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SRecommend = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [super recommend is negative return false].
        /// </summary>
        [Test]
        public void IsValid_SRecommendIdIsNegative_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.SRecommend = -1;

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
            Assert.AreEqual("Acc_CustomerUpdMasterLiveCS_All", settingKey);
        }

        /// <summary>
        /// Determines whether Get SP_PARAMS method return data contain parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name for checking contains.</param>
        [TestCase("CustId")]
        [TestCase("SRecommend")]
        [TestCase("V162spositiontaking_1101")]
        [TestCase("V162mpositiontaking_1101")]
        [TestCase("IsAuto_v162spositiontaking_1101")]
        [TestCase("Auto_v162spositiontaking_1101")]
        [TestCase("V162mpositiontaking_1101_min")]
        [TestCase("V162discount_1101")]
        [TestCase("V162UpdatePositiontaking_1101")]
        [TestCase("V162UpdateMinPositiontaking_1101")]
        [TestCase("V162UpdateAutoPositiontaking_1101")]
        [TestCase("V162UpdateCommission_1101")]
        [TestCase("UpdateAllMembers")]
        [TestCase("NewSPositionTaking")]
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
            
            criteria.SubAccID = 0;
            criteria.UserId = 2;
            criteria.IP = "10.19.10.54";
            criteria.Language = "E";

            // Action
            var actual = criteria.ToString();

            // Assert
            var expected = "SP Name = 'Acc_CustomerUpdMasterLiveCS_All'\nCustId = '1'\nSRecommend = '{2}'\nS3 = '{3}'\nM3 = '{4}'\nIs3 = '{5}'\nUs3 = '{6}'\nN3 = '{7}'\nUpdateAllMembers = '{8}'\nNewSPositionTaking = '{9}'\nSubAccID = '0'\nIP = '10.19.10.54'\nUserId = '2'\nLanguage = 'E'";
            Assert.AreEqual(expected, actual);
        }

        private UpdateMasterSettingCriteria CreateNewCriteria()
        {
            return new UpdateMasterSettingCriteria();
        }
    }
}