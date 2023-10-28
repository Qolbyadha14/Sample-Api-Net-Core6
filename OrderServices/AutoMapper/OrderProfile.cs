using AutoMapper;
using OrderServices.DataTransferObject;
using OrderServices.Models;
using OrderServices.ViewModel;

namespace OrderServices.AutoMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<CreateOrderDTO, Order>();
            CreateMap<CreateOrderDetailDTO, OrderDetail>();

            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderDetail, OrderDetailViewModel>();
        }
    }
}
