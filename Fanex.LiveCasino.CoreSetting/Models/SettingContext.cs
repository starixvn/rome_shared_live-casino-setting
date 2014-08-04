// --------------------------------------------------------------------------
// <copyright file="SettingContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Models
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingContext
    {
        /// <summary>
        /// Current login customer id or customer that is performing
        /// </summary>
        public int CustId { get; set; }

        /// <summary>
        /// Current login customer user name
        /// </summary>
        public string UserName { get; set; }

        public string TargetUsername { get; set; }

        /// <summary>
        /// Target User
        /// </summary>
        public string TargetUsernames { get; set; }

        /// <summary>
        /// Target customer id of edited customer
        /// </summary>
        public int TargetCustId { get; set; }

        /// <summary>
        /// Target customers id for edit multiple.
        /// </summary>
        public string TargetCustIds { get; set; }

        /// <summary>
        /// Administrator id, using in Accounting or internal agent
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Sub-Account id using in Agency
        /// </summary>
        public int SubAccId { get; set; }

        public string SubAccName { get; set; }

        /// <summary>
        /// Current role id of customer that is performing
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Site name
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Role id of edited customer
        /// </summary>
        public int TargetRoleId { get; set; }

        /// <summary>
        /// Form type of edit: Edit Min/Max PT, Edit MinPT or Edit PT
        /// </summary>
        public FormType FormType { get; set; }

        /// <summary>
        /// IP of current request
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Language of current request
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// LangKey of current request
        /// </summary>
        public string LangKey { get; set; }

        /// <summary>
        /// Is edit multiple or not.
        /// </summary>
        public bool IsEditMultiple { get; set; }

        /// <summary>
        /// Currency identify.
        /// </summary>
        public int ExchangeId { get; set; }

        public int SuperId { get; set; }

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public SettingContext()
        {
        }

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

        public bool IsAgentEdittingMember
        {
            get
            {
                return TargetRoleId == (int)CustRole.Member;
            }
        }
    }
}