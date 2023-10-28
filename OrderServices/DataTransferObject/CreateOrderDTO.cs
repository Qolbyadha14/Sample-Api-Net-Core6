using OrderServices.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderServices.DataTransferObject
{
    public class CreateOrderDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? Bod { get; set; }

        public List<CreateOrderDetailDTO> OrderDetails { get; set; } = null!;
    }

    public class CreateOrderDetailDTO
    {
        public long Quantity { get; set; }
        public string NameProduct { get; set; } = null!;
        public long Price { get; set; }
    }
}
