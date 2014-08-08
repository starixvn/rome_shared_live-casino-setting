namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using NUnit.Framework;

    /// <summary>
    /// Testing the ResultCriteria class.
    /// </summary>
    [TestFixture]
    public class ResultCriteriaTest
    {
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        [Test]
        public void GetSpParams_WhenCalled_ReturnParameters()
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            var expertedCount = 2;
            Assert.AreEqual(expertedCount, parameters.Count);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase("errcode")]
        [TestCase("errmsg")]
        public void GetSpParams_WhenCalled_ContainParameters(string parameter)
        {
            // Arrange
            var criteria = CreateNewCriteria();

            // Action
            var parameters = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(parameters.ContainsKey(parameter));
        }

        private ResultCriteria CreateNewCriteria()
        {
            return new ResultCriteria();
        }
    }
}
