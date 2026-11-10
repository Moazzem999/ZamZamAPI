using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.MachineryCategory;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class MachineryCategoryRepository(AppDbContext context) : IMachineryCategoryRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<MachineryCategoryResponseDto>>> GetAll(MachineryCategorySearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.MachineryCategories
                .AsNoTracking()
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x => x.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<MachineryCategoryResponseDto>(dtoList, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<MachineryCategoryResponseDto>>.SuccessAsync("Machinery categories retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<MachineryCategoryResponseDto>>> GetAllForDisplay()
        {
            var data = await context.MachineryCategories
                .AsNoTracking()
                .Where(x => x.Status == Status.Active)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            return await Result<List<MachineryCategoryResponseDto>>.SuccessAsync("Machinery categories retrieved successfully.", dtoList);
        }

        public async Task<Result<MachineryCategoryResponseDto>> GetById(long id)
        {
            var entity = await context.MachineryCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<MachineryCategoryResponseDto>.RecordNotFoundAsync("Machinery category not found.");
            }

            return await Result<MachineryCategoryResponseDto>.SuccessAsync("Machinery category retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<List<MachineryCategoryResponseDto>>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Result<List<MachineryCategoryResponseDto>>.SuccessAsync("Machinery categories retrieved successfully.", new List<MachineryCategoryResponseDto>());
            }

            var searchTerm = name.Trim();
            var data = await context.MachineryCategories
                .AsNoTracking()
                .Where(x => x.Status == Status.Active && x.Name.Contains(searchTerm))
                .ToListAsync();

            var list = data.Select(MapToDto).ToList();
            return await Result<List<MachineryCategoryResponseDto>>.SuccessAsync("Machinery categories retrieved successfully.", list);
        }

        public async Task<Result<long>> Create(MachineryCategoryRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<long>.BadRequestAsync("Category name is required.");
            }

            var category = new MachineryCategoryEntity
            {
                Name = dto.Name.Trim(),
                DisplayOrder = dto.DisplayOrder,
                Status = Status.Active
            };

            context.MachineryCategories.Add(category);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Machinery category successfully created.", category.Id);
        }

        public async Task<Result<MachineryCategoryResponseDto>> Update(MachineryCategoryRequestDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return await Result<MachineryCategoryResponseDto>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<MachineryCategoryResponseDto>.BadRequestAsync("Category name is required.");
            }

            var category = await context.MachineryCategories
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (category == null)
            {
                return await Result<MachineryCategoryResponseDto>.RecordNotFoundAsync("Machinery category not found.");
            }

            category.Name = dto.Name.Trim();
            category.DisplayOrder = dto.DisplayOrder;

            context.MachineryCategories.Update(category);
            await context.SaveChangesAsync();

            return await Result<MachineryCategoryResponseDto>.SuccessAsync("Machinery category successfully updated.", MapToDto(category));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var category = await context.MachineryCategories
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (category == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Machinery category not found.");
            }

            var hasActiveMachineries = await context.Machineries
                .AnyAsync(m => m.MachineryCategoryId == id && m.Status == Status.Active);

            if (hasActiveMachineries)
            {
                return await Result<bool>.BadRequestAsync("This category cannot be deleted because it has associated machineries. Please delete the machineries first.");
            }

            category.Status = Status.Deleted;

            context.MachineryCategories.Update(category);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Machinery category successfully deleted.", true);
        }

        private static MachineryCategoryResponseDto MapToDto(MachineryCategoryEntity x)
        {
            return new MachineryCategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}

