using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.UserRoleMapping;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class UserRoleMappingRepository(AppDbContext context) : IUserRoleMappingRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<UserRoleMappingResponseDto>>> GetAll(UserRoleMappingSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.UserRoleMappings.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Role)
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    (x.User != null && (x.User.Name.Contains(searchTerm) || x.User.Email.Contains(searchTerm))) ||
                    (x.Role != null && x.Role.Name.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappings = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<UserRoleMappingResponseDto>(mappings, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<UserRoleMappingResponseDto>>.SuccessAsync("User role mappings retrieved successfully.", pagedResult);
        }

        public async Task<Result<UserRoleMappingResponseDto>> GetById(long id)
        {
            var entity = await context.UserRoleMappings.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<UserRoleMappingResponseDto>.RecordNotFoundAsync("User role mapping not found.");
            }

            return await Result<UserRoleMappingResponseDto>.SuccessAsync("User role mapping retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(UserRoleMappingRequestDto dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.RoleId <= 0)
            {
                return await Result<long>.BadRequestAsync("Valid UserId and RoleId are required.");
            }

            var userExists = await context.Users.AsNoTracking()
                .AnyAsync(u => u.Id == dto.UserId && u.Status == Status.Active);

            if (!userExists)
            {
                return await Result<long>.RecordNotFoundAsync("User not found.");
            }

            var roleExists = await context.Roles.AsNoTracking()
                .AnyAsync(r => r.Id == dto.RoleId && r.Status == Status.Active);

            if (!roleExists)
            {
                return await Result<long>.RecordNotFoundAsync("Role not found.");
            }

            var mappingExists = await context.UserRoleMappings.AsNoTracking()
                .AnyAsync(x => x.Status == Status.Active && x.UserId == dto.UserId && x.RoleId == dto.RoleId);

            if (mappingExists)
            {
                return await Result<long>.RecordAlreadyExistAsync("User role mapping already exists.");
            }

            var newEntity = new UserRoleMappingEntity
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };

            context.UserRoleMappings.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("User role mapping successfully created.", newEntity.Id);
        }

        public async Task<Result<UserRoleMappingResponseDto>> Update(UserRoleMappingRequestDto dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.RoleId <= 0)
            {
                return await Result<UserRoleMappingResponseDto>.BadRequestAsync("Valid UserId and RoleId are required.");
            }

            var entity = await context.UserRoleMappings
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<UserRoleMappingResponseDto>.RecordNotFoundAsync("User role mapping not found.");
            }

            var userExists = await context.Users.AsNoTracking()
                .AnyAsync(u => u.Id == dto.UserId && u.Status == Status.Active);

            if (!userExists)
            {
                return await Result<UserRoleMappingResponseDto>.RecordNotFoundAsync("User not found.");
            }

            var roleExists = await context.Roles.AsNoTracking()
                .AnyAsync(r => r.Id == dto.RoleId && r.Status == Status.Active);

            if (!roleExists)
            {
                return await Result<UserRoleMappingResponseDto>.RecordNotFoundAsync("Role not found.");
            }

            var mappingExists = await context.UserRoleMappings.AsNoTracking()
                .AnyAsync(x => x.Id != dto.Id && x.Status == Status.Active && x.UserId == dto.UserId && x.RoleId == dto.RoleId);

            if (mappingExists)
            {
                return await Result<UserRoleMappingResponseDto>.RecordAlreadyExistAsync("User role mapping already exists.");
            }

            entity.UserId = dto.UserId;
            entity.RoleId = dto.RoleId;

            context.UserRoleMappings.Update(entity);
            await context.SaveChangesAsync();

            var updatedEntity = await context.UserRoleMappings.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstAsync(x => x.Id == entity.Id);

            return await Result<UserRoleMappingResponseDto>.SuccessAsync("User role mapping successfully updated.", MapToDto(updatedEntity));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.UserRoleMappings
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("User role mapping not found.");
            }

            entity.Status = Status.Deleted;

            context.UserRoleMappings.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("User role mapping successfully deleted.", true);
        }

        private static UserRoleMappingResponseDto MapToDto(UserRoleMappingEntity x)
        {
            return new UserRoleMappingResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User?.Name ?? string.Empty,
                UserEmail = x.User?.Email ?? string.Empty,
                RoleId = x.RoleId,
                RoleName = x.Role?.Name ?? string.Empty,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
