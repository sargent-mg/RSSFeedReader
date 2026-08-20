namespace RSSFeedReader.Api.Models;

public sealed class Subscription
{
    public string Url { get; init; } = string.Empty;
}

public sealed class AddSubscriptionRequest
{
    public string? Url { get; init; }
}
