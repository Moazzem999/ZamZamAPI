using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.Role;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<RoleResponseDto>>> GetAll(RoleSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.Roles.AsNoTracking()
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.Name.Contains(searchTerm) ||
                    (x.Description != null && x.Description.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var roles = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<RoleResponseDto>(roles, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<RoleResponseDto>>.SuccessAsync("Roles retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<RoleResponseDto>>> GetAllRoles()
        {
            var data = await context.Roles.AsNoTracking()
                .Where(x => x.Status == Status.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var roles = data.Select(MapToDto).ToList();
            return await Result<List<RoleResponseDto>>.SuccessAsync("Roles retrieved successfully.", roles);
        }

        public async Task<Result<RoleResponseDto>> GetById(long id)
        {
            var entity = await context.Roles.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<RoleResponseDto>.RecordNotFoundAsync("Role not found.");
            }

            return await Result<RoleResponseDto>.SuccessAsync("Role retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(RoleRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<long>.BadRequestAsync("Role name is required.");
            }

            var roleName = dto.Name.Trim();

            var exists = await context.Roles.AsNoTracking()
                .AnyAsync(x => x.Status == Status.Active && x.Name.ToLower() == roleName.ToLower());

            if (exists)
            {
                return await Result<long>.RecordAlreadyExistAsync("Role already exists.");
            }

            var newEntity = new RoleEntity
            {
                Name = roleName,
                Description = dto.Description?.Trim()
            };

            context.Roles.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Role successfully created.", newEntity.Id);
        }

        public async Task<Result<RoleResponseDto>> Update(RoleRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<RoleResponseDto>.BadRequestAsync("Invalid request data.");
            }

            var entity = await context.Roles
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<RoleResponseDto>.RecordNotFoundAsync("Role not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<RoleResponseDto>.BadRequestAsync("Role name is required.");
            }

            var roleName = dto.Name.Trim();

            var exists = await context.Roles.AsNoTracking()
                .AnyAsync(x => x.Id != dto.Id && x.Status == Status.Active && x.Name.ToLower() == roleName.ToLower());

            if (exists)
            {
                return await Result<RoleResponseDto>.RecordAlreadyExistAsync("Role already exists.");
            }

            entity.Name = roleName;
            entity.Description = dto.Description?.Trim();

            context.Roles.Update(entity);
            await context.SaveChangesAsync();

            return await Result<RoleResponseDto>.SuccessAsync("Role successfully updated.", MapToDto(entity));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.Roles
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Role not found.");
            }

            entity.Status = Status.Deleted;

            context.Roles.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Role successfully deleted.", true);
        }

        private static RoleResponseDto MapToDto(RoleEntity x)
        {
            return new RoleResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
