using ZamZamAPI.Dtos.User;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<Result<List<UserDto>>> GetAllUsers();
        Task<Result<long>> Create(UserRequestDto dto);
        Task<Result<LoginResponseDto>> Login(LoginRequestDto dto);
    }
}
