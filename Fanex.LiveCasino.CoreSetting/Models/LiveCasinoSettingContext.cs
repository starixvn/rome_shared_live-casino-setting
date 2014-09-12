namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Web;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Setting Context.
    /// </summary>
    public class LiveCasinoSettingContext
    {
        private string _ip = string.Empty;

        public Customer Customer { get; set; }

        public Customer Upline { get; set; }

        public User SubAccount { get; set; }

        public FormType FormType { get; set; }

        public int ActorId { get; set; }

        public string IP
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_ip) ? HttpContext.Current.Request.UserHostAddress : _ip;
            }

            set
            {
                _ip = value;
            }
        }

        public string Language { get; set; }
    }
}