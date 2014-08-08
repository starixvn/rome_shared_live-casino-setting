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
        /// Customer identifier string is empty return empty list.
        /// </summary>
        [Test]
        public void GetCustIds_CustIdStringIsEmpty_EmptyList()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            var custIds = string.Empty;

            // Action
            var listCustId = commonUtil.GetCustIds(custIds);

            // Assert
            var expertedCount = 0;
            Assert.AreEqual(expertedCount, listCustId.Count);
        }

        /// <summary>
        /// Customer identifier string is null return empty list.
        /// </summary>
        [Test]
        public void GetCustIds_CustIdStringIsNull_EmptyList()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            string custIds = null;

            // Action
            var listCustId = commonUtil.GetCustIds(custIds);

            // Assert
            var expertedCount = 0;
            Assert.AreEqual(expertedCount, listCustId.Count);
        }

        /// <summary>
        /// Customer identifier string is valid return list customer id.
        /// </summary>
        [Test]
        public void GetCustIds_ValidCustIdString_ListCustId()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            var custIds = "1,2,3";

            // Action
            var listCustId = commonUtil.GetCustIds(custIds);

            // Assert
            var expertedCount = 3;
            Assert.AreEqual(expertedCount, listCustId.Count);
            Assert.AreEqual(1, listCustId[0]);
            Assert.AreEqual(2, listCustId[1]);
            Assert.AreEqual(3, listCustId[2]);
        }

        /// <summary>
        /// Customer identifier string is valid return list customer id.
        /// </summary>
        [Test]
        public void GetCustIds_InvalidCustIdString_ListCustId()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            var custIds = "1,,3";

            // Action
            var listCustId = commonUtil.GetCustIds(custIds);

            // Assert
            var expertedCount = 2;
            Assert.AreEqual(expertedCount, listCustId.Count);
            Assert.AreEqual(1, listCustId[0]);
            Assert.AreEqual(3, listCustId[1]);
        }

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