namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPTComRepository : IBaseRepository
    {
        PTComDTO Get(ICriteria criteria);
    }
}