namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Setting Context.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1609:PropertyDocumentationMustHaveValue", Justification = "Reviewed.")]
    public class SettingContext
    {
        public SettingContext()
        {
        }

        /// <summary>
        /// Gets or sets Target customer id of edited customer.
        /// </summary>
        public int CustomerId { get; set; }

        public string Username { get; set; }

        public string ToUsername { get; set; }

        /// <summary>
        /// Gets or sets Role id of edited customer.
        /// </summary>
        public CustomerLevel CustomerLevel { get; set; }

        /// <summary>
        /// Gets or sets Current login customer id or customer that is performing.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets Current login customer user name.
        /// </summary>
        public string ParentUsername { get; set; }

        /// <summary>
        /// Gets or sets Current role id of customer that is performing.
        /// </summary>
        public CustomerLevel ParentLevel { get; set; }

        public int UserId
        {
            get
            {
                return AdminId > 0 ? AdminId : ParentId;
            }
        }

        /// <summary>
        /// Gets or sets Administrator id, using in Accounting or internal agent.
        /// </summary>
        public int AdminId { get; set; }

        public int DirectParentId { get; set; }

        /// <summary>
        /// Gets or sets Currency identify.
        /// </summary>
        public int ExchangeId { get; set; }

        /// <summary>
        /// Gets or sets Form type of edit: Edit Min/Max PT, Edit MinPT or Edit PT.
        /// </summary>
        public FormType FormType { get; set; }

        /// <summary>
        /// Gets or sets IP of current request.
        /// </summary>
        public string IP
        {
            get
            {
                HttpContext context = HttpContext.Current;

                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public bool IsEditMultiple { get; set; }

        public bool IsCustomerSyncedLiveCS { get; set; }

        /// <summary>
        /// Gets or sets Language of current request.
        /// </summary>
        public string Language { get; set; }

        public int OldCustId { get; set; }

        /// <summary>
        /// Gets or sets Site name.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets Sub-Account id using in Agency.
        /// </summary>
        public int SubAccId { get; set; }

        public string SubAccName { get; set; }

        public int AgentId { get; set; }

        public int MasterId { get; set; }

        public int SuperId { get; set; }
    }
}