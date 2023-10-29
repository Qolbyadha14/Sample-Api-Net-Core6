using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderServices.Controllers.v1;
using OrderServices.DataTransferObject;
using OrderServices.Models;
using OrderServices.Services;
using OrderServices.ViewModel;
using Xunit;
using Newtonsoft.Json;

namespace OrderServicesTest.Controllers.v1
{
    public class OrdersControllerTests
    {
        private Mock<IOrderService> _orderServiceMock;
        private Mock<IValidator<CreateOrderDTO>> _orderValidatorMock;
        private Mock<IMapper> _mapperMock;
        private OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderValidatorMock = new Mock<IValidator<CreateOrderDTO>>();
            _mapperMock = new Mock<IMapper>();

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Headers.Add("some-key", "some-value");

            _controller = new OrdersController(_orderServiceMock.Object, _orderValidatorMock.Object, _mapperMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }

        [Fact]
        public async Task GetOrders_ReturnsOrders()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var orderNumber = "12345";
            var email = "test@example.com";
            var phone = "1234567890";

            var orders = new List<Order>
            {
                new Order { Id = 1, OrderNumber = "12345", Email = "test@example.com", Phone = "1234567890" }
            };

            var paginationMetadata = new
            {
                totalItems = 1,
                totalPages = 1,
                currentPage = page,
                pageSize
            };

            _orderServiceMock.Setup(x => x.GetOrdersAsync(page, pageSize, orderNumber, email, phone))
                .ReturnsAsync((orders, 1, 1));
            // Act
            var result = await _controller.GetOrders(page, pageSize, orderNumber, email, phone);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<List<Order>>(okResult.Value);

            // Additional checks to handle possible null references
            Assert.NotNull(_controller.HttpContext);
            Assert.NotNull(_controller.HttpContext.Response);
            Assert.NotNull(_controller.HttpContext.Response.Headers);

            Assert.Contains("X-Pagination", _controller.HttpContext.Response.Headers.Keys);

            var paginationHeader = _controller.HttpContext.Response.Headers["X-Pagination"];
            Assert.Single(paginationHeader);

            var paginationHeaderValue = paginationHeader[0];
            var deserializedPaginationMetadata = JsonConvert.DeserializeObject<dynamic>(paginationHeaderValue);
            Assert.Equal(paginationMetadata.totalItems, (int)deserializedPaginationMetadata.totalItems);
            Assert.Equal(paginationMetadata.totalPages, (int)deserializedPaginationMetadata.totalPages);
            Assert.Equal(paginationMetadata.currentPage, (int)deserializedPaginationMetadata.currentPage);
            Assert.Equal(paginationMetadata.pageSize, (int)deserializedPaginationMetadata.pageSize);
        }

        [Fact]
        public async Task GetOrder_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new Order { Id = orderId, /* other properties */ };

            _orderServiceMock.Setup(x => x.GetOrderAsync(orderId))
                .ReturnsAsync(existingOrder);

            _mapperMock.Setup(x => x.Map<OrderViewModel>(existingOrder))
                .Returns(new OrderViewModel { Id = orderId, /* map other properties */ });

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<OrderViewModel>(okResult.Value);
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;

            _orderServiceMock.Setup(x => x.GetOrderAsync(orderId))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"Order with id {orderId} not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtAction_WhenOrderCreationIsSuccessful()
        {
            // Arrange
            var createOrderDTO = new CreateOrderDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                Gender = "M",
                Bod = DateTime.Parse("1990-01-01"),
                OrderDetails = new List<CreateOrderDetailDTO>
                {
                    new CreateOrderDetailDTO { Quantity = 1, NameProduct = "Product1", Price = 10 },
                    new CreateOrderDetailDTO { Quantity = 2, NameProduct = "Product2", Price = 20 },
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _orderValidatorMock.Setup(x => x.Validate(createOrderDTO))
                .Returns(validationResult);

            var orderMapper = new Order
            {
                Id = 1,
                OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 20), // Generate a unique order number
                Status = 1, // Set the status as needed
                CreatedAt = DateTime.Now,
                FirstName = createOrderDTO.FirstName,
                LastName = createOrderDTO.LastName,
                Email = createOrderDTO.Email,
                Phone = createOrderDTO.Phone,
                Gender = createOrderDTO.Gender,
                Bod = createOrderDTO.Bod,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { Quantity = 1, NameProduct = "Product1", Price = 10 },
                    new OrderDetail { Quantity = 2, NameProduct = "Product2", Price = 20 },
                }
            };

            _mapperMock.Setup(x => x.Map<Order>(createOrderDTO))
                .Returns(orderMapper);

            var createdOrder = new Order
            {
                Id = orderMapper.Id,
                OrderNumber = orderMapper.OrderNumber,
                Status = orderMapper.Status,
                CreatedAt = orderMapper.CreatedAt,
                FirstName = orderMapper.FirstName,
                LastName = orderMapper.LastName,
                Email = orderMapper.Email,
                Phone = orderMapper.Phone,
                Gender = orderMapper.Gender,
                Bod = orderMapper.Bod,
                OrderDetails = new List<OrderDetail>(orderMapper.OrderDetails),
            };

            _orderServiceMock.Setup(x => x.CreateOrderAsync(orderMapper))
                .ReturnsAsync(createdOrder);

            var orderViewModel = new OrderViewModel
            {
                Id = createdOrder.Id,
                OrderNumber = createdOrder.OrderNumber,
                Status = createdOrder.Status,
                CreatedAt = createdOrder.CreatedAt,
                FirstName = createdOrder.FirstName,
                LastName = createdOrder.LastName,
                Email = createdOrder.Email,
                Phone = createdOrder.Phone,
                Gender = createdOrder.Gender,
                Bod = createdOrder.Bod,
                OrderDetails = _mapperMock.Object.Map<ICollection<OrderDetailViewModel>>(createdOrder.OrderDetails),
            };

            _mapperMock.Setup(x => x.Map<OrderViewModel>(createdOrder))
                .Returns(orderViewModel);

            // Act
            var result = await _controller.CreateOrder(createOrderDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            var createdAtActionResult = (CreatedAtActionResult)result;

            Assert.Equal(nameof(OrdersController.GetOrder), createdAtActionResult.ActionName);
            Assert.Equal(createdOrder.Id, createdAtActionResult.RouteValues["id"]);
            Assert.IsType<OrderViewModel>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task CreateOrder_ReturnsBadRequest_WhenOrderValidationFails()
        {
            // Arrange
            var createOrderDTO = new CreateOrderDTO
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                Gender = "M",
                Bod = DateTime.Parse("1990-01-01"),
                OrderDetails = new List<CreateOrderDetailDTO>
                {
                    new CreateOrderDetailDTO { Quantity = 1, NameProduct = "Product1", Price = 10 },
                    new CreateOrderDetailDTO { Quantity = 2, NameProduct = "Product2", Price = 20 },
                }
            };

            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "The FirstName field is required."),
            };

            var validationResult = new FluentValidation.Results.ValidationResult(validationErrors);
            _orderValidatorMock.Setup(x => x.Validate(createOrderDTO))
                .Returns(validationResult);

            // Act
            var result = await _controller.CreateOrder(createOrderDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            Assert.Equal(validationErrors, badRequestResult.Value);
        }

    }

}
