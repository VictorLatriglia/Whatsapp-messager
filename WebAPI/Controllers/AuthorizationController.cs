using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly IUserInformationService _userInformationService;
        private readonly ILogInService _logInService;
        private readonly IWhatsappMessageSenderService _whatsappMessageSenderService;

        public AuthorizationController(
            IUserInformationService userInformationService,
            ILogInService logInService,
            IWhatsappMessageSenderService whatsappMessageSenderService)
        {
            _userInformationService = userInformationService;
            _logInService = logInService;
            _whatsappMessageSenderService = whatsappMessageSenderService;
        }

        [HttpGet]
        public async Task<IActionResult> LogIn(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return BadRequest();
            if (phoneNumber.Length == 10)
                phoneNumber = $"57{phoneNumber}";

            var user = await _userInformationService.GetUserAsync(phoneNumber);
            if (user == null)
                return Unauthorized();

            var otp = _logInService.CreateOTP(user.Id);

            await _whatsappMessageSenderService.SendMessage(phoneNumber,$"Tu código de inicio de sesión es: *{otp}*");

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromHeader(Name = "X-User-Phone")] string userPhone, int otp)
        {
            if (string.IsNullOrEmpty(userPhone))
                return BadRequest();
            if (otp == 0)
                return BadRequest();

            if (otp < 111111 || otp > 999999)
                return Unauthorized();

            var user = await _userInformationService.GetUserAsync(userPhone);
            if (user == null)
                return Unauthorized();

            var otpValid = _logInService.ValidateOTP(user.Id, otp);

            if (!otpValid)
                return Unauthorized();

            return Ok(user.Id);
        }
    }
}
