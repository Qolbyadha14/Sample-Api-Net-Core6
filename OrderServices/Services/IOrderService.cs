using OrderServices.Models;

namespace OrderServices.Services
{
    public interface IOrderService
    {
        Task<(IEnumerable<Order> Orders, int TotalItems, int TotalPages)> GetOrdersAsync(int page, int pageSize, string? orderNumber, string? email, string? phone);
        Task<Order> GetOrderAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
    }
}
