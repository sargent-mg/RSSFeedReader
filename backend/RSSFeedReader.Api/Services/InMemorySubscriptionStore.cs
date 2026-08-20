using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.Services;

public interface ISubscriptionStore
{
    IReadOnlyList<Subscription> GetAll();

    bool TryAdd(string? url, out Subscription? subscription);
}

public sealed class InMemorySubscriptionStore : ISubscriptionStore
{
    private readonly List<Subscription> subscriptions = [];

    public IReadOnlyList<Subscription> GetAll()
    {
        return subscriptions.ToList();
    }

    public bool TryAdd(string? url, out Subscription? subscription)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            subscription = null;
            return false;
        }

        subscription = new Subscription { Url = url };
        subscriptions.Add(subscription);
        return true;
    }
}
