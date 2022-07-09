namespace Whatsapp_bot.Controllers;
using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.Models;
using Whatsapp_bot.ServiceContracts;

[ApiController]
[Route("[controller]")]
public class UserInformationController : ControllerBase
{
    private readonly IUserInformationService _userService;

    public UserInformationController(
        IUserInformationService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task CreateUser(UserCreationModel data)
    {
        await _userService.AddUser(data.UserName, data.PhoneNumber);
    }
}