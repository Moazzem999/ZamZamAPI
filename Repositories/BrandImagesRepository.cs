using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.BrandImage;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class BrandImagesRepository(AppDbContext context, IWebHostEnvironment environment) : IBrandImagesRepository
    {
        private readonly AppDbContext context = context;
        private readonly IWebHostEnvironment environment = environment;

        public async Task<Result<List<BrandImageResponseDto>>> GetAll()
        {
            var data = await context.BrandImages.AsNoTracking()
                .Where(x => x.Status == Status.Active)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var list = data.Select(MapToDto).ToList();
            return await Result<List<BrandImageResponseDto>>.SuccessAsync("Brand images retrieved successfully.", list);
        }

        public async Task<Result<BrandImageResponseDto>> GetById(long id)
        {
            var entity = await context.BrandImages.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<BrandImageResponseDto>.RecordNotFoundAsync("Brand image not found.");
            }

            return await Result<BrandImageResponseDto>.SuccessAsync("Brand image retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(BrandImageRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<long>.BadRequestAsync("Name cannot be empty.");
            }

            if (dto.Image == null || dto.Image.Length == 0)
            {
                return await Result<long>.BadRequestAsync("Image file is required.");
            }

            var imageValidationError = ValidateImage(dto.Image);
            if (imageValidationError != null)
            {
                return await Result<long>.BadRequestAsync(imageValidationError);
            }

            var imagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "BrandImages");

            var newEntity = new BrandImageEntity
            {
                Name = dto.Name,
                Remarks = dto.Remarks ?? string.Empty,
                ImagePath = imagePath
            };

            context.BrandImages.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Brand image successfully created.", newEntity.Id);
        }

        public async Task<Result<BrandImageResponseDto>> Update(BrandImageRequestDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return await Result<BrandImageResponseDto>.BadRequestAsync("Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<BrandImageResponseDto>.BadRequestAsync("Name cannot be empty.");
            }

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imageValidationError = ValidateImage(dto.Image);
                if (imageValidationError != null)
                {
                    return await Result<BrandImageResponseDto>.BadRequestAsync(imageValidationError);
                }
            }

            var entity = await context.BrandImages.FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);
            if (entity == null)
            {
                return await Result<BrandImageResponseDto>.RecordNotFoundAsync("Brand image not found.");
            }

            entity.Name = dto.Name;
            entity.Remarks = dto.Remarks ?? string.Empty;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(entity.ImagePath))
                {
                    Helper.DeleteImage(environment.ContentRootPath, entity.ImagePath);
                }

                entity.ImagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "BrandImages");
            }

            context.BrandImages.Update(entity);
            await context.SaveChangesAsync();

            return await Result<BrandImageResponseDto>.SuccessAsync("Brand image successfully updated.", MapToDto(entity));
        }

        private static string? ValidateImage(IFormFile image)
        {
            const int maxFileSize = 1024 * 1024; // 1MB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png"];

            if (image.Length > maxFileSize)
            {
                return "Image size must be less than 1 MB.";
            }

            var fileType = Path.GetExtension(image.FileName).ToLower();
            if (string.IsNullOrEmpty(fileType) || !allowedFileTypes.Contains(fileType))
            {
                return "Invalid file type. Allowed types: .jpg, .jpeg, .png";
            }

            return null;
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.BrandImages.FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);
            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Brand image not found.");
            }

            if (!string.IsNullOrEmpty(entity.ImagePath))
            {
                Helper.DeleteImage(environment.ContentRootPath, entity.ImagePath);
            }

            context.BrandImages.Remove(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Brand image successfully deleted.", true);
        }

        private static BrandImageResponseDto MapToDto(BrandImageEntity x)
        {
            return new BrandImageResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Remarks = x.Remarks,
                ImagePath = x.ImagePath,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
