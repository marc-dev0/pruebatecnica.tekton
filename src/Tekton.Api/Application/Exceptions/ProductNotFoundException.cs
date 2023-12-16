namespace Tekton.Api.Application.Exceptions;

public sealed class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(int productId)
        : base($"El producto con identificador {productId} no se encontro.")
    {
    }
}