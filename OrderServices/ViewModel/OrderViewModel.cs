namespace OrderServices.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? Bod { get; set; }

        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public long Quantity { get; set; }
        public string NameProduct { get; set; }
        public long Price { get; set; }
    }
}
