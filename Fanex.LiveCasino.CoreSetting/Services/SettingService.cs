namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;
    using Newtonsoft.Json;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingService : ISettingService
    {
        private static JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };
        private readonly ISettingRepository _settingRepository;

        public SettingService() : this(null) { }

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public ICommandResult UpdateLiveCasinoStatus(UpdateLiveCasinoStatusCriteria criteria)
        {
            return _settingRepository.Update(criteria);
        }

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="criteria">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public CustInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            return _settingRepository.GetCustomerInfo(criteria);
        }

        public string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName)
        {
            var custSetting = new CustSettingViewModel
            {
                SettingContext = settingContext,
                CustInfo = GetCustomerInfo(new GetCustomerInfoCriteria
                {
                    Custid = settingContext.TargetCustId
                }),
                PtAndComSetting = GetPtAndComSetting(settingContext)
            };

            return FormatJavascript(custSetting, objectName);
        }

        #region Pt and Com

        public PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria)
        {
            return _settingRepository.GetSettingForEditSuper(criteria);
        }

        public PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria)
        {
            return _settingRepository.GetSettingForEditMaster(criteria);
        }

        public PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria)
        {
            return _settingRepository.GetSettingForEditAgent(criteria);
        }

        public PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForEditMember(criteria);
        }

        public PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForSuperEditMember(criteria);
        }

        public PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForMasterEditMember(criteria);
        }

        public ICommandResult UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            return _settingRepository.Update(new UpdateSuperSettingCriteria { 
                Custid = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserID = settingContext.GetUserIdDoAction(),
                v162discount_1101 = ptAndComSetting.Commission.Value,
                v162UpdateCommission_1101 = true,
                v162spositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.S4,
                v162UpdatePositiontaking_1101 = true,
                v162spositiontaking_1101_min = (decimal)ptAndComSetting.PositionTaking.N4,
                v162UpdateMinPositiontaking_1101 = true
            });
        }

        public ICommandResult UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            throw new System.NotImplementedException();
        }

        public ICommandResult UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            throw new System.NotImplementedException();
        }

        public ICommandResult AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            throw new System.NotImplementedException();
        }

        public ICommandResult SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            throw new System.NotImplementedException();
        }

        public ICommandResult MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Win Limit

        public CurrentMemberWin GetCurrentMemberWinLimit(int custId)
        {
            return _settingRepository.Get<CurrentMemberWin>(new GetCustomerWinLimitCriteria
            {
                CustId = custId
            });
        }

        public ICommandResult UpdateWinLimit(UpdateCustomerWinLimitCriteria criteria)
        {
            return _settingRepository.Update(criteria);
        }

        #endregion

        #region Private Methods

        private PtAndComSetting GetPtAndComSetting(SettingContext settingContext)
        {
            switch ((CustRole)settingContext.TargetRoleId)
            {
                case CustRole.Super:
                    return GetSettingForEditSuper(new GetSettingForEditSuperCriteria
                    {
                        Custid = settingContext.TargetCustId
                    });
                case CustRole.Master:
                    return GetSettingForEditMaster(new GetSettingForEditMasterCriteria
                        {
                            CustId = settingContext.TargetCustId,
                            SRecommend = settingContext.CustId
                        });
                case CustRole.Agent:
                    return GetSettingForEditAgent(new GetSettingForEditAgentCriteria
                        {
                            CustId = settingContext.TargetCustId,
                            MRecommend = settingContext.CustId
                        });
                case CustRole.Member:
                    switch ((CustRole)settingContext.RoleId)
                    {
                        case CustRole.Super:
                            return GetSettingForSuperEditMember(new GetSettingForSuperEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId
                            });
                        case CustRole.Master:
                            return GetSettingForMasterEditMember(new GetSettingForMasterEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId
                            });
                        default:
                            return GetSettingForEditMember(new GetSettingForEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId,
                                Recommend = settingContext.CustId
                            });
                    }
            }

            return null;
        }


        private string FormatJavascript(object obj, string name)
        {
            if (obj != null)
            {
                return string.Format("var {0} = {1}; \n", name, JsonConvert.SerializeObject(obj, jsonSerializeSettings));
            }

            return string.Empty;
        }

        #endregion
    }
}