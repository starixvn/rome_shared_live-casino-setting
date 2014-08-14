namespace Fanex.LiveCasino.CoreSetting.UnitTest.Utilities
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using NUnit.Framework;

    /// <summary>
    /// Common utility testing class.
    /// </summary>
    [TestFixture]
    public class CommonUtilTest
    {
        [TestCase("", "en")]
        [TestCase("en-US", "en")]
        [TestCase("zh-TW", "ch")]
        [TestCase("zh-CN", "cs")]
        [TestCase("ja-JP", "jp")]
        [TestCase("th-TH", "th")]
        [TestCase("ko-KR", "ko")]
        [TestCase("vi-VN", "vn")]
        public void CurrentLanguage_WhenCalled_ReturnValue(string culture, string expectedLanguage)
        {
            // Arrange
            var common = CreateCommonUtil();
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            // Assert
            Assert.AreEqual(expectedLanguage, common.CurrentLanguage);
        }

        /// <summary>
        /// Gets the customer information from input string customer id, username and synced status.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <param name="usernames">The usernames.</param>
        /// <param name="syncedStatus">The synced status.</param>
        [TestCase("", "", "")]
        [TestCase(null, null, null)]
        [TestCase("1", "", "")]
        [TestCase("1", "master1,master2", "")]
        [TestCase("1", "master1", "1,0")]
        public void GetCustomerInfo_InvalidInput_EmptyList(string custIds, string usernames, string syncedStatus)
        {
            // Arrange
            var commonUtil = CreateCommonUtil();

            // Action
            var listCustomer = commonUtil.GetCustomerInfo(custIds, usernames, syncedStatus);

            // Assert
            var expertedCount = 0;
            Assert.AreEqual(expertedCount, listCustomer.Count);
        }

        [TestCase("1,3", "master1,master3", "1,1")]
        [TestCase("1", "master1", "0")]
        public void GetCustomerInfo_ValidInput_ReturnList(string custIds, string usernames, string syncedStatus)
        {
            // Arrange
            var commonUtil = CreateCommonUtil();

            // Action
            var listCustomer = commonUtil.GetCustomerInfo(custIds, usernames, syncedStatus);

            // Assert
            Assert.IsTrue(listCustomer.Any());
        }

        /// <summary>
        /// Gets the customer info_ valid input_ return valid.
        /// </summary>
        [Test]
        public void GetCustomerInfo_ValidInput_ReturnValid()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();
            string custIds = "1";
            string usernames = "master1";
            string syncedStatus = "0";

            // Action
            var listCustomer = commonUtil.GetCustomerInfo(custIds, usernames, syncedStatus);

            // Assert
            Assert.AreEqual(1, listCustomer.FirstOrDefault().CustId);
            Assert.AreEqual("master1", listCustomer.FirstOrDefault().UserName);
            Assert.AreEqual(false, listCustomer.FirstOrDefault().IsSyncLiveCasino);
        }

        [Test]        
        public void LiveCasinoSettings_NotFoundConfig_ReturnNull()
        {
            // Arrange
            var commonUtil = CreateCommonUtil();

            // Assert
            Assert.IsNull(commonUtil.LiveCasinoSettings);
        }

        private ICommonUtil CreateCommonUtil()
        {
            return new CommonUtil();
        }
    }
}