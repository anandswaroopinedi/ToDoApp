using Common;
using FluentValidation;
using Models.DtoModels;
using System.Text.Json;

namespace ToDoAppWebApi.Validations
{
    public class ItemValidator:AbstractValidator<ItemDto>
    {
        public ItemValidator() 
        {
            RuleFor(item => item.Name)
           .NotNull().WithMessage("Name should not be null.")
           .Must(name => name.Length >= 3).WithMessage("Name should be minimum 3 characters");

            RuleFor(item => item.Description)
            .NotNull().WithMessage("Description should not be null.")
            .Must(t => t.Length >= 3).WithMessage("Description should be minimum 3 characters");
        }
    }
}
