namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Diagnostics.CodeAnalysis;
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
        /// Gets or sets Current login customer id or customer that is performing.
        /// </summary>
        public int CustId { get; set; }

        /// <summary>
        /// Gets or sets Current login customer user name.
        /// </summary>
        public string UserName { get; set; }

        public string TargetUsername { get; set; }

        /// <summary>
        /// Gets or sets Target User.
        /// </summary>
        public string TargetUsernames { get; set; }

        /// <summary>
        /// Gets or sets Target customer id of edited customer.
        /// </summary>
        public int TargetCustId { get; set; }

        /// <summary>
        /// Gets or sets Target customers id for edit multiple.
        /// </summary>
        public string TargetCustIds { get; set; }

        /// <summary>
        /// Gets or sets Administrator id, using in Accounting or internal agent.
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Gets or sets Sub-Account id using in Agency.
        /// </summary>
        public int SubAccId { get; set; }

        public string SubAccName { get; set; }

        /// <summary>
        /// Gets or sets Current role id of customer that is performing.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets Site name.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets Role id of edited customer.
        /// </summary>
        public int TargetRoleId { get; set; }

        /// <summary>
        /// Gets or sets Form type of edit: Edit Min/Max PT, Edit MinPT or Edit PT.
        /// </summary>
        public FormType FormType { get; set; }

        /// <summary>
        /// Gets or sets IP of current request.
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Gets or sets Language of current request.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets LangKey of current request.
        /// </summary>
        public string LangKey { get; set; }

        public bool IsEditMultiple { get; set; }

        /// <summary>
        /// Gets or sets Currency identify.
        /// </summary>
        public int ExchangeId { get; set; }

        public int SuperId { get; set; }

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public int OldCustId { get; set; }

        public string ToUsername { get; set; }

        public int DirectUplineId { get; set; }

        public string TargetSyncStatus { get; set; }

        public int UserIdDoAction
        {
            get
            {
                return AdminId > 0 ? AdminId : CustId;
            }
        }

        public bool IsEdittingSuper
        {
            get
            {
                return TargetRoleId == (int)CustRole.Super;
            }
        }

        public bool IsEdittingMaster
        {
            get
            {
                return TargetRoleId == (int)CustRole.Master;
            }
        }

        public bool IsEdittingAgent
        {
            get
            {
                return TargetRoleId == (int)CustRole.Agent;
            }
        }

        public bool IsEdittingMember
        {
            get
            {
                return TargetRoleId == (int)CustRole.Member;
            }
        }

        public bool IsAgentEditingMember
        {
            get
            {
                return IsEdittingMember && RoleId == (int)CustRole.Agent;
            }
        }

        public bool IsEditDirectDownline
        {
            get
            {
                return IsEdittingSuper || (RoleId - TargetRoleId) == 1; 
            }
        }
    }
}