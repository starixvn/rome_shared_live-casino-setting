namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Command
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;
    using NUnit.Framework;

    /// <summary>
    /// Testing CommandResults class.
    /// </summary>
    [TestFixture]
    public class CommandResultsTest
    {
        [Test]
        public void CommandResults_CreateNew_ReturnNewResults()
        {
            // Arrange
            var command = CreateInstance();

            command.Id = 0;
            command.Name = string.Empty;

            // Assert
            Assert.IsNotNull(command.Results);
        }

        /// <summary>
        /// Determines whether [is successful_ results has one fail_ return false].
        /// </summary>
        [Test]
        public void IsSuccessful_ResultsHasOneFail_ReturnFalse()
        {
            // Arrange
            var command = CreateInstance();

            command.Results = new List<ICommandResult>()
            {
                new CommandResult { Code = 1 },
                new CommandResult { Code = 0 }
            };

            // Assert
            Assert.IsFalse(command.IsSuccessful);
        }

        /// <summary>
        /// Determines whether [is successful_ all results success_ return true].
        /// </summary>
        [Test]
        public void IsSuccessful_AllResultsSuccess_ReturnTrue()
        {
            // Arrange
            var command = CreateInstance();

            command.Results = new List<ICommandResult>()
            {
                new CommandResult { Code = 0 },
                new CommandResult { Code = 0 }
            };

            // Assert
            Assert.IsTrue(command.IsSuccessful);
        }

        private CommandResults CreateInstance()
        {
            return new CommandResults();
        }
    }
}