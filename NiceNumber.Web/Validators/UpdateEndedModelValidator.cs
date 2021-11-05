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
                    .WithMessage("Имя должно содержать максимум {MaxLength} символов.")
                .Matches("[a-zA-Zа-яА-Я0-9_-]{0,50}")
                    .WithMessage("Имя содержит недопустимые символы.");
            RuleFor(x => x.Link)
                .MaximumLength(100)
                    .WithMessage("Ссылка должна содержать максимум {MaxLength} символов.")
                .Matches("(https?:\\/\\/)?(www\\.)?[_\\-a-zA-Zа-яА-Я0-9\\.]{1,100}\\.[a-zA-Zа-яА-Я0-9]{1,6}([-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)")
                    .WithMessage("Неверный формат ссылки.");
        }
    }
}