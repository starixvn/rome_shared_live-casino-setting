// -----------------------------------------------------------------------
// <copyright file="SettingContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
        /// Current agent of member
        /// </summary>
        public int AgentId { get; set; }

        /// <summary>
        /// Current login customer user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Target customer id of edited customer
        /// </summary>
        public int TargetCustId { get; set; }

        /// <summary>
        /// Administrator id, using in Accounting or internal agent
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Sub-Account id using in Agency
        /// </summary>
        public int SubAccId { get; set; }

        /// <summary>
        /// Current role id of customer that is performing
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Currency id
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Site name
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Role id of edited customer
        /// </summary>
        public int TargetRoleId { get; set; }

        /// <summary>
        /// Indicate this is neutral site, need to hide other 3rd products...
        /// </summary>
        public bool IsNeutralSite { get; set; }

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

        public SettingContext()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId">Current login customer id or customer that is performing</param>
        /// <param name="userId">Administrator id, using in Accounting or internal agent</param>
        /// <param name="subAccId">Sub-Account id using in Agency</param>
        /// <param name="roleId">Current role id of customer that is performing</param>
        /// <param name="targetRoleId">Role id of edited customer</param>
        /// <param name="formId">Form type of edit: Edit Min/Max PT, Edit MinPT or Edit PT</param>
        public SettingContext(int custId, int userId, int subAccId, int roleId, int targetRoleId, FormType formType)
        {
            CustId = custId;
            AdminId = userId;
            SubAccId = subAccId;
            RoleId = roleId;
            TargetRoleId = targetRoleId;
            FormType = formType;
        }

        public bool IsEditSuper()
        {
            return FormType == FormType.Edit && TargetRoleId == (int)CustRole.Super;
        }

        public bool IsEditMaster()
        {
            return FormType == FormType.Edit && TargetRoleId == (int)CustRole.Master;
        }

        public bool IsEditAgent()
        {
            return FormType == FormType.Edit && TargetRoleId == (int)CustRole.Agent;
        }

        public bool IsEditMember()
        {
            return FormType == FormType.Edit && TargetRoleId == (int)CustRole.Member;
        }
    }
}
