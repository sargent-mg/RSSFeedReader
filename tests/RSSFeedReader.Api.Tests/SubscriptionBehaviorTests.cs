using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Api.Controllers;
using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionBehaviorTests
{
    [Fact]
    public void NewStoreStartsEmpty()
    {
        var store = new InMemorySubscriptionStore();

        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void StorePreservesOrderAndDuplicateValues()
    {
        var store = new InMemorySubscriptionStore();

        Assert.True(store.TryAdd("https://example.com/feed.xml", out _));
        Assert.True(store.TryAdd("https://example.org/atom.xml", out _));
        Assert.True(store.TryAdd("https://example.com/feed.xml", out _));

        var subscriptions = store.GetAll();
        Assert.Collection(
            subscriptions,
            item => Assert.Equal("https://example.com/feed.xml", item.Url),
            item => Assert.Equal("https://example.org/atom.xml", item.Url),
            item => Assert.Equal("https://example.com/feed.xml", item.Url));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\n")]
    public void StoreRejectsEmptyOrWhitespaceValuesWithoutMutation(string url)
    {
        var store = new InMemorySubscriptionStore();

        Assert.False(store.TryAdd(url, out _));
        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void NewStoreAfterRestartDoesNotContainPreviousValues()
    {
        var firstStore = new InMemorySubscriptionStore();
        firstStore.TryAdd("https://example.com/feed.xml", out _);

        var restartedStore = new InMemorySubscriptionStore();

        Assert.Empty(restartedStore.GetAll());
    }

    [Fact]
    public void GetEndpointReturnsOrderedSubscriptions()
    {
        var store = new InMemorySubscriptionStore();
        store.TryAdd("https://example.com/feed.xml", out _);
        var controller = new SubscriptionsController(store);

        var result = controller.Get();

        var response = Assert.IsType<OkObjectResult>(result.Result);
        var subscriptions = Assert.IsAssignableFrom<IReadOnlyList<Subscription>>(response.Value);
        Assert.Single(subscriptions);
        Assert.Equal("https://example.com/feed.xml", subscriptions[0].Url);
    }

    [Fact]
    public void PostEndpointReturnsCreatedSubscription()
    {
        var controller = new SubscriptionsController(new InMemorySubscriptionStore());

        var result = controller.Post(new AddSubscriptionRequest { Url = "https://example.com/feed.xml" });

        var response = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, response.StatusCode);
        var subscription = Assert.IsType<Subscription>(response.Value);
        Assert.Equal("https://example.com/feed.xml", subscription.Url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void PostEndpointReturnsBadRequestWithoutChangingList(string? url)
    {
        var store = new InMemorySubscriptionStore();
        var controller = new SubscriptionsController(store);

        var result = controller.Post(new AddSubscriptionRequest { Url = url });

        Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Empty(store.GetAll());
    }
}
