using Microsoft.Extensions.Caching.Memory;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services
{
    public class LogInService : ILogInService
    {
        readonly IMemoryCache _cache;

        public LogInService(
            IMemoryCache cache)
        {
            _cache = cache;
        }

        public int CreateOTP(Guid UserId)
        {
            Random random = new Random();
            var otp = random.Next(111111, 999999);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(UserId.ToString(), otp, cacheEntryOptions);

            return otp;
        }

        public bool ValidateOTP(Guid UserId, int OTP)
        {
            if (_cache.TryGetValue(UserId.ToString(), out int storedOTP))
            {
                if (OTP == storedOTP)
                {
                    _cache.Remove(UserId.ToString());
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
