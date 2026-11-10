using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.Machinery;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class MachineryRepository(AppDbContext context, IWebHostEnvironment environment) : IMachineryRepository
    {
        private readonly AppDbContext context = context;
        private readonly IWebHostEnvironment environment = environment;

        public async Task<Result<PagedResult<MachineryResponseDto>>> GetAll(MachinerySearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.Machineries
                .AsNoTracking()
                .Include(x => x.MachineryCategory)
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.Title.Contains(searchTerm) ||
                    x.CardBadge.Contains(searchTerm) ||
                    x.Description.Contains(searchTerm) ||
                    (x.MachineryCategory != null && x.MachineryCategory.Name.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<MachineryResponseDto>(dtoList, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<MachineryResponseDto>>.SuccessAsync("Machineries retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<MachineryResponseDto>>> GetAllForDisplay(long categoryId)
        {
            var query = context.Machineries
                .AsNoTracking()
                .Include(x => x.MachineryCategory)
                .Where(x => x.Status == Status.Active);

            if (categoryId > 0)
            {
                query = query.Where(x => x.MachineryCategoryId == categoryId);
            }

            var data = await query
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            return await Result<List<MachineryResponseDto>>.SuccessAsync("Machineries retrieved successfully.", dtoList);
        }

        public async Task<Result<MachineryResponseDto>> GetById(long id)
        {
            var entity = await context.Machineries
                .AsNoTracking()
                .Include(x => x.MachineryCategory)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<MachineryResponseDto>.RecordNotFoundAsync("Machinery not found.");
            }

            return await Result<MachineryResponseDto>.SuccessAsync("Machinery retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(MachineryRequestDto dto)
        {
            var maxFileSize = 1024 * 1024; // 1 MB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png", ".webp"];

            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (dto.MachineryCategoryId <= 0)
            {
                return await Result<long>.BadRequestAsync("Please select a valid machinery category.");
            }

            var categoryExists = await context.MachineryCategories
                .AnyAsync(c => c.Id == dto.MachineryCategoryId && c.Status == Status.Active);

            if (!categoryExists)
            {
                return await Result<long>.BadRequestAsync("Selected machinery category does not exist.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<long>.BadRequestAsync("Title is required.");
            }

            string imagePath = string.Empty;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileType = Path.GetExtension(dto.Image.FileName).ToLower();

                if (dto.Image.Length > maxFileSize)
                    return await Result<long>.BadRequestAsync("Image size must be less than 1 MB.");

                if (string.IsNullOrEmpty(fileType) || !allowedFileTypes.Contains(fileType))
                    return await Result<long>.BadRequestAsync("Invalid file type. Allowed types: .jpg, .jpeg, .png, .webp");

                imagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "Machineries");
            }

            var machinery = new MachineryEntity
            {
                MachineryCategoryId = dto.MachineryCategoryId,
                Title = dto.Title.Trim(),
                CardBadge = dto.CardBadge?.Trim() ?? string.Empty,
                Description = dto.Description?.Trim() ?? string.Empty,
                ImagePath = imagePath,
                DisplayOrder = dto.DisplayOrder,
                Status = Status.Active
            };

            context.Machineries.Add(machinery);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Machinery successfully created.", machinery.Id);
        }

        public async Task<Result<MachineryResponseDto>> Update(MachineryRequestDto dto)
        {
            var maxFileSize = 1024 * 1024; // 1 MB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png", ".webp"];

            if (dto == null || dto.Id <= 0)
            {
                return await Result<MachineryResponseDto>.BadRequestAsync("Invalid request data.");
            }

            if (dto.MachineryCategoryId <= 0)
            {
                return await Result<MachineryResponseDto>.BadRequestAsync("Please select a valid machinery category.");
            }

            var categoryExists = await context.MachineryCategories
                .AnyAsync(c => c.Id == dto.MachineryCategoryId && c.Status == Status.Active);

            if (!categoryExists)
            {
                return await Result<MachineryResponseDto>.BadRequestAsync("Selected machinery category does not exist.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<MachineryResponseDto>.BadRequestAsync("Title is required.");
            }

            var machinery = await context.Machineries
                .Include(x => x.MachineryCategory)
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (machinery == null)
            {
                return await Result<MachineryResponseDto>.RecordNotFoundAsync("Machinery not found.");
            }

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileType = Path.GetExtension(dto.Image.FileName).ToLower();

                if (dto.Image.Length > maxFileSize)
                    return await Result<MachineryResponseDto>.BadRequestAsync("Image size must be less than 1 MB.");

                if (string.IsNullOrEmpty(fileType) || !allowedFileTypes.Contains(fileType))
                    return await Result<MachineryResponseDto>.BadRequestAsync("Invalid file type. Allowed types: .jpg, .jpeg, .png, .webp");

                if (!string.IsNullOrEmpty(machinery.ImagePath))
                {
                    Helper.DeleteImage(environment.ContentRootPath, machinery.ImagePath);
                }

                machinery.ImagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "Machineries");
            }

            machinery.MachineryCategoryId = dto.MachineryCategoryId;
            machinery.Title = dto.Title.Trim();
            machinery.CardBadge = dto.CardBadge?.Trim() ?? string.Empty;
            machinery.Description = dto.Description?.Trim() ?? string.Empty;
            machinery.DisplayOrder = dto.DisplayOrder;

            context.Machineries.Update(machinery);
            await context.SaveChangesAsync();

            // Re-fetch category if changed
            if (machinery.MachineryCategory == null || machinery.MachineryCategory.Id != machinery.MachineryCategoryId)
            {
                var category = await context.MachineryCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == machinery.MachineryCategoryId);
                if (category != null)
                {
                    machinery.MachineryCategory = category;
                }
            }

            return await Result<MachineryResponseDto>.SuccessAsync("Machinery successfully updated.", MapToDto(machinery));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var machinery = await context.Machineries
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (machinery == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Machinery not found.");
            }

            if (!string.IsNullOrEmpty(machinery.ImagePath))
            {
                Helper.DeleteImage(environment.ContentRootPath, machinery.ImagePath);
            }

            machinery.Status = Status.Deleted;
            context.Machineries.Update(machinery);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Machinery successfully deleted.", true);
        }

        private static MachineryResponseDto MapToDto(MachineryEntity x)
        {
            return new MachineryResponseDto
            {
                Id = x.Id,
                MachineryCategoryId = x.MachineryCategoryId,
                MachineryCategoryName = x.MachineryCategory?.Name ?? string.Empty,
                Title = x.Title,
                CardBadge = x.CardBadge,
                Description = x.Description,
                ImagePath = x.ImagePath,
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
