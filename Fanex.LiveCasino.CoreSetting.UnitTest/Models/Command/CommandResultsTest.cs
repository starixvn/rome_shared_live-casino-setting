namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Command
{
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// Testing CommandResults class.
    /// </summary>
    [TestFixture]
    public class CommandResultsTest
    {
        private CommandResults CreateInstance()
        {
            return Substitute.For<CommandResults>();
        }
    }
}