using Tekton.Api.Domain;

namespace Tekton.Api.Application.Proxies;

public interface IDiscountProvider
{
    Task<DiscountResponse> GetRandomDiscount(int productId);
}