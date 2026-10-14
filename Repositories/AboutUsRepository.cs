using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.AboutUs;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class AboutUsRepository(AppDbContext context) : IAboutUsRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<List<AboutUsResponseDto>>> GetAll()
        {
            var data = await context.AboutUs.AsNoTracking()
                .Where(x => x.Status == Status.Active)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var list = data.Select(MapToDto).ToList();
            return await Result<List<AboutUsResponseDto>>.SuccessAsync("About us retrieved successfully.", list);
        }

        public async Task<Result<AboutUsResponseDto>> GetById(long id)
        {
            var entity = await context.AboutUs.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<AboutUsResponseDto>.RecordNotFoundAsync("About us not found.");
            }

            return await Result<AboutUsResponseDto>.SuccessAsync("About us retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(AboutUsRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request.");
            }

            var alreadyExists = await context.AboutUs.AnyAsync(x => x.Status == Status.Active);
            if (alreadyExists)
            {
                return await Result<long>.BadRequestAsync("An About Us record already exists. Only one record is allowed.");
            }

            if (string.IsNullOrWhiteSpace(dto.SectionLabel))
            {
                return await Result<long>.BadRequestAsync("Section label cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(dto.MainHeading))
            {
                return await Result<long>.BadRequestAsync("Main heading cannot be empty.");
            }

            var newEntity = new AboutUsEntity
            {
                SectionLabel = dto.SectionLabel,
                MainHeading = dto.MainHeading,
                Introduction = dto.Introduction ?? string.Empty,
                ExperienceDescription = dto.ExperienceDescription ?? string.Empty,
                ProductionDescription = dto.ProductionDescription ?? string.Empty,
                CapabilityTitle = dto.CapabilityTitle ?? string.Empty,
                CapabilityDescription = dto.CapabilityDescription ?? string.Empty
            };

            context.AboutUs.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("About us successfully created.", newEntity.Id);
        }

        public async Task<Result<AboutUsResponseDto>> Update(AboutUsRequestDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return await Result<AboutUsResponseDto>.BadRequestAsync("Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(dto.SectionLabel))
            {
                return await Result<AboutUsResponseDto>.BadRequestAsync("Section label cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(dto.MainHeading))
            {
                return await Result<AboutUsResponseDto>.BadRequestAsync("Main heading cannot be empty.");
            }

            var entity = await context.AboutUs.FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);
            if (entity == null)
            {
                return await Result<AboutUsResponseDto>.RecordNotFoundAsync("About us not found.");
            }

            entity.SectionLabel = dto.SectionLabel;
            entity.MainHeading = dto.MainHeading;
            entity.Introduction = dto.Introduction ?? string.Empty;
            entity.ExperienceDescription = dto.ExperienceDescription ?? string.Empty;
            entity.ProductionDescription = dto.ProductionDescription ?? string.Empty;
            entity.CapabilityTitle = dto.CapabilityTitle ?? string.Empty;
            entity.CapabilityDescription = dto.CapabilityDescription ?? string.Empty;

            context.AboutUs.Update(entity);
            await context.SaveChangesAsync();

            return await Result<AboutUsResponseDto>.SuccessAsync("About us successfully updated.", MapToDto(entity));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.AboutUs.FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);
            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("About us not found.");
            }

            context.AboutUs.Remove(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("About us successfully deleted.", true);
        }

        private static AboutUsResponseDto MapToDto(AboutUsEntity x)
        {
            return new AboutUsResponseDto
            {
                Id = x.Id,
                SectionLabel = x.SectionLabel,
                MainHeading = x.MainHeading,
                Introduction = x.Introduction,
                ExperienceDescription = x.ExperienceDescription,
                ProductionDescription = x.ProductionDescription,
                CapabilityTitle = x.CapabilityTitle,
                CapabilityDescription = x.CapabilityDescription,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
