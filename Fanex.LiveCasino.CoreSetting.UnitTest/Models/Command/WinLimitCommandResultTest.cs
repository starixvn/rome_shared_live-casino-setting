namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Command
{
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using NUnit.Framework;

    /// <summary>
    /// Testing WinLimitCommandResult class.
    /// </summary>
    [TestFixture]
    public class WinLimitCommandResultTest
    {
        /// <summary>
        /// Wins the limit command result_ command result_ return instance.
        /// </summary>
        [Test]
        public void WinLimitCommandResult_CommandResult_ReturnInstance()
        {
            // Arrange
            var command = new WinLimitCommandResult(new CommandResult
            {
                Code = 1,
                Message = string.Empty
            });

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

        private WinLimitCommandResult CreateInstance()
        {
            return new WinLimitCommandResult();
        }
    }
}