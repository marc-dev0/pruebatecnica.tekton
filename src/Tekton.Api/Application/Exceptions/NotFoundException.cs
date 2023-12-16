namespace Tekton.Api.Application.Exceptions;

public abstract class NotFoundException : ApplicationException
{
    protected NotFoundException(string message)
        : base("No encontrado", message)
    {
    }
}