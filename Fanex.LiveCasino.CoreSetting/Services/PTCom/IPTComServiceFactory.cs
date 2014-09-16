namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Factory for generating PT COM service for updating.
    /// </summary>
    public interface IPTComServiceFactory
    {
        IPTComService GetService(LiveCasinoSettingContext settingContext);
    }
}