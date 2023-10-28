using System.ComponentModel.DataAnnotations;

namespace OrderServices.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameProduct { get; set; } = null!;

        [Required]
        public long Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }

}
