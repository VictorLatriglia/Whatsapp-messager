
using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Utils.Middleware
{
    public class BackgroundAutoRemember : IBackgroundAutoRemember
    {
        readonly IHttpService<SendMessageTemplateModel, string> _httpService;
        readonly IRepository<UserOutgoing> _userOutgoingRepo;
        readonly IRepository<User> _usersRepo;
        readonly IVaultInformationService _vaultInformationService;

        public BackgroundAutoRemember(
            IHttpService<SendMessageTemplateModel, string> httpService,
            IRepository<UserOutgoing> userOutgoingRepo,
            IRepository<User> usersRepo,
            IVaultInformationService vaultInformationService)
        {
            _httpService = httpService;
            _userOutgoingRepo = userOutgoingRepo;
            _usersRepo = usersRepo;
            _vaultInformationService = vaultInformationService;
        }

        public async Task SendMessagesToUsers()
        {
            if (ManagedDateTime.Now.Hour > 22 && ManagedDateTime.Now.Hour < 7)
                return;

            var users = await _usersRepo.GetAllAsync();
            var baseUrl = _vaultInformationService.GetParameter(Globals.WHATSAPP_BASE_URL);
            var phoneId = _vaultInformationService.GetParameter(Globals.WHATSAPP_PHONE_ID);
            var token = _vaultInformationService.GetParameter(Globals.WHATSAPP_TOKEN);

            baseUrl = baseUrl.Replace("{PHONE_ID}", phoneId);
            foreach (User user in users)
            {
                var outgoings = await _userOutgoingRepo.QueryAsync(x => x.CreatedOn > ManagedDateTime.Now.AddHours(-12));
                if (outgoings.Count == 0)
                {
                    var objectData = new SendMessageTemplateModel(user.PhoneNumber, user.Name);
                    await _httpService.PostAsync(baseUrl, objectData, token);
                }
            }
        }
    }

    public interface IBackgroundAutoRemember
    {
        Task SendMessagesToUsers();
    }
}
