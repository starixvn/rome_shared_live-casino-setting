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
    }
}