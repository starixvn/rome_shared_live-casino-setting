namespace Fanex.LiveCasino.CoreSetting.UnitTest.Models.Command
{
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.NAP.Framework.Command;
    using NUnit.Framework;

    /// <summary>
    /// Testing PT And ComCommandResult class.
    /// </summary>
    [TestFixture]
    public class PtAndComCommandResultTest
    {
        /// <summary>
        /// PT the and COM command result_ command result_ return instance.
        /// </summary>
        [Test]
        public void PtAndComCommandResult_CommandResult_ReturnInstance()
        {
            // Arrange
            var command = new PtAndComCommandResult(new CommandResult
            {
                Code = 1,
                Message = string.Empty
            });

            // Assert
            Assert.IsFalse(command.IsSuccessful);
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

        [Test]
        public void IsSuccessful_CodeIsNull_ReturnTrue()
        {
            // Arrange
            var command = CreateInstance();

            command.Code = null;

            // Assert
            Assert.IsTrue(command.IsSuccessful);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void IsSuccessful_ErrorCode_ReturnFalse(int errorCode)
        {
            // Arrange
            var command = CreateInstance();

            command.Code = errorCode;

            // Assert
            Assert.IsFalse(command.IsSuccessful);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsSuccessful_ErrorCode_ReturnTrue(int errorCode)
        {
            // Arrange
            var command = CreateInstance();

            command.Code = errorCode;

            // Assert
            Assert.IsTrue(command.IsSuccessful);
        }

        private PtAndComCommandResult CreateInstance()
        {
            return new PtAndComCommandResult();
        }
    }
}