using Tekton.Api.Domain;

namespace Tekton.Api.Infraestructure.Repositories.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
