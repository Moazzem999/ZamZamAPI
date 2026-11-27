using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.SecurityManu;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class SecurityManuRepository(AppDbContext context) : ISecurityManuRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<SecurityManuResponseDto>>> GetAll(SecurityManuSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.SecurityManus.AsNoTracking()
                .Include(x => x.ParentManu)
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.ManuName.Contains(searchTerm) ||
                    (x.ControllerName != null && x.ControllerName.Contains(searchTerm)) ||
                    (x.ActionName != null && x.ActionName.Contains(searchTerm)) ||
                    (x.ParentManu != null && x.ParentManu.ManuName.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.Sort ?? int.MaxValue)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var manus = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<SecurityManuResponseDto>(manus, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<SecurityManuResponseDto>>.SuccessAsync("Security menus retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<SecurityManuResponseDto>>> GetAllParentMenu()
        {
            var data = await context.SecurityManus.AsNoTracking()
                .Where(x => x.Status == Status.Active && x.ParentManuId == null)
                .OrderBy(x => x.Sort ?? int.MaxValue)
                .ThenBy(x => x.ManuName)
                .ToListAsync();

            var parentMenus = data.Select(MapToDto).ToList();
            return await Result<List<SecurityManuResponseDto>>.SuccessAsync("Parent security menus retrieved successfully.", parentMenus);
        }

        public async Task<Result<List<SecurityManuResponseDto>>> GetAllSecurityManus()
        {
            var data = await context.SecurityManus.AsNoTracking()
                .Include(x => x.ParentManu)
                .Where(x => x.Status == Status.Active)
                .OrderBy(x => x.Sort ?? int.MaxValue)
                .ThenBy(x => x.ManuName)
                .ToListAsync();

            var manus = data.Select(MapToDto).ToList();
            return await Result<List<SecurityManuResponseDto>>.SuccessAsync("Security menus retrieved successfully.", manus);
        }

        public async Task<Result<SecurityManuResponseDto>> GetById(long id)
        {
            var entity = await context.SecurityManus.AsNoTracking()
                .Include(x => x.ParentManu)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<SecurityManuResponseDto>.RecordNotFoundAsync("Security menu not found.");
            }

            return await Result<SecurityManuResponseDto>.SuccessAsync("Security menu retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(SecurityManuRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.ManuName))
            {
                return await Result<long>.BadRequestAsync("Menu name is required.");
            }

            long? parentManuId = dto.ParentManuId.HasValue && dto.ParentManuId.Value > 0
                ? dto.ParentManuId.Value
                : null;

            if (parentManuId.HasValue)
            {
                var parentExists = await context.SecurityManus.AsNoTracking()
                    .AnyAsync(x => x.Id == parentManuId.Value && x.Status == Status.Active);

                if (!parentExists)
                {
                    return await Result<long>.RecordNotFoundAsync("Parent security menu not found.");
                }
            }

            var manuName = dto.ManuName.Trim();

            var exists = await context.SecurityManus.AsNoTracking()
                .AnyAsync(x => x.Status == Status.Active && x.ParentManuId == parentManuId && x.ManuName.ToLower() == manuName.ToLower());

            if (exists)
            {
                return await Result<long>.RecordAlreadyExistAsync("Security menu already exists.");
            }

            var newEntity = new SecurityManuEntity
            {
                ParentManuId = parentManuId,
                ManuName = manuName,
                ControllerName = dto.ControllerName?.Trim(),
                ActionName = dto.ActionName?.Trim(),
                Icon = dto.Icon?.Trim(),
                Sort = dto.Sort
            };

            context.SecurityManus.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Security menu successfully created.", newEntity.Id);
        }

        public async Task<Result<SecurityManuResponseDto>> Update(SecurityManuRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<SecurityManuResponseDto>.BadRequestAsync("Invalid request data.");
            }

            var entity = await context.SecurityManus
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<SecurityManuResponseDto>.RecordNotFoundAsync("Security menu not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.ManuName))
            {
                return await Result<SecurityManuResponseDto>.BadRequestAsync("Menu name is required.");
            }

            long? parentManuId = dto.ParentManuId.HasValue && dto.ParentManuId.Value > 0
                ? dto.ParentManuId.Value
                : null;

            if (parentManuId == dto.Id)
            {
                return await Result<SecurityManuResponseDto>.BadRequestAsync("Security menu cannot be its own parent.");
            }

            if (parentManuId.HasValue)
            {
                var parentExists = await context.SecurityManus.AsNoTracking()
                    .AnyAsync(x => x.Id == parentManuId.Value && x.Status == Status.Active);

                if (!parentExists)
                {
                    return await Result<SecurityManuResponseDto>.RecordNotFoundAsync("Parent security menu not found.");
                }
            }

            var manuName = dto.ManuName.Trim();

            var exists = await context.SecurityManus.AsNoTracking()
                .AnyAsync(x => x.Id != dto.Id && x.Status == Status.Active && x.ParentManuId == parentManuId && x.ManuName.ToLower() == manuName.ToLower());

            if (exists)
            {
                return await Result<SecurityManuResponseDto>.RecordAlreadyExistAsync("Security menu already exists.");
            }

            entity.ParentManuId = parentManuId;
            entity.ManuName = manuName;
            entity.ControllerName = dto.ControllerName?.Trim();
            entity.ActionName = dto.ActionName?.Trim();
            entity.Icon = dto.Icon?.Trim();
            entity.Sort = dto.Sort;

            context.SecurityManus.Update(entity);
            await context.SaveChangesAsync();

            var updatedEntity = await context.SecurityManus.AsNoTracking()
                .Include(x => x.ParentManu)
                .FirstAsync(x => x.Id == entity.Id);

            return await Result<SecurityManuResponseDto>.SuccessAsync("Security menu successfully updated.", MapToDto(updatedEntity));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.SecurityManus
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Security menu not found.");
            }

            entity.Status = Status.Deleted;

            context.SecurityManus.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Security menu successfully deleted.", true);
        }

        private static SecurityManuResponseDto MapToDto(SecurityManuEntity x)
        {
            return new SecurityManuResponseDto
            {
                Id = x.Id,
                ParentManuId = x.ParentManuId,
                ParentManuName = x.ParentManu?.ManuName,
                ManuName = x.ManuName,
                ControllerName = x.ControllerName,
                ActionName = x.ActionName,
                Icon = x.Icon,
                Sort = x.Sort,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
