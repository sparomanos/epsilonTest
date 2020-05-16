using DnsClient.Protocol;
using Interfaces;
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
    public class UnitCustomerServiceGet
    {
        private readonly Mock<IDbContext<Customer>> _mockContext;
        private readonly CustomerService _customerService;

        public UnitCustomerServiceGet()
        {
            _mockContext = new Mock<IDbContext<Customer>>();
            _customerService = new CustomerService(_mockContext.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnCustomer_WhenCustomerExists()
        {
            //Arrange
            string Id= ObjectId.GenerateNewId().ToString();
            Customer mockCustomer = MockData.GetOneCustomerMockData();
            mockCustomer.Id = Id;

            _mockContext.Setup(x => x.FindByIdAsync(Id)).ReturnsAsync(mockCustomer);
            
            //Act
            var customer = await _customerService.Get(Id);
            
            //Assert
            Assert.Equal(Id, customer.Id);
        }

        [Fact]
        public async Task Get_ShouldReturnNull_WhenCustomerDoesNotExists()
        {
            //Arrange
            string Id = It.IsAny<string>();
            _mockContext.Setup(x => x.FindByIdAsync(Id)).ReturnsAsync(()=>null);
            
            //Act
            var customer = await _customerService.Get(Id);

            //Assert
            Assert.Null(customer);
        }
    }
}
