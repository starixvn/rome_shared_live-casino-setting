// --------------------------------------------------------------------------
// <copyright file="ISettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISettingRepository : IBaseRepository
    {
        CustInfo GetCustomerInfo(ICriteria criteria);

        #region Get Setting

        PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria);

        PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria);

        PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria);

        PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria);

        PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria);

        PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria);

        #endregion Get Setting

        #region Update Setting

        Dictionary<int, ICommandResult> UpdatePtAndCommSettingMultiple(ICriteria criteria, List<int> custIds);

        #endregion Update Setting

        #region Game Setting

        List<GameType> GetGameSetting(ICriteria criteria, bool hasChecked);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes);

        #endregion Game Setting
    }
}