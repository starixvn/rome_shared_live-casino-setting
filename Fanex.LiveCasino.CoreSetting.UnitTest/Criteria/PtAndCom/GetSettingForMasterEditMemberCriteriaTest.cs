namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Get Setting For Master Edit Member CriteriaTest.
    /// </summary>
    [TestFixture]
    public class GetSettingForMasterEditMemberCriteriaTest
    {
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
            Assert.AreEqual("Acc_CustomerSelMEditCustomerLiveCS", settingKey);
        }

        private GetSettingForMasterEditMemberCriteria CreateNewCriteria()
        {
            return new GetSettingForMasterEditMemberCriteria();
        }
    }
}