using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.QuotationRequest;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class QuotationRequestRepository(AppDbContext context) : IQuotationRequestRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<QuotationRequestResponseDto>>> GetAll(QuotationRequestSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.QuotationRequests
                .AsNoTracking()
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.FullName.Contains(searchTerm) ||
                    x.Email.Contains(searchTerm) ||
                    x.Phone.Contains(searchTerm) ||
                    x.CompanyName.Contains(searchTerm) ||
                    x.ProductReference.Contains(searchTerm) ||
                    x.EstimatedQuantity.Contains(searchTerm)
                );
            }

            if (searchDto.ServiceCategory.HasValue)
            {
                query = query.Where(x => x.ServiceCategory == searchDto.ServiceCategory.Value);
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
            var pagedResult = new PagedResult<QuotationRequestResponseDto>(items, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<QuotationRequestResponseDto>>.SuccessAsync("Quotation requests retrieved successfully.", pagedResult);
        }

        public async Task<Result<QuotationRequestResponseDto>> GetById(long id)
        {
            var entity = await context.QuotationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<QuotationRequestResponseDto>.RecordNotFoundAsync("Quotation request not found.");
            }

            return await Result<QuotationRequestResponseDto>.SuccessAsync("Quotation request retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(QuotationRequestRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return await Result<long>.BadRequestAsync("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return await Result<long>.BadRequestAsync("Email is required.");
            }

            if (!Helper.IsValidEmail(dto.Email))
            {
                return await Result<long>.BadRequestAsync("Invalid email address format.");
            }

            if (string.IsNullOrWhiteSpace(dto.Phone))
            {
                return await Result<long>.BadRequestAsync("Phone is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.ServiceCategory.ToString()))
            {
                return await Result<long>.BadRequestAsync("Service category is required.");
            }

            var newEntity = new QuotationRequestEntity
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone.Trim(),
                CompanyName = dto.CompanyName?.Trim() ?? string.Empty,
                ServiceCategory = dto.ServiceCategory,
                EstimatedQuantity = dto.EstimatedQuantity?.Trim() ?? string.Empty,
                ProductReference = dto.ProductReference?.Trim() ?? string.Empty,
                DetailedSpecifications = dto.DetailedSpecifications?.Trim() ?? string.Empty
            };

            context.QuotationRequests.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Quotation request successfully created.", newEntity.Id);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.QuotationRequests
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Quotation request not found.");
            }

            entity.Status = Status.Deleted;

            context.QuotationRequests.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Quotation request successfully deleted.", true);
        }

        private static QuotationRequestResponseDto MapToDto(QuotationRequestEntity x)
        {
            return new QuotationRequestResponseDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                CompanyName = x.CompanyName,
                ServiceCategory = x.ServiceCategory,
                ServiceCategoryName = x.ServiceCategory.GetEnumDescription(),
                EstimatedQuantity = x.EstimatedQuantity,
                ProductReference = x.ProductReference,
                DetailedSpecifications = x.DetailedSpecifications,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
