using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.Employee;
using ZamZamAPI.Dtos.User;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class EmployeesService(
        IEmployeesRepository employeesRepository,
        IUsersService usersService,
        AppDbContext context) : IEmployeesService
    {
        private readonly IEmployeesRepository employeesRepository = employeesRepository;
        private readonly IUsersService usersService = usersService;
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<EmployeeResponseDto>>> GetAll(EmployeeSearchDto searchDto)
        {
            return await employeesRepository.GetAll(searchDto);
        }

        public async Task<Result<EmployeeResponseDto>> GetById(long id)
        {
            return await employeesRepository.GetById(id);
        }

        public async Task<Result<List<EmployeeResponseDto>>> GetByName(string name)
        {
            return await employeesRepository.GetByName(name);
        }

        public async Task<Result<long>> Create(EmployeeRequestDto dto)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var employeeResult = await employeesRepository.Create(dto);
                if (!employeeResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return employeeResult;
                }

                var userDto = new UserRequestDto
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    UserName = dto.Email,
                    Password = "111111",
                    EmployeeId = employeeResult.Data
                };

                var userResult = await usersService.Create(userDto);
                if (!userResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return await Result<long>.ErrorAsync(userResult.Message, userResult.StatusCode);
                }

                var employeeRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.Name == "Employee" && r.Status == Status.Active);

                if (employeeRole == null)
                {
                    await transaction.RollbackAsync();
                    return await Result<long>.RecordNotFoundAsync("Employee role not found.");
                }

                var userRoleMapping = new UserRoleMappingEntity
                {
                    UserId = userResult.Data,
                    RoleId = employeeRole.Id
                };

                context.UserRoleMappings.Add(userRoleMapping);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                return employeeResult;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return await Result<long>.ErrorAsync($"Failed to create employee: {ex.Message}", StatusCodes.Status500InternalServerError, ex);
            }
        }

        public async Task<Result<EmployeeResponseDto>> Update(EmployeeRequestDto dto)
        {
            return await employeesRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await employeesRepository.Delete(id);
        }
    }
}
