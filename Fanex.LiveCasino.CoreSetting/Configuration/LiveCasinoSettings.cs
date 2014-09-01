namespace Fanex.LiveCasino.CoreSetting.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Live casino settings.
    /// </summary>
    public class LiveCasinoSettings : ConfigurationSection
    {
        private const string __SYSTEM_ID = "SystemId";
        private const string __UN_AUTHORIZED_URL = "UnAuthorizedUrl";
        private const string __AUTHORIZATION_SERVICE_TYPE = "AuthorizationServiceType";
        private const string __LANGUAGE_SESSION_KEY = "LanguageSessionKey";
        private const string __LOG_CATEGORY = "LogCategory";

        [ConfigurationProperty(__SYSTEM_ID, IsRequired = true)]
        public int SystemId
        {
            get { return (int)this[__SYSTEM_ID]; }
            set { this[__SYSTEM_ID] = value; }
        }

        [ConfigurationProperty(__UN_AUTHORIZED_URL, IsRequired = false, DefaultValue = "~/LiveCasinoSetting/LiveCasinoUser/UnAuthorized")]
        public string UnAuthorizeUrl
        {
            get { return (string)this[__UN_AUTHORIZED_URL]; }
            set { this[__UN_AUTHORIZED_URL] = value; }
        }

        [ConfigurationProperty(__AUTHORIZATION_SERVICE_TYPE, IsRequired = false, DefaultValue = "")]
        public string AuthorizationServiceType
        {
            get { return (string)this[__AUTHORIZATION_SERVICE_TYPE]; }
            set { this[__AUTHORIZATION_SERVICE_TYPE] = value; }
        }

        [ConfigurationProperty(__LANGUAGE_SESSION_KEY, IsRequired = false, DefaultValue = "")]
        public string LanguageSessionKey
        {
            get { return (string)this[__LANGUAGE_SESSION_KEY]; }
            set { this[__LANGUAGE_SESSION_KEY] = value; }
        }

        [ConfigurationProperty(__LOG_CATEGORY, IsRequired = false, DefaultValue = "LiveCasinoSetting")]
        public string LogCategory
        {
            get { return (string)this[__LOG_CATEGORY]; }
            set { this[__LOG_CATEGORY] = value; }
        }
    }
}