using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderServices.Database;
using OrderServices.DataTransferObject;
using OrderServices.Models;
using OrderServices.Services;
using OrderServices.Validation;
using OrderServices.ViewModel;

namespace OrderServices.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<CreateOrderDTO> _orderValidator;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IValidator<CreateOrderDTO> orderValidator, IMapper mapper)
        {
            _orderService = orderService;
            _orderValidator = orderValidator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? orderNumber = null, [FromQuery] string? email = null, [FromQuery] string? phone = null)
        {
            var (orders, totalItems, totalPages) = await _orderService.GetOrdersAsync(page, pageSize, orderNumber, email, phone);

            var paginationMetadata = new
            {
                totalItems,
                totalPages,
                currentPage = page,
                pageSize
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);

            if (order == null)
                return NotFound($"Order with id {id} not found");

            var response = _mapper.Map<OrderViewModel>(order);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO order)
        {
            var validationResult = _orderValidator.Validate(order);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var order_mapper= _mapper.Map<Order>(order);
            var createdOrder = await _orderService.CreateOrderAsync(order_mapper);
            var response = _mapper.Map<OrderViewModel>(createdOrder);


            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, response);
        }
    }

}
