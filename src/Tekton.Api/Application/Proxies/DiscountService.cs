
using Tekton.Api.Domain;

namespace Tekton.Api.Application.Proxies;

public class DiscountService
{
    private readonly IDiscountProvider discountProvider;

    public DiscountService(IDiscountProvider discountProvider)
    {
        this.discountProvider = discountProvider;
    }

    public async Task<DiscountResponse> GetDiscountAsync(int productId)
    {
        return await discountProvider.GetRandomDiscount(productId);
    }
}
