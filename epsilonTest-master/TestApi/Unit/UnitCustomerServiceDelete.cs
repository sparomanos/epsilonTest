using Model;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestApi.Mocks;
using WebAPI.Services;
using Xunit;

namespace TestApi.Unit
{
    public class UnitCustomerServiceDelete
    {
        private readonly Mock<IDbContext<Customer>> _mockContext;
        private readonly CustomerService _customerService;

        public UnitCustomerServiceDelete()
        {
            _mockContext = new Mock<IDbContext<Customer>>();
            _customerService = new CustomerService(_mockContext.Object);
        }

        [Fact]
        public async Task Delete_ShouldDeleteCustomer_WhenIdExists()
        {
            //Arrange
            List<Customer> customers = MockData.GetCustomerMockData();
            Customer mockCustomer = customers.FirstOrDefault();
           
            _mockContext.Setup(x => x.DeleteByIdAsync(It.IsAny<string>()))
                 .Callback((string Id) =>
                 {
                     var remomveCustomer = customers.FirstOrDefault(x => x.Id == Id);
                     customers.Remove(remomveCustomer);
                 });

            //Act
            await _customerService.Delete(mockCustomer.Id);

            //Assert
            Assert.Equal(11, customers.Count());
        }

        [Fact]
        public async Task Delete_ShouldNotDeleteCustomer_WhenIdDoesNotExists()
        {
            //Arrange
            List<Customer> customers = MockData.GetCustomerMockData();

            Customer mockCustomer = customers.FirstOrDefault();
            
            _mockContext.Setup(x => x.DeleteByIdAsync(It.IsAny<string>()))
                 .Callback((string Id) =>
                 {
                     var remomveCustomer = customers.FirstOrDefault(x => x.Id == Id);
                     customers.Remove(remomveCustomer);
                 });

            //Act
            await _customerService.Delete(ObjectId.GenerateNewId().ToString());

            //Assert
            Assert.Equal(12, customers.Count());
        }
    }
}
