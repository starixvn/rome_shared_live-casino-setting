// -----------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.Common.Extensions;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingRepository : BaseRepository, ISettingRepository
    {
        public CustInfo GetCustomerInfo(ICriteria criteria)
        {
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var custInfo = objectDb.QueryEntity<CustInfo, MemberWinLimit, MemberWinLimit, CustInfo>(
                    (ci, mw, smw) => { ci.MemberWinLimit = mw; mw.Upline = smw; return ci; },
                    "isAutoResetLiveCasino,UplineIsAutoResetLiveCasino",
                    criteria.GetSpParams());

                return custInfo;
            }
        }


        public CustSettingViewModel GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria)
        {
            return GetCommonSettingForEdit(criteria, (reader, settingViewModel)
                =>
                {
                    settingViewModel.Commission = new Commission
                    {
                        Value = reader.ToDecimal("v162sdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToDecimal("v162sdicount_1101_max")
                        }
                    };

                    settingViewModel.PositionTaking = new PositionTaking
                    {
                        S4 = reader.To<decimal?>("v162spositiontaking_1101"),
                        L4 = reader.To<decimal?>("v162spositiontaking_1101_max")
                    };
                });
        }

        public CustSettingViewModel GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        public CustSettingViewModel GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        public CustSettingViewModel GetSettingForEditMember(GetSettingForEditMemberCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        public CustSettingViewModel GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        public CustSettingViewModel GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria)
        {
            throw new System.NotImplementedException();
        }
    }
}
