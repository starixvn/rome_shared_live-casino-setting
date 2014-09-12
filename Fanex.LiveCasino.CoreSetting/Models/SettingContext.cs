namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Web;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Setting Context.
    /// </summary>
    public class SettingContext
    {
        public Customer Customer { get; set; }

        public Customer Upline { get; set; }

        public User SubAccount { get; set; }

        public FormType FormType { get; set; }

        public int ActorId { get; set; }

        public string IP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        public string Language { get; set; }
    }
}