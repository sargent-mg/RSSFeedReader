using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SubscriptionsController(ISubscriptionStore store) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<Subscription>> Get()
    {
        return Ok(store.GetAll());
    }

    [HttpPost]
    public ActionResult<Subscription> Post([FromBody] AddSubscriptionRequest? request)
    {
        if (request is null || !store.TryAdd(request.Url, out var subscription))
        {
            return BadRequest(new { error = "A non-empty subscription URL is required." });
        }

        return CreatedAtAction(nameof(Get), null, subscription);
    }
}
