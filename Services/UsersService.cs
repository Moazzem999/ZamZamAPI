using ZamZamAPI.Dtos.User;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class UsersService(IUsersRepository usersRepository) : IUsersService
    {
        public async Task<Result<List<UserDto>>> GetAllUsers()
        {
            return await usersRepository.GetAllUsers();
        }

        public async Task<Result<long>> Create(UserRequestDto dto)
        {
            return await usersRepository.Create(dto);
        }

        public async Task<Result<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            return await usersRepository.Login(dto);
        }
    }
}
