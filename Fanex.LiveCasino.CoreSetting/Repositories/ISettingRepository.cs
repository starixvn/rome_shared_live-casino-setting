namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    public interface ISettingRepository : IBaseRepository
    {
        UplineCustomerInfoDTO GetUplineCustomerInfo(ICriteria criteria);

        Customer GetCustomerInfo(ICriteria criteria);

        CurrentMemberWinLimitDTO GetCurrentMemberWin(ICriteria criteria);

        PTComDTO GetSettingForEditSuper(ICriteria criteria);

        PTComDTO GetSettingForEditMaster(ICriteria criteria);

        PTComDTO GetSettingForEditAgent(ICriteria criteria);

        PTComDTO GetSettingForEditMember(ICriteria criteria);

        PTComDTO GetSettingForSuperEditMember(ICriteria criteria);

        PTComDTO GetSettingForMasterEditMember(ICriteria criteria);

        List<GameType> GetGameSetting(int custId);

        List<GameType> GetGameSetting(int exchangeId, string site, bool defaultCheckedValue = false);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, List<GameType> gameTypes);
    }
}