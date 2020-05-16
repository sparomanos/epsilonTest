using Model;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApi.Mocks;
using WebAPI.Services;
using Xunit;

namespace TestApi.Unit
{
    public class UnitCustomerServiceUpdate
    {
        private readonly Mock<IDbContext<Customer>> _mockContext;
        private readonly CustomerService _customerService;

        public UnitCustomerServiceUpdate()
        {
            _mockContext = new Mock<IDbContext<Customer>>();
            _customerService = new CustomerService(_mockContext.Object);
        }

        [Fact]
        public async Task Update_ShouldUpdateCustomer_WhenCustomerExists()
        {
            //Arrange
            List<Customer> customerList = new List<Customer>();
            
            Customer mockCustomer = MockData.GetOneCustomerMockData();
            mockCustomer.CompanyName = "Updated Company";

            _mockContext.Setup(x => x.ReplaceOneAsync(It.IsAny<Customer>()))
            .Callback((Customer customer) =>
            {
                customerList.Add(customer);
            });
            _mockContext.Setup(x => x.FindByIdAsync(mockCustomer.Id)).ReturnsAsync(mockCustomer);

            //Act
            await _customerService.Update(mockCustomer);
            
            Customer customer = await _customerService.Get(customerList[0].Id);

            //Assert
            Assert.Single(customerList);
            Assert.Equal(customer.CompanyName, mockCustomer.CompanyName);
        }

    }
}
