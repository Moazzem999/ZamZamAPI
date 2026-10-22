using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.LocationHistory;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class LocationHistoryRepository(AppDbContext context) : ILocationHistoryRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<PagedResult<LocationHistoryResponseDto>>> GetAll(LocationHistorySearchDto searchDto)
        {
            if (searchDto.EmployeeId <= 0)
            {
                return await Result<PagedResult<LocationHistoryResponseDto>>.BadRequestAsync("Invalid Employee ID.");
            }

            if (searchDto.FromDate.HasValue && searchDto.ToDate.HasValue && searchDto.FromDate.Value > searchDto.ToDate.Value)
            {
                return await Result<PagedResult<LocationHistoryResponseDto>>.BadRequestAsync("FromDate cannot be greater than ToDate.");
            }

            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.LocationHistories
                .AsNoTracking()
                .Include(x => x.Employee)
                .Where(x => x.Status == Status.Active && x.EmployeeId == searchDto.EmployeeId);

            if (searchDto.FromDate.HasValue)
            {
                query = query.Where(x => x.RecordedAt >= searchDto.FromDate.Value);
            }

            if (searchDto.ToDate.HasValue)
            {
                var toDate = searchDto.ToDate.Value;
                if (toDate.TimeOfDay == TimeSpan.Zero)
                {
                    toDate = toDate.Date.AddDays(1).AddTicks(-1);
                }
                query = query.Where(x => x.RecordedAt <= toDate);
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.RecordedAt)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<LocationHistoryResponseDto>(items, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<LocationHistoryResponseDto>>.SuccessAsync("Location histories retrieved successfully.", pagedResult);
        }

        public async Task<Result<LocationHistoryResponseDto>> GetById(long id)
        {
            var entity = await context.LocationHistories
                .AsNoTracking()
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<LocationHistoryResponseDto>.RecordNotFoundAsync("Location history not found.");
            }

            return await Result<LocationHistoryResponseDto>.SuccessAsync("Location history retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(LocationHistoryRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (dto.EmployeeId <= 0)
            {
                return await Result<long>.BadRequestAsync("Employee is required.");
            }

            var employeeExists = await context.Employees
                .AnyAsync(x => x.Id == dto.EmployeeId 
                && x.DateOfBirth == dto.DateOfBirth 
                && x.Mobile == dto.Mobile 
                && x.Status == Status.Active);

            if (!employeeExists)
            {
                return await Result<long>.BadRequestAsync("Employee not found.");
            }

            var newEntity = new LocationHistoryEntity
            {
                EmployeeId = dto.EmployeeId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Accuracy = dto.Accuracy,
                Speed = dto.Speed,
                BatteryPercentage = dto.BatteryPercentage,
                RecordedAt = dto.RecordedAt == default ? DateTime.Now : dto.RecordedAt
            };

            context.LocationHistories.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Location history successfully created.", newEntity.Id);
        }

        private static LocationHistoryResponseDto MapToDto(LocationHistoryEntity x)
        {
            return new LocationHistoryResponseDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.Employee != null ? x.Employee.Name : null,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Accuracy = x.Accuracy,
                Speed = x.Speed,
                BatteryPercentage = x.BatteryPercentage,
                RecordedAt = x.RecordedAt,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
