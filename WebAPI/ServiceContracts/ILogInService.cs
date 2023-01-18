namespace Whatsapp_bot.ServiceContracts
{
    public interface ILogInService
    {
        int CreateOTP(Guid UserId);
        bool ValidateOTP(Guid UserId, int OTP);
    }
}
