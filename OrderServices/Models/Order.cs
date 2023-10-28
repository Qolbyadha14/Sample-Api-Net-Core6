using System.ComponentModel.DataAnnotations;

namespace OrderServices.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(20)]
        public string OrderNumber { get; set; } = null!;

        public int Status { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? FirstName { get; set; }
        
        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(1)]
        public string? Gender { get; set; }
        public DateTime? Bod { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
