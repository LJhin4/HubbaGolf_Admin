using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Database.Models;
using HubbaGolfAdmin.Services.Interfaces;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Implements
{
    public class SystemService : ISystemService
    {
        private readonly EComDbContextExtCustom _DbContext;
        private readonly IMapper _Mapper;

        public SystemService(EComDbContextExtCustom dbContext, IMapper mapper)
        {
            _DbContext = dbContext;
            _Mapper = mapper;
        }
        //-----------------------------------------------------------------------------
        public async Task<List<UserDto>> GetListUserAsync()
        {
            var zQuery = from user in _DbContext.Users
                         where
                         user.RecordStatus != 99
                         select new UserDto
                         {
                             Id = user.Id,
                             UserName = user.UserName,
                             GroupName = null,
                             GroupId = null,
                             LastLoginOn = user.LastLoginOn,
                             Description = user.Description,
                             Password = user.Password,
                             Active = user.Active,
                             DisplayName = user.DisplayName,
                             Email = user.Email
                         };

            return await zQuery.ToListAsync();
        }
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var zUser = await _DbContext.Users.FindAsync(id);
            return _Mapper.Map<UserDto>(zUser);
        }
        public async Task<int> SaveUserAsync(int id, UserDto userDto)
        {
            if (id != 0)
            {
                var zUser = await _DbContext.Users.FindAsync(id);
                zUser = _Mapper.Map(userDto, zUser);
                _DbContext.Update(zUser);
            }
            else
            {
                var zUser = _Mapper.Map(userDto, new User());
                _DbContext.Add(zUser);
            }

            return await _DbContext.SaveChangesAsync();
        }
        public async Task<Result<int>> DeleteUserAsync(int id)
        {
            var zUser = await _DbContext.Users.FindAsync(id);
            _DbContext.Remove(zUser);
            var result = await _DbContext.SaveChangesAsync();

            if (result > 0)
            {
                return new Result<int>
                {
                    Success = true,
                    Message = "Updated successfully!"
                };
            }
            return new Result<int>
            {
                Success = false,
                Message = "An error occurred!"
            };
        }
        public async Task<int> ChangeStatusUserAsync(int id)
        {
            var zUser = await _DbContext.Users.FindAsync(id);
            var zActive = zUser.Active;
            if (zActive == 1)
            {
                zUser.Active = 0;
            }
            else
            {
                zUser.Active = 1;
            }
            _DbContext.Update(zUser);
            return await _DbContext.SaveChangesAsync();
        }
        //-----------------------------------------------------------------------------
        public async Task<int> SaveMenuAsync(int id, MenuDto menuDto)
        {
            if (id != 0)
            {
                var zMenu = await _DbContext.Menus.FindAsync(id);
                zMenu = _Mapper.Map(menuDto, zMenu);
                _DbContext.Update(zMenu);
            }
            else
            {
                var zMenu = _Mapper.Map(menuDto, new Menu());
                _DbContext.Add(zMenu);
            }

            return await _DbContext.SaveChangesAsync();
        }
        public async Task<int> DeleteMenuAsync(int id)
        {
            var zMenu = await _DbContext.Menus.FindAsync(id);
            _DbContext.Remove(zMenu);
            return await _DbContext.SaveChangesAsync();
        }
        public async Task<MenuDto> GetMenuByIdAsync(int id)
        {
            var zMenu = await _DbContext.Menus.FindAsync(id);
            return _Mapper.Map<MenuDto>(zMenu);
        }
        public async Task<List<MenuDto>> GetListAllMenuAsync()
        {
            var zMenu = await _DbContext.Menus
                                                .Where(r => r.RecordStatus != 99)
                                                .OrderByDescending(r => r.Id)
                                                .ToListAsync();
            var zMenuDto = new List<MenuDto>();
            zMenuDto = _Mapper.Map<List<MenuDto>>(zMenu);
            return zMenuDto;
        }
    }
}