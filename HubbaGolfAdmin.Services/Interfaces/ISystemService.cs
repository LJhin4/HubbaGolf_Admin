using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Database.Models;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Interfaces
{
    public interface ISystemService
    {
        #region [Menu]
        Task<List<MenuDto>> GetListAllMenuAsync();
        Task<MenuDto> GetMenuByIdAsync(int id);
        Task<int> SaveMenuAsync(int id, MenuDto menuDto);
        Task<int> DeleteMenuAsync(int id);
        #endregion

        #region [User - User Menu]
        Task<UserDto> GetUserByIdAsync(int id);
        Task<List<UserDto>> GetListUserAsync();
        Task<int> SaveUserAsync(int id, UserDto userDto);
        Task<Result<int>> DeleteUserAsync(int id);
        Task<int> ChangeStatusUserAsync(int id);
        #endregion
    }
}
