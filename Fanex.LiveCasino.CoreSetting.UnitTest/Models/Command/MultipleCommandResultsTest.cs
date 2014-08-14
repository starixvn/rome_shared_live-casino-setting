namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Command
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Data;
    using NUnit.Framework;

    /// <summary>
    /// Testing MultipleCommandResults class.
    /// </summary>
    [TestFixture]
    public class MultipleCommandResultsTest
    {
        [Test]
        public void CommandResults_CreateNew_ReturnNewResults()
        {
            // Arrange
            var command = CreateInstance();

            // Assert
            Assert.IsNotNull(command.CommandResultsList);
        }

        /// <summary>
        /// Determines whether [is successful_ results has one fail_ return false].
        /// </summary>
        [Test]
        public void IsSuccessful_ResultsHasOneFail_ReturnFalse()
        {
            // Arrange
            var command = CreateInstance();

            command.CommandResultsList = new List<ICommandResults>()
            {
                new CommandResults
                {
                    Results = new List<ICommandResult>
                    {
                        new CommandResult { Code = 1 },
                        new CommandResult { Code = 0 }
                    }
                },
                new CommandResults
                {
                    Results = new List<ICommandResult>
                    {
                        new CommandResult { Code = 0 },
                        new CommandResult { Code = 0 }
                    }
                }
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

            command.CommandResultsList = new List<ICommandResults>()
            {
                new CommandResults
                {
                    Results = new List<ICommandResult>
                    {
                        new CommandResult { Code = 0 },
                        new CommandResult { Code = 0 }
                    }
                },
                new CommandResults
                {
                    Results = new List<ICommandResult>
                    {
                        new CommandResult { Code = 0 },
                        new CommandResult { Code = 0 }
                    }
                }
            };

            // Assert
            Assert.IsTrue(command.IsSuccessful);
        }

        private MultipleCommandResults CreateInstance()
        {
            return new MultipleCommandResults();
        }
    }
}