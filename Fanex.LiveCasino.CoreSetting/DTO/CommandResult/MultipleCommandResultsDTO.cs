namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Multiple Command Results.
    /// </summary>
    public class MultipleCommandResultsDTO : IMultipleCommandResultsDTO
    {
        public MultipleCommandResultsDTO()
        {
            CommandResultsList = new List<ICommandResultsDTO>();
        }

        public List<ICommandResultsDTO> CommandResultsList { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return CommandResultsList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}