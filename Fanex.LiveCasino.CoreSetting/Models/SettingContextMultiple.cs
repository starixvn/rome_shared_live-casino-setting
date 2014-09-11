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
        public string TargetCustIds { get; set; }

        public string TargetSyncStatus { get; set; }

        /// <summary>
        /// Gets or sets Target User.
        /// </summary>
        public string TargetUsernames { get; set; }
    }
}