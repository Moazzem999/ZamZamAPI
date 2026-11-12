using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.ProductCategory;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class ProductCategoryRepository(AppDbContext context) : IProductCategoryRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<ProductCategoryResponseDto>>> GetAll(ProductCategorySearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.ProductCategories
                .AsNoTracking()
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x => x.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<ProductCategoryResponseDto>(dtoList, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<ProductCategoryResponseDto>>.SuccessAsync("Product categories retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<ProductCategoryResponseDto>>> GetAllForDisplay()
        {
            var data = await context.ProductCategories
                .AsNoTracking()
                .Where(x => x.Status == Status.Active)
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.Id)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            return await Result<List<ProductCategoryResponseDto>>.SuccessAsync("Product categories retrieved successfully.", dtoList);
        }

        public async Task<Result<ProductCategoryResponseDto>> GetById(long id)
        {
            var entity = await context.ProductCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<ProductCategoryResponseDto>.RecordNotFoundAsync("Product category not found.");
            }

            return await Result<ProductCategoryResponseDto>.SuccessAsync("Product category retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<List<ProductCategoryResponseDto>>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Result<List<ProductCategoryResponseDto>>.SuccessAsync("Product categories retrieved successfully.", new List<ProductCategoryResponseDto>());
            }

            var searchTerm = name.Trim();
            var data = await context.ProductCategories
                .AsNoTracking()
                .Where(x => x.Status == Status.Active && x.Name.Contains(searchTerm))
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.Id)
                .ToListAsync();

            var list = data.Select(MapToDto).ToList();
            return await Result<List<ProductCategoryResponseDto>>.SuccessAsync("Product categories retrieved successfully.", list);
        }

        public async Task<Result<long>> Create(ProductCategoryRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<long>.BadRequestAsync("Category name is required.");
            }

            var category = new ProductCategoryEntity
            {
                Name = dto.Name.Trim(),
                DisplayOrder = dto.DisplayOrder,
                Status = Status.Active
            };

            context.ProductCategories.Add(category);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Product category successfully created.", category.Id);
        }

        public async Task<Result<ProductCategoryResponseDto>> Update(ProductCategoryRequestDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return await Result<ProductCategoryResponseDto>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<ProductCategoryResponseDto>.BadRequestAsync("Category name is required.");
            }

            var category = await context.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (category == null)
            {
                return await Result<ProductCategoryResponseDto>.RecordNotFoundAsync("Product category not found.");
            }

            category.Name = dto.Name.Trim();
            category.DisplayOrder = dto.DisplayOrder;

            context.ProductCategories.Update(category);
            await context.SaveChangesAsync();

            return await Result<ProductCategoryResponseDto>.SuccessAsync("Product category successfully updated.", MapToDto(category));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var category = await context.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (category == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Product category not found.");
            }

            var hasActiveProducts = await context.Products
                .AnyAsync(p => p.ProductCategoryId == id && p.Status == Status.Active);

            if (hasActiveProducts)
            {
                return await Result<bool>.BadRequestAsync("This category cannot be deleted because it has associated products. Please delete the products first.");
            }

            category.Status = Status.Deleted;

            context.ProductCategories.Update(category);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Product category successfully deleted.", true);
        }

        private static ProductCategoryResponseDto MapToDto(ProductCategoryEntity x)
        {
            return new ProductCategoryResponseDto
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

