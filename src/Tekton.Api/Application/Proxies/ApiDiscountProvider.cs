using System.Net.Http;
using Tekton.Api.Domain;

namespace Tekton.Api.Application.Proxies;

public class ApiDiscountProvider : IDiscountProvider
{
    private readonly HttpClient _httpClient;

    public ApiDiscountProvider(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<DiscountResponse?> GetRandomDiscount(int productId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://657bf061394ca9e4af1507b0.mockapi.io/api/v1/discounts/{productId}"
        );

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var discount = await response.Content.ReadFromJsonAsync<DiscountResponse>();

            return discount;
        }

        return null;
    }
}