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

        public User Actor { get; set; }

        public string IP
        {
            get
            {
                var localIP = "127.0.0.1";

                var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                ip = ip.Equals("::1") ? localIP : (string.IsNullOrEmpty(ip) ? localIP : ip.Split(',')[0]);

                return string.IsNullOrWhiteSpace(_ip) ? ip : _ip;
            }

            set
            {
                _ip = value;
            }
        }

        public string Language { get; set; }
    }
}