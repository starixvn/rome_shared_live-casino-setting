namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Data;

    public interface ISettingRepository : IBaseRepository
    {
        Customer GetCustomerInfo(ICriteria criteria);

        CurrentMemberWinDTO GetCurrentMemberWin(ICriteria criteria);

        PTComSettingDTO GetSettingForEditSuper(ICriteria criteria);

        PTComSettingDTO GetSettingForEditMaster(ICriteria criteria);

        PTComSettingDTO GetSettingForEditAgent(ICriteria criteria);

        PTComSettingDTO GetSettingForEditMember(ICriteria criteria);

        PTComSettingDTO GetSettingForSuperEditMember(ICriteria criteria);

        PTComSettingDTO GetSettingForMasterEditMember(ICriteria criteria);

        List<GameType> GetGameSetting(ICriteria criteria, bool hasCheckedColumn);

        ICommandResult InsertGameSettingAccount(ICriteria criteria, ref List<GameType> gameTypes);
    }
}