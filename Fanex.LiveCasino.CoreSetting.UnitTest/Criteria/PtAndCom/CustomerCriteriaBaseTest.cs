namespace Fanex.LiveCasino.CoreSetting.UnitTest.Criteria.PtAndCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using NUnit.Framework;

    /// <summary>
    /// Customer criteria base test.
    /// </summary>
    [TestFixture]
    public class CustomerCriteriaBaseTest
    {
        /// <summary>
        /// Determines whether is valid criteria [customer identifier is zero return false].
        /// </summary>
        [Test]
        public void IsValid_CustIdIsZero_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = 0;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [customer identifier is negative return false].
        /// </summary>
        [Test]
        public void IsValid_CustIdIsNegative_ReturnFalse()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = -1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsFalse(actualValue);
        }

        /// <summary>
        /// Determines whether is valid criteria [customer identifier is positive return true].
        /// </summary>
        [Test]
        public void IsValid_CustIdIsPositive_ReturnTrue()
        {
            // Arrange
            var criteria = CreateNewCriteria();
            criteria.CustId = 1;

            // Action
            var actualValue = criteria.IsValid();

            // Assert
            Assert.IsTrue(actualValue);
        }

        private CustomerCriteriaBase CreateNewCriteria()
        {
            return new CustomerCriteriaBase();
        }
    }
}