using Model;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TestApi.Mocks;
using WebAPI.Services;
using Xunit;

namespace TestApi.Unit
{
    public class UnitCustomerServiceGetItems
    {
        private readonly Mock<IDbContext<Customer>> _mockContext;
        private readonly CustomerService _customerService;

        public UnitCustomerServiceGetItems()
        {
            _mockContext = new Mock<IDbContext<Customer>>();
            _customerService = new CustomerService(_mockContext.Object);
        }

        [Fact]
        public void GetItems_ShouldReturnPagginationObjectWithCustomerList_WhenTypePage()
        {
            //Arrange
            IQueryable<Customer> customers = MockData.GetCustomerMockData().AsQueryable();
            _mockContext.Setup(x => x.AsQueryable()).Returns(customers);

            //Act
            var fistPage = _customerService.GetItems(1);

            //Assert
            Assert.Equal(5, fistPage.Items.Count);
        }

        [Fact]
        public void GetItems_ShouldReturnObjectWithoutItems_WhenPageIndexIsOutOfRage()
        {
            //Arrange
            IQueryable<Customer> customers = MockData.GetCustomerMockData().AsQueryable();
            _mockContext.Setup(x => x.AsQueryable()).Returns(customers);

            //Act
            var outOfRangePage = _customerService.GetItems(4);

            //Assert
            Assert.Empty(outOfRangePage.Items);
        }
        [Fact]
        public void GetItems_ShouldReturnObjectWithoutItem_WhenSourceIsEmpty()
        {
            //Arrange
            IQueryable<Customer> customers = new List<Customer>().AsQueryable();
            _mockContext.Setup(x => x.AsQueryable()).Returns(customers);

            //Act
            var outOfRangePage = _customerService.GetItems(4);

            //Assert
            Assert.Empty(outOfRangePage.Items);
        }

        [Fact]
        public void GetItems_ShouldReturnObjectWithTwoItems_WhenSourceHavesOnlyTwoRecordsAndPageIsOne()
        {
            //Arrange
            IQueryable<Customer> customers = MockData.GetCustomerMockData().Take(2).AsQueryable();
            _mockContext.Setup(x => x.AsQueryable()).Returns(customers);

            //Act
            var outOfRangePage = _customerService.GetItems(1);

            //Assert
            Assert.Equal(2, outOfRangePage.Items.Count);
        }
    }
}
