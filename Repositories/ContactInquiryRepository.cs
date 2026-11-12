using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.ContactInquiry;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class ContactInquiryRepository(AppDbContext context) : IContactInquiryRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<ContactInquiryResponseDto>>> GetAll(ContactInquirySearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.ContactInquiries
                .AsNoTracking()
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.FirstName.Contains(searchTerm) ||
                    x.LastName.Contains(searchTerm) ||
                    x.Email.Contains(searchTerm) ||
                    x.PhoneNumber.Contains(searchTerm) ||
                    x.Subject.Contains(searchTerm) ||
                    x.Message.Contains(searchTerm)
                );
            }

            if (searchDto.FromDate.HasValue)
            {
                query = query.Where(x => x.CreatedOn >= searchDto.FromDate.Value);
            }

            if (searchDto.ToDate.HasValue)
            {
                var toDate = searchDto.ToDate.Value;
                if (toDate.TimeOfDay == TimeSpan.Zero)
                {
                    toDate = toDate.Date.AddDays(1).AddTicks(-1);
                }
                query = query.Where(x => x.CreatedOn <= toDate);
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<ContactInquiryResponseDto>(items, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<ContactInquiryResponseDto>>.SuccessAsync("Contact inquiries retrieved successfully.", pagedResult);
        }

        public async Task<Result<ContactInquiryResponseDto>> GetById(long id)
        {
            var entity = await context.ContactInquiries
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<ContactInquiryResponseDto>.RecordNotFoundAsync("Contact inquiry not found.");
            }

            return await Result<ContactInquiryResponseDto>.SuccessAsync("Contact inquiry retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(ContactInquiryRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return await Result<long>.BadRequestAsync("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                return await Result<long>.BadRequestAsync("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return await Result<long>.BadRequestAsync("Email is required.");
            }

            if (!Helper.IsValidEmail(dto.Email))
            {
                return await Result<long>.BadRequestAsync("Invalid email address format.");
            }

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return await Result<long>.BadRequestAsync("Phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                return await Result<long>.BadRequestAsync("Message is required.");
            }

            var newEntity = new ContactInquiryEntity
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                PhoneNumber = dto.PhoneNumber.Trim(),
                Subject = dto.Subject?.Trim() ?? string.Empty,
                Message = dto.Message.Trim(),
                Status = Status.Active
            };

            context.ContactInquiries.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Contact inquiry submitted successfully.", newEntity.Id);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.ContactInquiries
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Contact inquiry not found.");
            }

            entity.Status = Status.Deleted;
            context.ContactInquiries.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Contact inquiry successfully deleted.", true);
        }

        private static ContactInquiryResponseDto MapToDto(ContactInquiryEntity x)
        {
            return new ContactInquiryResponseDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Subject = x.Subject,
                Message = x.Message,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}

