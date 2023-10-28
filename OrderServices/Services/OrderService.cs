using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderServices.Database;
using OrderServices.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OrderServices.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
       
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Order> Orders, int TotalItems, int TotalPages)> GetOrdersAsync(int page, int pageSize, string? orderNumber, string? email, string? phone)
        {
            IQueryable<Order> query = _context.Orders;

            if (!string.IsNullOrEmpty(orderNumber))
            {
                query = query.Where(o => o.OrderNumber.Contains(orderNumber));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(o => o.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(o => o.Phone.Contains(phone));
            }


            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var orders = await query.ToListAsync();

            return (orders, totalItems, totalPages);
        }


        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6)}";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

    }


}
