using FluentValidation;

namespace ClientSideFluentValidation.Models
{
    public class BasicPersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        public int LengthOfService { get; set; }

        public int ContractLength { get; set; }

        public bool AdditionalField { get; set; }

        public string Nationality { get; set; }
    }

    public class BasicPersonViewModelValidator : AbstractValidator<BasicPersonViewModel>
    {
        public BasicPersonViewModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Age).InclusiveBetween(18, 60);
            RuleFor(x => x.LengthOfService).GreaterThan(x => x.Age);
            RuleFor(x => x.ContractLength).LessThan(x => x.LengthOfService);
            RuleFor(x => x.Nationality).NotEmpty().When(x => x.AdditionalField == true);
        }
    }
}