namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Position Taking and Commission Repository.
    /// </summary>
    public interface IPTComRepository : IBaseRepository
    {
        PTComDTO Get(ICriteria criteria);
    }
}