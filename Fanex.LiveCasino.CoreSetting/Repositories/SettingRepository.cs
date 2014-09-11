namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.Logging;
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
        public Customer GetCustomerInfo(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var custInfo = new Customer();
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
                                custInfo.MemberWinLimit = new WinLimit
                                {
                                    IsAutoReset = reader.To<bool>("isAutoResetLiveCasino"),
                                    Max = reader.To<float>("MaxWinLiveCasino"),
                                    Upline = new WinLimit
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
                    Logger.Log.Error(ex.Message, LiveCasinoSettings.Settings.LogCategory);

                    throw ex;
                }
            }

            return new Customer();
        }

        /// <summary>
        /// Gets the current member win.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Current member win limit.</returns>
        public CurrentMemberWinDTO GetCurrentMemberWin(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                var currentMemberWin = new CurrentMemberWinDTO();

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
                    Logger.Log.Error(ex.Message);

                    throw ex;
                }
            }

            return new CurrentMemberWinDTO();
        }

        /// <summary>
        /// Gets the setting for edit super.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and Commission model.</returns>
        public object[] GetSettingForEditSuper(ICriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting[0] = new PositionTaking
                    {
                        S4 = reader.To<float>("v162spositiontaking_1101"),
                        L4 = reader.To<float>("v162spositiontaking_1101_max"),
                        N4 = reader.To<float>("v162spositiontaking_1101_min")
                    };

                    ptAndComSetting[1] = new Commission
                    {
                        Value = reader.ToFloat("v162sdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162sdicount_1101_max")
                        }
                    };
                });
        }

        /// <summary>
        /// Gets the setting for edit master.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and Commission model.</returns>
        public object[] GetSettingForEditMaster(ICriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting[0] = new PositionTaking
                    {
                        S4 = reader.To<float>("v162spositiontaking_1101_max"),
                        N3 = reader.To<float>("v162mpositiontaking_1101_min"),
                        Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                        Us3 = reader.To<float>("auto_v162spostiontaking_1101"),
                        S3 = reader.To<float>("v162spositiontaking_1101"),
                        M3 = reader.To<float>("v162mpositiontaking_1101")
                    };

                    ptAndComSetting[1] = new Commission
                    {
                        Value = reader.ToFloat("v162discount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162discount_1101_max")
                        }
                    };
                });
        }

        /// <summary>
        /// Gets the setting for edit agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and Commission model.</returns>
        public object[] GetSettingForEditAgent(ICriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting[0] = new PositionTaking
                    {
                        IsDisableAuto = reader.To<bool>("DisableAutoMPT"),
                        M2 = reader.To<float>("v162mpositiontaking_1101"),
                        Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                        Um2 = reader.To<float>("auto_v162mpositiontaking_1101"),
                        A2 = reader.To<float>("v162apositiontaking_1101"),
                        M3 = reader.To<float>("v162mpositiontaking_1101_max"),
                        N2 = reader.To<float>("v162apositiontaking_1101_min")
                    };

                    ptAndComSetting[1] = new Commission
                    {
                        Value = reader.ToFloat("v162discount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162discount_1101_max")
                        }
                    };
                });
        }

        /// <summary>
        /// Gets the setting for edit member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and Commission model.</returns>
        public object[] GetSettingForEditMember(ICriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting[0] = new PositionTaking
                    {
                        A1 = reader.To<float>("v162apositiontaking_1101"),
                        A2 = reader.To<float>("v162apositiontaking_1101_max"),
                        N2 = reader.To<float>("v162apositiontaking_1101_min"),
                    };

                    ptAndComSetting[1] = new Commission
                    {
                        Value = reader.ToFloat("v162playerdiscount_1101"),
                        Upline = new Commission
                        {
                            Value = reader.ToFloat("v162playerdiscount_1101_max")
                        }
                    };
                });
        }

        /// <summary>
        /// Gets the setting for super edit member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Position taking and Commission model.</returns>
        public object[] GetSettingForSuperEditMember(ICriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting[0] = new PositionTaking
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
        /// <returns>Position taking and Commission model.</returns>
        public object[] GetSettingForMasterEditMember(ICriteria criteria)
        {
            return GetCommonSettingForEdit(
                criteria,
                (reader, ptAndComSetting) =>
                {
                    ptAndComSetting[0] = new PositionTaking
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
                    Logger.Log.Error(ex.Message);
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var result = new GameSettingCommandResultDTO();
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
                        result = new GameSettingCommandResultDTO(GetCommandResult(outParameters));
                    }

                    if (!result.IsSuccessful)
                    {
                        Logger.Log.Info(string.Format("{0}\n{1}", result.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);
                    throw ex;
                }
            }

            return new GameSettingCommandResultDTO();
        }
    }
}