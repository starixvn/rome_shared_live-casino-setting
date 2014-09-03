namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public CustInfo GetCustomerInfo(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var custInfo = new CustInfo();
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var parameters = criteria.GetSpParams();
                        using (var reader = objectDb.ExecuteReader(parameters))
                        {
                            if (reader.Read())
                            {
                                custInfo.CurrencyId = reader.To<byte>("currency");
                                custInfo.CustId = Converter.To<int>(parameters["custid"]);
                                custInfo.DisableLiveCasino = reader.To<bool>("DisableLiveCasino");
                                custInfo.Fax = reader.To<string>("fax");
                                custInfo.FirstName = reader.To<string>("firstname");
                                custInfo.IsSyncLiveCasino = reader.To<bool>("SyncLiveCasino");
                                custInfo.LastName = reader.To<string>("lastname");
                                custInfo.Mobile = reader.To<string>("mobilephone");
                                custInfo.NickName = reader.To<string>("username2");
                                custInfo.Phone = reader.To<string>("phone");
                                custInfo.RoleId = reader.To<byte>("roleid");
                                custInfo.Site = reader.To<string>("site");
                                custInfo.UplineDisableLiveCasino = reader.To<bool>("UplineDisableLiveCasino");
                                custInfo.UserName = reader.To<string>("username");
                                custInfo.MemberWinLimit = new MemberWinLimit
                                {
                                    IsAutoReset = reader.To<bool>("isAutoResetLiveCasino"),
                                    Max = reader.To<float>("MaxWinLiveCasino"),
                                    Upline = new MemberWinLimit
                                    {
                                        IsAutoReset = reader.To<bool>("UplineIsAutoResetLiveCasino"),
                                        Max = reader.To<float>("UplineMaxWinLiveCasino")
                                    }
                                };
                            }
                        }
                    }

                    return custInfo;
                }
                catch (Exception ex)
                {
                    CommonUtil.LogError(ex.Message);
                    throw ex;
                }
            }

            return new CustInfo();
        }

        /// <summary>
        /// Gets the current member win.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Current member win limit.</returns>
        public CurrentMemberWin GetCurrentMemberWin(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                var currentMemberWin = new CurrentMemberWin();

                try
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var parameters = criteria.GetSpParams();
                        using (var reader = objectDb.ExecuteReader(parameters))
                        {
                            if (reader.Read())
                            {
                                currentMemberWin.Balance = reader.To<float>("Balance");
                                currentMemberWin.MaxWinCredit = reader.To<float>("MaxWinCredit");
                                currentMemberWin.MaxWinLiveCasino = reader.To<float>("MaxWinLiveCasino");
                                currentMemberWin.WinPercent = reader.To<float>("LCSWinPercent");
                            }
                        }
                    }

                    return currentMemberWin;
                }
                catch (Exception ex)
                {
                    CommonUtil.LogError(ex.Message);

                    throw ex;
                }
            }

            return new CurrentMemberWin();
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
                        Value = reader.ToFloat("v162sdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162sdicount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        S4 = reader.To<float>("v162spositiontaking_1101"),
                        L4 = reader.To<float>("v162spositiontaking_1101_max"),
                        N4 = reader.To<float>("v162spositiontaking_1101_min")
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
                        Value = reader.ToFloat("v162discount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162discount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        S4 = reader.To<float>("v162spositiontaking_1101_max"),
                        N3 = reader.To<float>("v162mpositiontaking_1101_min"),
                        Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                        Us3 = reader.To<float>("auto_v162spostiontaking_1101"),
                        S3 = reader.To<float>("v162spositiontaking_1101"),
                        M3 = reader.To<float>("v162mpositiontaking_1101"),
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
                        Value = reader.ToFloat("v162discount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162discount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        IsDisableAuto = reader.To<bool>("DisableAutoMPT"),
                        M2 = reader.To<float>("v162mpositiontaking_1101"),
                        Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                        Um2 = reader.To<float>("auto_v162mpositiontaking_1101"),
                        A2 = reader.To<float>("v162apositiontaking_1101"),
                        M3 = reader.To<float>("v162mpositiontaking_1101_max"),
                        N2 = reader.To<float>("v162apositiontaking_1101_min"),
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
                        Value = reader.ToFloat("v162playerdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162playerdiscount_1101_max")
                        }
                    };

                    ptAndComSetting.PositionTaking = new PositionTaking
                    {
                        A1 = reader.To<float>("v162apositiontaking_1101"),
                        A2 = reader.To<float>("v162apositiontaking_1101_max"),
                        N2 = reader.To<float>("v162apositiontaking_1101_min"),
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
                        S1 = reader.To<float>("v162spositiontaking_1101"),
                        S1Max = reader.To<float>("v162spositiontaking_1101_max"),
                        S1Min = reader.To<float>("v162spositiontaking_1101_min"),
                        M1 = reader.To<float>("v162mpositiontaking_1101"),
                        A1 = reader.To<float>("v162apositiontaking_1101"),
                        Us1 = reader.To<float>("auto_v162spositiontaking_1101_mem"),
                        Is1 = reader.To<bool>("isauto_v162spositiontaking_1101_mem"),
                        Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                        S4 = reader.To<float>("auto_v162spositiontaking_1101_max")
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
                        M1 = reader.To<float>("v162mpositiontaking_1101"),
                        A1 = reader.To<float>("V162apositiontaking_1101"),
                        Um1 = reader.To<float>("auto_v162mpositiontaking_1101_mem"),
                        Im1 = reader.To<bool>("isauto_v162mpositiontaking_1101_mem"),
                        M1Max = reader.To<float>("v162mpositiontaking_1101_max"),
                        M1Min = reader.To<float>("v162mpositiontaking_1101_min"),
                        Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                        M3 = reader.To<float>("auto_v162mpositiontaking_1101_max")
                    };
                });
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="hasCheckedColumn">If set to <c>true</c> [has checked column].</param>
        /// <returns>List of game types.</returns>
        public List<GameType> GetGameSetting(ICriteria criteria, bool hasCheckedColumn)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var gameTypes = new List<GameType>();
                    GameType current = null;

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
                                    IsChecked = hasCheckedColumn ? reader.ToBool("IsChecked") : false
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

                    return gameTypes;
                }
                catch (Exception ex)
                {
                    CommonUtil.LogError(ex.Message);
                    throw ex;
                }
            }

            return new List<GameType>();
        }

        /// <summary>
        /// Inserts the game setting account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Game setting command result.</returns>
        public ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var result = new GameSettingCommandResult();
                    GameType current = null;
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
                                    Range = reader.ToString("StakeLevelName"),
                                    IsChecked = true
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

                    return result;
                }
                catch (Exception ex)
                {
                    CommonUtil.LogError(ex.Message);
                    throw ex;
                }
            }

            return new GameSettingCommandResult();
        }
    }
}