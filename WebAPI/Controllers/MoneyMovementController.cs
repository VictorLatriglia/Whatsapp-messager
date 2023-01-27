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
        private readonly ILoggerService _loggerService;

        public MoneyMovementController(
        IUserOutgoingsService userOutgoingsService,
        ILoggerService loggerService)
        {
            _userOutgoingsService = userOutgoingsService;
            _loggerService = loggerService;
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(MoneyMovementResult),200)]
        public async Task<IActionResult> GetMovements([FromHeader(Name = "X-User-Key")] Guid userId, DateTime beginDate, DateTime? endDate)
        {
            try
            {
                var results = await _userOutgoingsService.GetOutgoingsSummary(userId, beginDate, endDate);
                return Ok(new MoneyMovementResult(results));
            }
            catch (Exception ex)
            {
                await _loggerService.SaveLog(ex.ToString(),true,ActionType.InternalProcess);
                return StatusCode(500);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> EditMovement([FromHeader(Name = "X-User-Key")] Guid userId, [FromBody] MoneyMovementUpdate data)
        {
            try
            {
                await _userOutgoingsService.UpdateMovement(userId, data.BuildEntityDBO());
                return Ok();
            }
            catch (Exception ex)
            {
                await _loggerService.SaveLog(ex.ToString(), true, ActionType.InternalProcess);
                return StatusCode(500);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteMovement([FromQuery] Guid moneyMovementId)
        {
            try
            {
                await _userOutgoingsService.DeleteMovement(moneyMovementId);
                return Ok();
            }
            catch (Exception ex)
            {
                await _loggerService.SaveLog(ex.ToString(), true, ActionType.InternalProcess);
                return StatusCode(500);
            }
        }
    }
}
