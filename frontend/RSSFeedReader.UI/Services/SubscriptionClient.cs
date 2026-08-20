using System.Net.Http.Json;
using RSSFeedReader.UI.Models;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Subscription>> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Subscription>>("subscriptions") ?? [];
    }

    public async Task<Subscription?> AddAsync(string url)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "subscriptions",
            new AddSubscriptionRequest { Url = url });

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Subscription>();
    }
}
