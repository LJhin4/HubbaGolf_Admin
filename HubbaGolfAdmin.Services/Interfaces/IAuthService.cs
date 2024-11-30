using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// check username password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>userDto as data</returns>
        Task<Result<UserDto>> AuthenticateAsync(string username, string password);

        Task<Result<bool>> ChangePassAsync(int userId, string passnew);
    }
}