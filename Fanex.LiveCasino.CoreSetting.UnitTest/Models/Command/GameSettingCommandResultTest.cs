namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Command
{
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using NUnit.Framework;

    /// <summary>
    /// Testing GameSettingCommandResult class.
    /// </summary>
    [TestFixture]
    public class GameSettingCommandResultTest
    {
        /// <summary>
        /// Games the setting command result_ command result_ return instance.
        /// </summary>
        [Test]
        public void GameSettingCommandResult_CommandResult_ReturnInstance()
        {
            // Arrange
            var command = new GameSettingCommandResult(new CommandResult
            {
                Code = 1,
                Message = string.Empty
            });
            command.Id = 0;

            // Assert
            Assert.IsFalse(command.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_CodeIsZero_ReturnTrue()
        {
            // Arrange
            var command = CreateInstance();

            command.Code = 0;

            // Assert
            Assert.IsTrue(command.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_CodeIsEmpty_ReturnTrue()
        {
            // Arrange
            var command = CreateInstance();

            command.Code = string.Empty;

            // Assert
            Assert.IsTrue(command.IsSuccessful);
        }

        private GameSettingCommandResult CreateInstance()
        {
            return new GameSettingCommandResult();
        }
    }
}