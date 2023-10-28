using FluentValidation;
using OrderServices.DataTransferObject;
using OrderServices.Models;

namespace OrderServices.Validation
{
    public class OrderValidator : AbstractValidator<CreateOrderDTO>
    {
        public OrderValidator() 
        { 
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50); 
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
            RuleFor(order => order.Gender).NotEmpty().Length(1).Must(BeValidGender).WithMessage("Invalid gender. Use 'L', 'P', or 'O'");
            RuleFor(order => order.Bod).NotNull();
            RuleFor(order => order.OrderDetails).NotEmpty();
        }

        private bool BeValidGender(string gender)
        {
            return gender == "F" || gender == "M" || gender == "O";
        }
    }
}
