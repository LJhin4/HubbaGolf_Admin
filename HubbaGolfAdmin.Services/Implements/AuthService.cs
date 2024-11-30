using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Services.Interfaces;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly EComDbContextExtCustom _DbContext;
        private readonly IMapper _Mapper;

        public AuthService(EComDbContextExtCustom dbContext, IMapper mapper)
        {
            _DbContext = dbContext;
            _Mapper = mapper;
        }
        public async Task<Result<UserDto>> AuthenticateAsync(string username, string password)
        {
            var zUser = await _DbContext.Users.Where(r => r.UserName == username && r.Active == 1 && r.RecordStatus != 99).FirstOrDefaultAsync();
            if (zUser != null)
            {
                var zHash = zUser.Password;
                var zValid = string.Equals(password, zHash);
                if (zValid)
                {
                    zUser.LastLoginOn = DateTime.Now;
                    _DbContext.Update(zUser);
                    await _DbContext.SaveChangesAsync();

                    var zUserDto = _Mapper.Map<UserDto>(zUser);
                    
                    return new Result<UserDto>
                    {
                        Success = true,
                        Data = zUserDto
                    };
                }
                else
                {
                    return new Result<UserDto>
                    {
                        Success = false,
                        Message = "Password is incorrect"
                    };
                }
            }
            else
            {
                return new Result<UserDto>
                {
                    Success = false,
                    Message = "User not found"
                };
            }
        }
        public async Task<Result<bool>> ChangePassAsync(int userId, string passnew)
        {
            var zUser = await _DbContext.Users.Where(r => r.Id == userId && r.RecordStatus != 99 && r.Active == 1).FirstOrDefaultAsync();
            if (zUser == null)
            {
                return new Result<bool> { Success = false, Message = $"User not found {userId}" };
            }

            try
            {
                zUser.Password = passnew;
                _DbContext.Update(zUser);
                await _DbContext.SaveChangesAsync();

                return new Result<bool> { Success = true };
            }
            catch (Exception ex)
            {
                return new Result<bool> { Success = false, Message = $"Error update {ex.Message}" };
            }
        }
    }
}
