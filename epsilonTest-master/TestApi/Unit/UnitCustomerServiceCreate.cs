using Model;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Mocks;
using WebAPI.Services;
using Xunit;

namespace TestApi.Unit
{
    public class UnitCustomerServiceCreate
    {
        private readonly Mock<IDbContext<Customer>> _mockContext;
        private readonly CustomerService _customerService;

        public UnitCustomerServiceCreate()
        {
            _mockContext = new Mock<IDbContext<Customer>>();
            _customerService = new CustomerService(_mockContext.Object);
        }

        [Fact]
        public async Task Create_ShouldSaveNewCustomer_WhenCustomerIsAnyCustomerObject()
        {
            //Arrange
            List<Customer> customerList = new List<Customer>();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.InsertOneAsync(It.IsAny<Customer>()))
           .Callback((Customer customer) =>
           {
               customerList.Add(customer);
           });

            //Act
            await _customerService.Create(mockCustomer);

            //Assert
            Assert.Single(customerList);
            Assert.Equal("Company Test", mockCustomer.CompanyName);
        }

        [Fact]
        public async Task Create_ShouldNotSavedNewCustomer_WhenCustomerIsNull()
        {
            //Arrange
            List<Customer> customerList = new List<Customer>();
            IQueryable<Customer> customers = new List<Customer>().AsQueryable();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.InsertOneAsync(It.IsAny<Customer>()))
           .Callback((Customer customer) =>
           {
               if(customer!=null)
               customerList.Add(customer);
           });

            //Act
            await _customerService.Create(null);

            //Assert
            Assert.Empty(customerList);
        }
    }
}
