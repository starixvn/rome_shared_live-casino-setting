namespace Fanex.LiveCasino.CoreSetting.Models
{
    /// <summary>
    /// Setting Context Multiple class.
    /// </summary>
    public class SettingContextMultiple : SettingContext
    {
        /// <summary>
        /// Gets or sets Target customers id for edit multiple.
        /// </summary>
        public string CustomerIds { get; set; }

        public string CustomerSyncedStatus { get; set; }

        /// <summary>
        /// Gets or sets Target User.
        /// </summary>
        public string Usernames { get; set; }
    }
}