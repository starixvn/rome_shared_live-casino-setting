namespace Fanex.LiveCasino.CoreSetting.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Live casino settings.
    /// </summary>
    public class LiveCasinoSettings : ConfigurationSection
    {
        private const string __LIVE_CASINO_SETTINGS = "LiveCasinoSettings";
        private const string __SYSTEM_ID = "SystemId";
        private const string __UN_AUTHORIZED_PATH = "UnAuthorizedPath";
        private const string __AUTHORIZATION_SERVICE_TYPE = "AuthorizationServiceType";
        private const string __LANGUAGE_SESSION_KEY = "LanguageSessionKey";
        private const string __LOG_CATEGORY = "LogCategory";
        private const string __IS_UM_TIME = "IsUMTime";
        private const string __UM_FROM = "UMFrom";
        private const string __UM_TO = "UMTo";

        private static LiveCasinoSettings _settings = ConfigurationManager.GetSection(__LIVE_CASINO_SETTINGS) as LiveCasinoSettings;

        public static LiveCasinoSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        [ConfigurationProperty(__SYSTEM_ID, IsRequired = true)]
        public int SystemId
        {
            get { return (int)this[__SYSTEM_ID]; }
            set { this[__SYSTEM_ID] = value; }
        }

        [ConfigurationProperty(__UN_AUTHORIZED_PATH, IsRequired = false, DefaultValue = "~/Areas/LiveCasinoSetting/Views/Common/UnAuthorized.cshtml")]
        public string UnAuthorizedPath
        {
            get { return (string)this[__UN_AUTHORIZED_PATH]; }
            set { this[__UN_AUTHORIZED_PATH] = value; }
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

        [ConfigurationProperty(__IS_UM_TIME, IsRequired = false, DefaultValue = false)]
        public bool IsUMTime
        {
            get { return (bool)this[__IS_UM_TIME]; }
            set { this[__IS_UM_TIME] = value; }
        }

        [ConfigurationProperty(__UM_FROM, IsRequired = false)]
        public string UMFrom
        {
            get { return (string)this[__UM_FROM]; }
            set { this[__UM_FROM] = value; }
        }

        [ConfigurationProperty(__UM_TO, IsRequired = false)]
        public string UMTo
        {
            get { return (string)this[__UM_TO]; }
            set { this[__UM_TO] = value; }
        }
    }
}