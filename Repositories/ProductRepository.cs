using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.Product;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class ProductRepository(AppDbContext context, IWebHostEnvironment environment) : IProductRepository
    {
        private readonly AppDbContext context = context;
        private readonly IWebHostEnvironment environment = environment;

        public async Task<Result<PagedResult<ProductResponseDto>>> GetAll(ProductSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.Products
                .AsNoTracking()
                .Include(x => x.ProductCategory)
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.Title.Contains(searchTerm) ||
                    x.CardBadge.Contains(searchTerm) ||
                    x.Description.Contains(searchTerm) ||
                    (x.ProductCategory != null && x.ProductCategory.Name.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<ProductResponseDto>(dtoList, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<ProductResponseDto>>.SuccessAsync("Products retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<ProductResponseDto>>> GetAllForDisplay(long categoryId)
        {
            var query = context.Products
                .AsNoTracking()
                .Include(x => x.ProductCategory)
                .Where(x => x.Status == Status.Active);

            if (categoryId > 0)
            {
                query = query.Where(x => x.ProductCategoryId == categoryId);
            }

            var data = await query
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            return await Result<List<ProductResponseDto>>.SuccessAsync("Products retrieved successfully.", dtoList);
        }

        public async Task<Result<ProductResponseDto>> GetById(long id)
        {
            var entity = await context.Products
                .AsNoTracking()
                .Include(x => x.ProductCategory)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<ProductResponseDto>.RecordNotFoundAsync("Product not found.");
            }

            return await Result<ProductResponseDto>.SuccessAsync("Product retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(ProductRequestDto dto)
        {
            var maxFileSize = 1024 * 1024; // 1 MB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png", ".webp"];

            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (dto.ProductCategoryId <= 0)
            {
                return await Result<long>.BadRequestAsync("Please select a valid product category.");
            }

            var categoryExists = await context.ProductCategories
                .AnyAsync(c => c.Id == dto.ProductCategoryId && c.Status == Status.Active);

            if (!categoryExists)
            {
                return await Result<long>.BadRequestAsync("Selected product category does not exist.");
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

                imagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "Products");
            }

            var product = new ProductEntity
            {
                ProductCategoryId = dto.ProductCategoryId,
                Title = dto.Title.Trim(),
                CardBadge = dto.CardBadge?.Trim() ?? string.Empty,
                Description = dto.Description?.Trim() ?? string.Empty,
                ImagePath = imagePath,
                DisplayOrder = dto.DisplayOrder,
                Status = Status.Active
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Product successfully created.", product.Id);
        }

        public async Task<Result<ProductResponseDto>> Update(ProductRequestDto dto)
        {
            var maxFileSize = 1024 * 1024; // 1 MB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png", ".webp"];

            if (dto == null || dto.Id <= 0)
            {
                return await Result<ProductResponseDto>.BadRequestAsync("Invalid request data.");
            }

            if (dto.ProductCategoryId <= 0)
            {
                return await Result<ProductResponseDto>.BadRequestAsync("Please select a valid product category.");
            }

            var categoryExists = await context.ProductCategories
                .AnyAsync(c => c.Id == dto.ProductCategoryId && c.Status == Status.Active);

            if (!categoryExists)
            {
                return await Result<ProductResponseDto>.BadRequestAsync("Selected product category does not exist.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<ProductResponseDto>.BadRequestAsync("Title is required.");
            }

            var product = await context.Products
                .Include(x => x.ProductCategory)
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (product == null)
            {
                return await Result<ProductResponseDto>.RecordNotFoundAsync("Product not found.");
            }

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileType = Path.GetExtension(dto.Image.FileName).ToLower();

                if (dto.Image.Length > maxFileSize)
                    return await Result<ProductResponseDto>.BadRequestAsync("Image size must be less than 1 MB.");

                if (string.IsNullOrEmpty(fileType) || !allowedFileTypes.Contains(fileType))
                    return await Result<ProductResponseDto>.BadRequestAsync("Invalid file type. Allowed types: .jpg, .jpeg, .png, .webp");

                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    Helper.DeleteImage(environment.ContentRootPath, product.ImagePath);
                }

                product.ImagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "Products");
            }

            product.ProductCategoryId = dto.ProductCategoryId;
            product.Title = dto.Title.Trim();
            product.CardBadge = dto.CardBadge?.Trim() ?? string.Empty;
            product.Description = dto.Description?.Trim() ?? string.Empty;
            product.DisplayOrder = dto.DisplayOrder;

            context.Products.Update(product);
            await context.SaveChangesAsync();

            // Re-fetch category if changed
            if (product.ProductCategory == null || product.ProductCategory.Id != product.ProductCategoryId)
            {
                var category = await context.ProductCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == product.ProductCategoryId);
                if (category != null)
                {
                    product.ProductCategory = category;
                }
            }

            return await Result<ProductResponseDto>.SuccessAsync("Product successfully updated.", MapToDto(product));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (product == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Product not found.");
            }

            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                Helper.DeleteImage(environment.ContentRootPath, product.ImagePath);
            }

            product.Status = Status.Deleted;
            context.Products.Update(product);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Product successfully deleted.", true);
        }

        private static ProductResponseDto MapToDto(ProductEntity x)
        {
            return new ProductResponseDto
            {
                Id = x.Id,
                ProductCategoryId = x.ProductCategoryId,
                ProductCategoryName = x.ProductCategory?.Name ?? string.Empty,
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

