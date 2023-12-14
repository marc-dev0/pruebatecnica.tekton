using FluentValidation;

namespace Tekton.Api.Application.Products.Commands.InsertProduct;

public class InsertProductValidator : AbstractValidator<InsertProductCommand>
{
    public InsertProductValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("El nombre del producto no puede ser nulo")
            .NotEmpty().WithMessage("El nombre del producto no puede estar vacío");
        RuleFor(x => x.Status)
            .NotNull().WithMessage("El estado del producto no puede ser nulo");
        RuleFor(x => x.Description)
            .NotNull().WithMessage("La descripción del producto no puede ser nulo")
            .NotEmpty().WithMessage("La descripción del producto no puede estar vacío");
        RuleFor(x => x.Price)
            .NotNull().WithMessage("El precio del producto no puede ser nulo")
            .NotEmpty().WithMessage("El precio del producto no puede ser vacío")
            .GreaterThanOrEqualTo(0).WithMessage("El campo Price debe ser mayor o igual a 0."); ;
    }
}
