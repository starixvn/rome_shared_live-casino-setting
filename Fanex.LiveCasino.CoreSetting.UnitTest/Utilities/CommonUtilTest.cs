namespace Fanex.LiveCasino.CoreSetting.UnitTest.Utilities
{
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using NUnit.Framework;

    /// <summary>
    /// Common utility testing class.
    /// </summary>
    [TestFixture]
    public class CommonUtilTest
    {
        /// <summary>
        /// Gets the customer information from input string customer id, username and synced status.
        /// </summary>
        [Test]
        public void GetCustomerInfo_InputIsStringEmpty_EmptyList()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            var custIds = string.Empty;
            var usernames = string.Empty;
            var syncStatus = string.Empty;

            // Action
            var listCustomer = commonUtil.GetCustomerInfo(custIds, usernames, syncStatus);

            // Assert
            var expertedCount = 0;
            Assert.AreEqual(expertedCount, listCustomer.Count);
        }

        /// <summary>
        /// Gets the customer information from input string customer id, username and synced status.
        /// </summary>
        [Test]
        public void GetCustomerInfo_InputIsNull_EmptyList()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            string custIds = null;
            string usernames = null;
            string syncStatus = null;

            // Action
            var listCustomer = commonUtil.GetCustomerInfo(custIds, usernames, syncStatus);

            // Assert
            var expertedCount = 0;
            Assert.AreEqual(expertedCount, listCustomer.Count);
        }

        private ICommonUtil CreateCommonUtil()
        {
            return new CommonUtil();
        }
    }
}