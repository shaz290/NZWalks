using FluentValidation;

namespace NZWalks.API.Validators
{
    public class ValidateAddWalkRequestValidator:AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public ValidateAddWalkRequestValidator()
        {
            RuleFor(x =>x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
