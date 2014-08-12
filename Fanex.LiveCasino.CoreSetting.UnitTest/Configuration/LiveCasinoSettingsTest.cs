namespace Fanex.LiveCasino.CoreSetting.UnitTest.Configuration
{
    using System.Configuration;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using NUnit.Framework;

    /// <summary>
    /// Testing LiveCasinoSettings class.
    /// </summary>
    [TestFixture]
    public class LiveCasinoSettingsTest
    {
        [Test]
        public void SystemId_WhenCalled_ReturnFromConfiguration()
        {
            // Action
            var actualConfig = GetValidConfiguration();

            // Assert
            const int SYSTEMID = 1;
            Assert.AreEqual(SYSTEMID, actualConfig.SystemId);
        }

        /// <summary>
        /// Systems the id_ set new value_ return new value.
        /// </summary>
        [Test]
        public void SystemId_SetNewValue_ReturnNewValue()
        {
            // Arrange
            const int SYSTEMID = 2;

            // Action
            var actualConfig = GetValidConfiguration();
            actualConfig.SystemId = SYSTEMID;

            // Assert            
            Assert.AreEqual(SYSTEMID, actualConfig.SystemId);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SystemId_MissingConfiguration_ThrowException()
        {
            // Action
            var actualConfig = GetMissingSystemIdConfiguration();
        }

        [Test]
        public void UnAuthorizeUrl_WhenCalled_ReturnFromConfiguration()
        {
            // Action
            var actualConfig = GetValidConfiguration();

            // Assert
            const string UNAUTHORIZEURL = "UnAuthorized Url";
            Assert.AreEqual(UNAUTHORIZEURL, actualConfig.UnAuthorizeUrl);
        }

        /// <summary>
        /// Un Authorize url_ set new value_ return new value.
        /// </summary>
        [Test]
        public void UnAuthorizeUrl_SetNewValue_ReturnNewValue()
        {
            // Arrange
            const string UNAUTHORIZEURL = "UnAuthorizeUrl new";

            // Action
            var actualConfig = GetValidConfiguration();
            actualConfig.UnAuthorizeUrl = UNAUTHORIZEURL;

            // Assert            
            Assert.AreEqual(UNAUTHORIZEURL, actualConfig.UnAuthorizeUrl);
        }

        [Test]        
        public void UnAuthorizeUrl_MissingConfiguration_ReturnDefaultValue()
        {
            // Action
            var actualConfig = GetMissingUnAuthorizeUrlConfiguration();

            // Assert
            const string UNAUTHORIZEURL = "~/LiveCasinoSetting/LiveCasinoUser/UnAuthorized";
            Assert.AreEqual(UNAUTHORIZEURL, actualConfig.UnAuthorizeUrl);
        }

        private LiveCasinoSettings GetValidConfiguration()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "Configuration/WebTest.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LiveCasinoSettings section = config.GetSection("LiveCasinoSettings") as LiveCasinoSettings;

            return section;
        }

        private LiveCasinoSettings GetMissingSystemIdConfiguration()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "Configuration/MissingSystemIdWebTest.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LiveCasinoSettings section = config.GetSection("LiveCasinoSettings") as LiveCasinoSettings;

            return section;
        }

        private LiveCasinoSettings GetMissingUnAuthorizeUrlConfiguration()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "Configuration/MissingUnAuthorizeUrlWebTest.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LiveCasinoSettings section = config.GetSection("LiveCasinoSettings") as LiveCasinoSettings;

            return section;
        }
    }
}
