namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting repository class.
    /// </summary>
    public class SettingRepository : BaseRepository, ISettingRepository
    {
        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer information.</returns>
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

        /// <summary>
        /// Gets the setting for edit super.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and commission model.</returns>
        public PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting.Commission = new Commission
                    {
                        Value = reader.ToDecimal("v162sdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToDecimal("v162sdicount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        S4 = reader.To<decimal>("v162spositiontaking_1101"),
                        L4 = reader.To<decimal>("v162spositiontaking_1101_max"),
                        N4 = reader.To<decimal>("v162spositiontaking_1101_min")
                    };
                });
        }

        /// <summary>
        /// Gets the setting for edit master.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and commission model.</returns>
        public PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting.Commission = new Commission
                    {
                        Value = reader.ToDecimal("v162discount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToDecimal("v162discount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        S4 = reader.To<decimal>("v162spositiontaking_1101_max"),
                        N3 = reader.To<decimal>("v162mpositiontaking_1101_min"),
                        Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                        Us3 = reader.To<decimal>("auto_v162spostiontaking_1101"),
                        S3 = reader.To<decimal>("v162spositiontaking_1101"),
                        M3 = reader.To<decimal>("v162mpositiontaking_1101"),
                        NewPresetOnMember = 0,
                        UpdateAllMembers = false
                    };
                });
        }

        /// <summary>
        /// Gets the setting for edit agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and commission model.</returns>
        public PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting.Commission = new Commission
                    {
                        Value = reader.ToDecimal("v162discount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToDecimal("v162discount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        IsDisableAuto = reader.To<bool>("DisableAutoMPT"),
                        M2 = reader.To<decimal>("v162mpositiontaking_1101"),
                        Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                        Um2 = reader.To<decimal>("auto_v162mpositiontaking_1101"),
                        A2 = reader.To<decimal>("v162apositiontaking_1101"),
                        M3 = reader.To<decimal>("v162mpositiontaking_1101_max"),
                        N2 = reader.To<decimal>("v162apositiontaking_1101_min"),
                        NewPresetOnMember = 0,
                        UpdateAllMembers = false
                    };
                });
        }

        /// <summary>
        /// Gets the setting for edit member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and commission model.</returns>
        public PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting.Commission = new Commission
                    {
                        Value = reader.ToDecimal("v162playerdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToDecimal("v162playerdiscount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        A1 = reader.To<decimal>("v162apositiontaking_1101"),
                        A2 = reader.To<decimal>("v162apositiontaking_1101_max"),
                        N2 = reader.To<decimal>("v162apositiontaking_1101_min"),
                    };
                });
        }

        /// <summary>
        /// Gets the setting for super edit member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and commission model.</returns>
        public PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        S1 = reader.To<decimal>("v162spositiontaking_1101"),
                        S1Max = reader.To<decimal>("v162spositiontaking_1101_max"),
                        S1Min = reader.To<decimal>("v162spositiontaking_1101_min"),
                        M1 = reader.To<decimal>("v162mpositiontaking_1101"),
                        A1 = reader.To<decimal>("v162apositiontaking_1101"),
                        Us1 = reader.To<decimal>("auto_v162spositiontaking_1101_mem"),
                        Is1 = reader.To<bool>("isauto_v162spositiontaking_1101_mem"),
                        Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                        S4 = reader.To<decimal>("auto_v162spositiontaking_1101_max")
                    };
                });
        }

        /// <summary>
        /// Gets the setting for master edit member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and commission model.</returns>
        public PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        IsDisableAuto = reader.To<bool>("DisableAutoMPT"),
                        M1 = reader.To<decimal>("v162mpositiontaking_1101"),
                        A1 = reader.To<decimal>("V162apositiontaking_1101"),
                        Um1 = reader.To<decimal>("auto_v162mpositiontaking_1101_mem"),
                        Im1 = reader.To<bool>("isauto_v162mpositiontaking_1101_mem"),
                        M1Max = reader.To<decimal>("v162mpositiontaking_1101_max"),
                        M1Min = reader.To<decimal>("v162mpositiontaking_1101_min"),
                        Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                        M3 = reader.To<decimal>("auto_v162mpositiontaking_1101_max")
                    };
                });
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="hasChecked">If set to <c>true</c> [has checked column].</param>
        /// <returns>List of game types.</returns>
        public List<GameType> GetGameSetting(ICriteria criteria, bool hasChecked)
        {
            var gameTypes = new List<GameType>();
            GameType current = null;

            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            var itemGameType = new GameType
                            {
                                Id = reader.ToInt("GameTypeID"),
                                BetTypeId = reader.ToInt("IBCBetTypeID")
                            };

                            var itemStakeLevel = new StakeLevel
                            {
                                Id = reader.ToInt("StakeLevelID"),
                                Range = reader.ToString("StakeLevelName"),
                                IsChecked = hasChecked ? reader.ToBool("IsChecked") : false
                            };

                            if (current == null || current.Id != itemGameType.Id)
                            {
                                gameTypes.Add(itemGameType);
                                current = itemGameType;
                                current.BettingLimit = new List<StakeLevel>();
                            }

                            current.BettingLimit.Add(itemStakeLevel);
                        }
                    }
                }
            }

            return gameTypes;
        }

        /// <summary>
        /// Inserts the game setting account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Game setting command result.</returns>
        public ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes)
        {
            var result = new GameSettingCommandResult();
            GameType current = null;

            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            var itemGameType = new GameType
                            {
                                Id = reader.ToInt("GameTypeID"),
                                BetTypeId = reader.ToInt("IBCBetTypeID")
                            };

                            var itemStakeLevel = new StakeLevel
                            {
                                Range = reader.ToString("StakeLevelName")
                            };

                            if (current == null || current.Id != itemGameType.Id)
                            {
                                gameTypes.Add(itemGameType);
                                current = itemGameType;
                                current.BettingLimit = new List<StakeLevel>();
                            }

                            current.BettingLimit.Add(itemStakeLevel);
                        }
                    }

                    var outParameters = objDb.GetOuputParams();
                    result = new GameSettingCommandResult(GetCommandResult(outParameters));
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the position taking and commission setting multiple.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="custIds">The customer ids.</param>
        /// <returns>Command result.</returns>
        public Dictionary<int, ICommandResult> UpdatePtAndCommSettingMultiple(ICriteria criteria, List<int> custIds)
        {
            Dictionary<int, ICommandResult> results = new Dictionary<int, ICommandResult>();

            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());
                    objDb.AssignParamValues(parameters);
                    foreach (var custId in custIds)
                    {
                        objDb.Database.SetParameterValue(objDb.DbCommand, "custid", custId);

                        objDb.ExecuteNonQuery();

                        var outParameters = objDb.GetOuputParams();
                        if (outParameters != null)
                        {
                            results[custId] = new CommandResult
                            {
                                Code = outParameters.Contains("errcode") ? outParameters["errcode"] : 0,
                                Message = outParameters.Contains("errmsg") ? outParameters["errmsg"].ToString() : string.Empty
                            };
                        }
                    }
                }
            }

            return results;
        }
    }
}