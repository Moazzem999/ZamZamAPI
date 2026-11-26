using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.EmployeeTask;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Services;

namespace ZamZamAPI.Repositories
{
    public class EmployeeTaskRepository(AppDbContext context
        , ICurrentUserService currentUserService) : IEmployeeTaskRepository
    {
        private readonly AppDbContext context = context;
        private readonly ICurrentUserService currentUserService = currentUserService;

        public async Task<Result<PagedResult<EmployeeTaskResponseDto>>> GetAll(EmployeeTaskSearchDto searchDto)
        {
            var query = context.EmployeeTasks.AsNoTracking()
                .Include(x => x.Employee)
                .Where(x => x.Status == Status.Active);

            if (string.Equals(currentUserService.RoleName, "Employee", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.EmployeeId == currentUserService.EmployeeId);
            }

            return await GetPagedTasks(query, searchDto);
        }

        public async Task<Result<EmployeeTaskResponseDto>> GetById(long id)
        {
            var entity = await context.EmployeeTasks.AsNoTracking()
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<EmployeeTaskResponseDto>.RecordNotFoundAsync("Employee task not found.");
            }

            return await Result<EmployeeTaskResponseDto>.SuccessAsync("Employee task retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(EmployeeTaskRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<long>.BadRequestAsync("Task title is required.");
            }

            var employeeExists = await context.Employees.AsNoTracking()
                .AnyAsync(e => e.Id == currentUserService.EmployeeId && e.Status == Status.Active);

            if (!employeeExists)
            {
                return await Result<long>.RecordNotFoundAsync("Employee not found.");
            }

            var newEntity = new EmployeeTaskEntity
            {
                EmployeeId = currentUserService.EmployeeId.Value,
                Title = dto.Title.Trim(),
                Description = dto.Description,
                ScheduledOn = dto.ScheduledOn
            };

            context.EmployeeTasks.Add(newEntity);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Employee task successfully created.", newEntity.Id);
        }

        public async Task<Result<EmployeeTaskResponseDto>> Update(EmployeeTaskRequestDto dto)
        {
            if (dto == null)
            {
                return await Result<EmployeeTaskResponseDto>.BadRequestAsync("Invalid request data.");
            }

            var entity = await context.EmployeeTasks
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<EmployeeTaskResponseDto>.RecordNotFoundAsync("Employee task not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<EmployeeTaskResponseDto>.BadRequestAsync("Task title is required.");
            }

            entity.Title = dto.Title.Trim();
            entity.Description = dto.Description;
            entity.ScheduledOn = dto.ScheduledOn;

            context.EmployeeTasks.Update(entity);
            await context.SaveChangesAsync();

            return await Result<EmployeeTaskResponseDto>.SuccessAsync("Employee task successfully updated.", MapToDto(entity));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.EmployeeTasks
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Employee task not found.");
            }

            entity.Status = Status.Deleted;

            context.EmployeeTasks.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Employee task successfully deleted.", true);
        }

        private static async Task<Result<PagedResult<EmployeeTaskResponseDto>>> GetPagedTasks(
            IQueryable<EmployeeTaskEntity> query,
            EmployeeTaskSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.Title.Contains(searchTerm) ||
                    (x.Description != null && x.Description.Contains(searchTerm)) ||
                    (x.Employee != null && x.Employee.Name.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tasks = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<EmployeeTaskResponseDto>(tasks, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<EmployeeTaskResponseDto>>.SuccessAsync("Employee tasks retrieved successfully.", pagedResult);
        }

        private static EmployeeTaskResponseDto MapToDto(EmployeeTaskEntity x)
        {
            return new EmployeeTaskResponseDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.Employee?.Name ?? string.Empty,
                Title = x.Title,
                Description = x.Description,
                ScheduledOn = x.ScheduledOn,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
