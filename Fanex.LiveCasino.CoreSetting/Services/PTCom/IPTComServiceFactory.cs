namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPTComServiceFactory
    {
        IPTComService GetService(LiveCasinoSettingContext settingContext);
    }
}
