namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;

    [TestFixture]
    public class AgentUpdateMemberSettingCriteriaTest
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
        /// Determines whether is valid criteria [customer identifier is negative return false].
        /// </summary>
        [Test]
        public void IsValid_CustIdIsNegative_ReturnFalse()
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



        private AgentUpdateMemberSettingCriteria CreateNewCriteria()
        {
            return new AgentUpdateMemberSettingCriteria();
        }
    }
}
