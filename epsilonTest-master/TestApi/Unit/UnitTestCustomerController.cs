using Common;
using Microsoft.AspNetCore.Mvc;
using Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Mocks;
using WebAPI.Controllers;
using WebAPI.Services;
using Xunit;

namespace TestApi.Unit
{
    public class UnitTestCustomerController
    {
        private readonly Mock<IDbContext<Customer>> _mockContext;
        private readonly CustomerService _customerService;
        private CustomerController _customerController;
        public UnitTestCustomerController()
        {
            _mockContext = new Mock<IDbContext<Customer>>();
            _customerService = new CustomerService(_mockContext.Object);

            _customerController = new CustomerController(_customerService);
        }


        [Fact]
        public void ActionResultGet_ShouldReturnEmptyList()
        {
            // Arrange
            List<Customer> customers = new List<Customer>();
            _mockContext.Setup(x => x.AsQueryable())
                .Returns(customers.AsQueryable());

            // Act
            var result =  _customerController.Get();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<Customer>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.NotNull(model.Value);
            Assert.Equal(200, model.StatusCode);
            Assert.Empty((model.Value as IEnumerable<Customer>));
        }

        [Fact]
        public void ActionResultGet_ShouldReturnListWithCustomers()
        {
            // Arrange
            List<Customer> customers = MockData.GetCustomerMockData();
            _mockContext.Setup(x => x.AsQueryable())
                .Returns(customers.AsQueryable());

            // Act
            var result = _customerController.Get();

            // Assert

            var viewResult = Assert.IsType<ActionResult<IEnumerable<Customer>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);
            
            Assert.NotNull(model.Value);
            Assert.Equal(200, model.StatusCode);
            Assert.Equal(12,(model.Value as IEnumerable<Customer>).Count());
        }

        [Fact]
        public void ActionResultGetItems_ShouldReturnPageObjectWithEmptyItems()
        {
            // Arrange
            List<Customer> customers = new List<Customer>();
            _mockContext.Setup(x => x.AsQueryable())
                .Returns(customers.AsQueryable());

            // Act
            var result = _customerController.GetItems(1);

            // Assert
            var viewResult = Assert.IsType<ActionResult<PaginationList<Customer>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.Equal(200, model.StatusCode);
            Assert.Empty((model.Value as PaginationList<Customer>).Items);
            Assert.Equal(1, (model.Value as PaginationList<Customer>).PageIndex);
        }

        [Fact]
        public void ActionResultGetItems_ShouldReturnSignlePageObjectWithFiveItems()
        {
            // Arrange
            List<Customer> customers = MockData.GetCustomerMockData();
            _mockContext.Setup(x => x.AsQueryable())
                .Returns(customers.AsQueryable());

            // Act
            var result = _customerController.GetItems(2);

            // Assert
            var viewResult = Assert.IsType<ActionResult<PaginationList<Customer>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.Equal(200, model.StatusCode);
            Assert.Equal(5,(model.Value as PaginationList<Customer>).Items.Count());
            Assert.Equal(2, (model.Value as PaginationList<Customer>).PageIndex);
        }

        [Fact]
        public async Task ActionResultGetOne_ShouldReturnOK_WithASignleCustomer()
        {
            // Arrange
            var mockCustomer =  MockData.GetOneCustomerMockData();
            _mockContext.Setup(x => x.FindByIdAsync(mockCustomer.Id)).ReturnsAsync(mockCustomer);
            // Act
            var result = await _customerController.Get(mockCustomer.Id);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.NotNull(model.Value);
            Assert.Equal(200,model.StatusCode);
        }

        [Fact]
        public async Task ActionResultGetOne_ShouldReturnNotFound()
        {
            // Arrange
            var mockCustomer = new Customer();
            _mockContext.Setup(x => x.FindByIdAsync("")).ReturnsAsync(mockCustomer);
            // Act
            var result = await _customerController.Get(mockCustomer.Id);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<NotFoundResult>(viewResult.Result as NotFoundResult);

            Assert.Equal(404,model.StatusCode);
        }

        [Fact]
        public async Task ActionResultPost_ShouldReturnCreateStatus_WithASignleCustomer()
        {
            // Arrange
            List<Customer> customerList = new List<Customer>();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.InsertOneAsync(It.IsAny<Customer>()))
               .Callback((Customer customer) =>
               {
                   customerList.Add(customer);
               });

            // Act
            var result = await _customerController.Post(mockCustomer);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<CreatedAtRouteResult>(viewResult.Result as CreatedAtRouteResult);

            Assert.NotNull(model.Value);
            Assert.Equal(201, model.StatusCode);
        }

        [Fact]
        public async Task ActionResultPost_ShouldReturnBadRequest()
        {
            // Arrange
            List<Customer> customerList = new List<Customer>();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.InsertOneAsync(It.IsAny<Customer>()))
               .Callback((Customer customer) =>
               {
                   customerList.Add(customer);
               });

            // Act
            var result = await _customerController.Post(null);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<BadRequestResult>(viewResult.Result as BadRequestResult);

            Assert.NotNull(model);
            Assert.Equal(400, model.StatusCode);
        }

        [Fact]
        public async Task ActionResultPut_ShouldReturOKStatus_WithUpdatedCustomer()
        {
            // Arrange
            List<Customer> customerList = new List<Customer>();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.FindByIdAsync(mockCustomer.Id)).ReturnsAsync(mockCustomer);
            _mockContext.Setup(x => x.ReplaceOne(It.IsAny<Customer>()))
               .Callback((Customer customer) =>
               {
                   customerList.Add(customer);
               });

            // Act
            var result = await _customerController.Put(mockCustomer);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.NotNull(model.Value);
            Assert.Equal(200, model.StatusCode);
        }

        [Fact]
        public async Task ActionResultPut_ShouldReturnStatusNotFound()
        {
            // Arrange
            List<Customer> customerList = new List<Customer>();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.ReplaceOne(It.IsAny<Customer>()))
               .Callback((Customer customer) =>
               {
                   customerList.Add(customer);
               });

            // Act
            var result = await _customerController.Put(mockCustomer);

            // Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<NotFoundResult>(viewResult.Result as NotFoundResult);

            Assert.Equal(404, model.StatusCode);
        }

        [Fact]
        public async Task ActionResultDelete_ShouldReturOKStatus_WhenRecordDeletedSuccessfull()
        {
            // Arrange
            List<Customer> customers = MockData.GetCustomerMockData();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.FindByIdAsync(mockCustomer.Id)).ReturnsAsync(mockCustomer);

            _mockContext.Setup(x => x.DeleteByIdAsync(It.IsAny<string>()))
                 .Callback((string Id) =>
                 {
                     var remomveCustomer = customers.FirstOrDefault(x => x.Id == Id);
                     customers.Remove(remomveCustomer);
                 });

            // Act
            var result = await _customerController.Delete(mockCustomer.Id);

            // Assert
            var viewResult = Assert.IsType<OkResult>(result);
            
            Assert.NotNull(viewResult);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task ActionResultDelete_ShouldReturStatusNotFound_WhenRecordNotFound()
        {
            // Arrange
            List<Customer> customers = MockData.GetCustomerMockData();
            Customer mockCustomer = MockData.GetOneCustomerMockData();

            _mockContext.Setup(x => x.DeleteByIdAsync(It.IsAny<string>()))
                 .Callback((string Id) =>
                 {
                     var remomveCustomer = customers.FirstOrDefault(x => x.Id == Id);
                     customers.Remove(remomveCustomer);
                 });

            // Act
            var result = await _customerController.Delete(mockCustomer.Id);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.NotNull(viewResult);
            Assert.Equal(404, viewResult.StatusCode);
        }
    }
}
