namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using NUnit.Framework;

    /// <summary>
    /// Testing  LogInsertFromExistingAccount class.
    /// </summary>
    [TestFixture]
    public class LogInsertFromExistingAccountTest
    {
        /// <summary>
        /// SP the name of the name_ when called_ return.
        /// </summary>
        [Test]
        public void SpName_WhenCalled_ReturnName()
        {
            // Arrange
            var logModel = CreateNewInstance();

            // Assert
            const string SPNAME = "TP_User_Customer_AutoInsertAccountFromExisting";
            Assert.AreEqual(SPNAME, logModel.SpName);
        }

        private LogInsertFromExistingAccount CreateNewInstance()
        {
            return new LogInsertFromExistingAccount();
        }
    }
}