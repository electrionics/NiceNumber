using FluentValidation;
using NiceNumber.Web.ViewModels;

namespace NiceNumber.Web.Validators
{
    public class UpdateEndedModelValidator:AbstractValidator<UpdateEndedModel>
    {
        public UpdateEndedModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Имя обязательно для заполнения.")
                .MaximumLength(50)
                    .WithMessage("Имя должно содержать максимум {MaxLength} символов.");
            RuleFor(x => x.Link)
                .MaximumLength(100)
                    .WithMessage("Ссылка должна содержать максимум {MaxLength} символов.");
        }
    }
}