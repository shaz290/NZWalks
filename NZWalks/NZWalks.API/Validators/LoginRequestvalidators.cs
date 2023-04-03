using FluentValidation;

namespace NZWalks.API.Validators
{
    public class LoginRequestvalidators : AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginRequestvalidators()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();

        }
    }
}
