
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace App.Business
{
    public interface IHashService
    {
        ServiceResult<string> HashString(string input);
    }

    public class HashService : IHashService
    {
        public HashService(ILogger<HashService> logger)
        {
            Logger = logger;
        }

        public ILogger<HashService> Logger { get; }

        public ServiceResult<string> HashString(string input)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = SHA256.HashData(bytes);
                var hashedString = Convert.ToBase64String(hash);

                return ServiceResult.Success(hashedString);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error hashing string");
                return ServiceResult.Fail<string>("Error hashing string", StatusCodes.Status500InternalServerError);
            }
        }
    }
}
