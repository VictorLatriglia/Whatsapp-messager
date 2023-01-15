using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.Models;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Utils.Middleware;

namespace Whatsapp_bot.Controllers
{
    [ApiController]
    [EmeraldAuthorization]
    [Route("[controller]")]
    public class MoneyMovementController : Controller
    {
        private readonly IUserOutgoingsService _userOutgoingsService;

        public MoneyMovementController(
        IUserOutgoingsService userOutgoingsService)
        {
            _userOutgoingsService = userOutgoingsService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetMovements([FromHeader(Name = "X-User-Key")] Guid userId, DateTime beginDate, DateTime? endDate)
        {
            var results = await _userOutgoingsService.GetOutgoingsSummary(userId, beginDate, endDate);
            return Ok(new MoneyMovementResult(results));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> EditMovement([FromHeader(Name = "X-User-Key")] Guid userId, [FromBody] MoneyMovement data)
        {
            await _userOutgoingsService.UpdateMovement(userId, data);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteMovement([FromQuery] Guid moneyMovementId)
        {
            await _userOutgoingsService.DeleteMovement(moneyMovementId);
            return Ok();
        }
    }
}
