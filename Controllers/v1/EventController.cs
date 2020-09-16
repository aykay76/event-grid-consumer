using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("test")]
        public ActionResult Test()
        {
            _logger.Log(LogLevel.Information, $"Testing, testing...");
            return Ok("Test succeeded");
        }

        [HttpPost]
        [HttpGet]
        [Route("deliver"), AllowAnonymous]
        public async Task<IActionResult> Deliver()
        {
            _logger.Log(LogLevel.Information, "Action!");

            string response = string.Empty;

            string requestContent = await new StreamReader(Request.Body).ReadToEndAsync();
            _logger.Log(LogLevel.Information, $"Received events: {requestContent}");

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
            EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.Data is SubscriptionValidationEventData)
                {
                    var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
                    _logger.Log(LogLevel.Information, $"Got SubscriptionValidation event data, validationCode: {eventData.ValidationCode},  validationUrl: {eventData.ValidationUrl}, topic: {eventGridEvent.Topic}");

                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = eventData.ValidationCode
                    };

                    return Ok(responseData);
                }
            }

            return Ok(response);
        }
    }
}