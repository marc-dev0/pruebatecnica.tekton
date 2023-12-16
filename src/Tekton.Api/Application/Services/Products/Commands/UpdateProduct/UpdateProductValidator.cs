using FluentValidation;
using Tekton.Api.Application.Services.Products.Commands.InsertProduct;

namespace Tekton.Api.Application.Services.Products.Commands.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("El código de producto no puede estar vacío")
            .GreaterThanOrEqualTo(0).WithMessage("El código del producto debe ser mayor a 0.");
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
            .GreaterThanOrEqualTo(0).WithMessage("El campo Price debe ser mayor o igual a 0.");
    }
}
