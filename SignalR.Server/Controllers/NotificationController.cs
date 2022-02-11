using Microsoft.AspNetCore.Mvc;
using SignalR.Server.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

       [HttpGet("sendNotification")]
        [SwaggerOperation(Summary = "Send message throught SignalR",
            Description = "Send 0 to get error.\n" +
            "Send 1 to get warning.")]
        public async Task<IActionResult> SendNotification(string message)
        {
            await _notificationService.SendNotification(message);
            return Ok();
        }
    }
}
