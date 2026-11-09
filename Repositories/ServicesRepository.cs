using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZamZamAPI.Dtos.Service;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class ServicesRepository(
        AppDbContext context,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor) : IServicesRepository
    {
        private readonly AppDbContext context = context;
        private readonly IWebHostEnvironment environment = environment;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        public async Task<Result<PagedResult<ServiceResponseDto>>> GetAll(ServiceSearchDto searchDto)
        {
            var pageNumber = searchDto.PageNumber < 1 ? 1 : searchDto.PageNumber;
            var pageSize = searchDto.PageSize < 1 ? 10 : searchDto.PageSize;

            var query = context.Services
                .AsNoTracking()
                .Include(x => x.ServiceHighlights)
                .Where(x => x.Status == Status.Active);

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.Trim();
                query = query.Where(x =>
                    x.Title.Contains(searchTerm) ||
                    x.Description.Contains(searchTerm) ||
                    x.ServiceHighlights.Any(h => h.Status == Status.Active && h.HighlightText.Contains(searchTerm))
                );
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            var pagedResult = new PagedResult<ServiceResponseDto>(dtoList, totalCount, pageNumber, pageSize);

            return await Result<PagedResult<ServiceResponseDto>>.SuccessAsync("Services retrieved successfully.", pagedResult);
        }

        public async Task<Result<List<ServiceResponseDto>>> GetAllForDisplay()
        {
            var data = await context.Services
                .AsNoTracking()
                .Include(x => x.ServiceHighlights)
                .Where(x => x.Status == Status.Active)
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.Id)
                .ToListAsync();

            var dtoList = data.Select(MapToDto).ToList();
            return await Result<List<ServiceResponseDto>>.SuccessAsync("Services retrieved successfully.", dtoList);
        }

        public async Task<Result<ServiceResponseDto>> GetById(long id)
        {
            var entity = await context.Services
                .AsNoTracking()
                .Include(x => x.ServiceHighlights)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<ServiceResponseDto>.RecordNotFoundAsync("Service not found.");
            }

            return await Result<ServiceResponseDto>.SuccessAsync("Service retrieved successfully.", MapToDto(entity));
        }

        public async Task<Result<long>> Create(ServiceRequestDto dto)
        {
            var maxFileSize = 300 * 1024; // 300 KB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png"];

            if (dto == null)
            {
                return await Result<long>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<long>.BadRequestAsync("Service title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return await Result<long>.BadRequestAsync("Service description is required.");
            }

            if (dto.Image == null || dto.Image.Length == 0)
            {
                return await Result<long>.BadRequestAsync("Image file is required.");
            }

            if (dto.DisplayOrder <= 0)
            {
                return await Result<long>.BadRequestAsync("Display order is required.");
            }

            string imagePath = string.Empty;
            if (dto.Image != null)
            {
                var fileType = Path.GetExtension(dto.Image.FileName).ToLower();

                if (dto.Image.Length > maxFileSize)
                    return await Result<long>.BadRequestAsync("Image size must be less than 300 KB.");

                if (string.IsNullOrEmpty(fileType) || !allowedFileTypes.Contains(fileType))
                    return await Result<long>.BadRequestAsync("Invalid file type. Allowed types: .jpg, .jpeg, .png");

                imagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "Services");
            }

            var service = new ServiceEntity
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim() ?? string.Empty,
                ImagePath = imagePath,
                DisplayOrder = dto.DisplayOrder,
                Status = Status.Active
            };

            PopulateHighlightsFromDto(dto);

            if (dto.Highlights != null && dto.Highlights.Count > 0)
            {
                foreach (var h in dto.Highlights)
                {
                    if (!string.IsNullOrWhiteSpace(h.HighlightText))
                    {
                        service.ServiceHighlights.Add(new ServiceHighlightEntity
                        {
                            HighlightText = h.HighlightText.Trim(),
                            DisplayOrder = h.DisplayOrder,
                            Status = Status.Active
                        });
                    }
                }
            }

            context.Services.Add(service);
            await context.SaveChangesAsync();

            return await Result<long>.SuccessAsync("Service successfully created.", service.Id);
        }

        public async Task<Result<ServiceResponseDto>> Update(ServiceRequestDto dto)
        {
            var maxFileSize = 300 * 1024; // 300 KB
            string[] allowedFileTypes = [".jpg", ".jpeg", ".png"];

            if (dto == null || dto.Id <= 0)
            {
                return await Result<ServiceResponseDto>.BadRequestAsync("Invalid request data.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return await Result<ServiceResponseDto>.BadRequestAsync("Service title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return await Result<ServiceResponseDto>.BadRequestAsync("Service description is required.");
            }

            if (dto.Image == null || dto.Image.Length == 0)
            {
                return await Result<ServiceResponseDto>.BadRequestAsync("Image file is required.");
            }

            if (dto.DisplayOrder <= 0)
            {
                return await Result<ServiceResponseDto>.BadRequestAsync("Display order is required.");
            }

            var service = await context.Services
                .Include(x => x.ServiceHighlights)
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Status == Status.Active);

            if (service == null)
            {
                return await Result<ServiceResponseDto>.RecordNotFoundAsync("Service not found.");
            }

            if (dto.Image != null)
            {
                var fileType = Path.GetExtension(dto.Image.FileName).ToLower();

                if (dto.Image.Length > maxFileSize)
                    return await Result<ServiceResponseDto>.BadRequestAsync("Image size must be less than 300 KB.");

                if (string.IsNullOrEmpty(fileType) || !allowedFileTypes.Contains(fileType))
                    return await Result<ServiceResponseDto>.BadRequestAsync("Invalid file type. Allowed types: .jpg, .jpeg, .png");

                if (!string.IsNullOrEmpty(service.ImagePath))
                {
                    Helper.DeleteImage(environment.ContentRootPath, service.ImagePath);
                }

                service.ImagePath = await Helper.SaveImage(environment.ContentRootPath, dto.Image, "Services");
            }

            service.Title = dto.Title.Trim();
            service.Description = dto.Description?.Trim() ?? string.Empty;
            service.DisplayOrder = dto.DisplayOrder;

            PopulateHighlightsFromDto(dto);

            var incomingHighlights = dto.Highlights ?? new List<ServiceHighlightRequestDto>();
            var incomingValidHighlights = incomingHighlights.Where(h => !string.IsNullOrWhiteSpace(h.HighlightText)).ToList();
            var incomingIds = incomingValidHighlights.Where(h => h.Id > 0).Select(h => h.Id).ToHashSet();

            // Mark existing highlights that are not in the incoming request as Deleted
            foreach (var existing in service.ServiceHighlights.Where(h => h.Status == Status.Active))
            {
                if (!incomingIds.Contains(existing.Id))
                {
                    existing.Status = Status.Deleted;
                }
            }

            // Update existing or add new highlights
            foreach (var hDto in incomingValidHighlights)
            {
                if (hDto.Id > 0)
                {
                    var existing = service.ServiceHighlights.FirstOrDefault(x => x.Id == hDto.Id);
                    if (existing != null)
                    {
                        existing.HighlightText = hDto.HighlightText.Trim();
                        existing.DisplayOrder = hDto.DisplayOrder;
                        existing.Status = Status.Active;
                    }
                    else
                    {
                        service.ServiceHighlights.Add(new ServiceHighlightEntity
                        {
                            ServiceId = service.Id,
                            HighlightText = hDto.HighlightText.Trim(),
                            DisplayOrder = hDto.DisplayOrder,
                            Status = Status.Active
                        });
                    }
                }
                else
                {
                    service.ServiceHighlights.Add(new ServiceHighlightEntity
                    {
                        ServiceId = service.Id,
                        HighlightText = hDto.HighlightText.Trim(),
                        DisplayOrder = hDto.DisplayOrder,
                        Status = Status.Active
                    });
                }
            }

            context.Services.Update(service);
            await context.SaveChangesAsync();

            return await Result<ServiceResponseDto>.SuccessAsync("Service successfully updated.", MapToDto(service));
        }

        public async Task<Result<bool>> Delete(long id)
        {
            var entity = await context.Services
                .Include(x => x.ServiceHighlights)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);

            if (entity == null)
            {
                return await Result<bool>.RecordNotFoundAsync("Service not found.");
            }

            if (!string.IsNullOrEmpty(entity.ImagePath))
            {
                Helper.DeleteImage(environment.ContentRootPath, entity.ImagePath);
            }

            entity.ImagePath = null;
            entity.Status = Status.Deleted;

            foreach (var highlight in entity.ServiceHighlights.Where(h => h.Status == Status.Active))
            {
                highlight.Status = Status.Deleted;
            }

            context.Services.Update(entity);
            await context.SaveChangesAsync();

            return await Result<bool>.SuccessAsync("Service successfully deleted.", true);
        }

        private void PopulateHighlightsFromDto(ServiceRequestDto dto)
        {
            // If dto.Highlights already contains valid items with highlight text, nothing to do
            if (dto.Highlights != null && dto.Highlights.Any(h => !string.IsNullOrWhiteSpace(h.HighlightText)))
            {
                return;
            }

            dto.Highlights ??= new List<ServiceHighlightRequestDto>();
            dto.Highlights.Clear();

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // 1. Check HighlightsJson property on DTO
            if (!string.IsNullOrWhiteSpace(dto.HighlightsJson))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<List<ServiceHighlightRequestDto>>(dto.HighlightsJson, jsonOptions);
                    if (parsed != null && parsed.Count > 0)
                    {
                        dto.Highlights.AddRange(parsed);
                        return;
                    }
                }
                catch
                {
                    // Ignore parsing error
                }
            }

            // 2. Read from multipart form if available (handles Swagger UI array<object> and frontend FormData)
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Request.HasFormContentType)
            {
                var form = httpContext.Request.Form;

                // Check "Highlights" or "highlights"
                var formKey = form.Keys.FirstOrDefault(k => string.Equals(k, "Highlights", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(formKey))
                {
                    var values = form[formKey];
                    foreach (var rawVal in values)
                    {
                        if (string.IsNullOrWhiteSpace(rawVal)) continue;

                        var trimmed = rawVal.Trim();

                        // Case A: Full array string "[{...}, {...}]"
                        if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
                        {
                            try
                            {
                                var parsedList = JsonSerializer.Deserialize<List<ServiceHighlightRequestDto>>(trimmed, jsonOptions);
                                if (parsedList != null)
                                {
                                    dto.Highlights.AddRange(parsedList);
                                }
                            }
                            catch { }
                        }
                        // Case B: Single JSON object string "{"id": 1, ...}" (sent by Swagger UI for each array item)
                        else if (trimmed.StartsWith('{') && trimmed.EndsWith('}'))
                        {
                            try
                            {
                                var parsedItem = JsonSerializer.Deserialize<ServiceHighlightRequestDto>(trimmed, jsonOptions);
                                if (parsedItem != null)
                                {
                                    dto.Highlights.Add(parsedItem);
                                }
                            }
                            catch { }
                        }
                    }
                }

                // If still empty, check form field "HighlightsJson"
                if (dto.Highlights.Count == 0)
                {
                    var jsonKey = form.Keys.FirstOrDefault(k => string.Equals(k, "HighlightsJson", StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrEmpty(jsonKey))
                    {
                        var jsonVal = form[jsonKey].ToString();
                        if (!string.IsNullOrWhiteSpace(jsonVal))
                        {
                            try
                            {
                                var parsedList = JsonSerializer.Deserialize<List<ServiceHighlightRequestDto>>(jsonVal, jsonOptions);
                                if (parsedList != null)
                                {
                                    dto.Highlights.AddRange(parsedList);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private static ServiceResponseDto MapToDto(ServiceEntity x)
        {
            return new ServiceResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImagePath = x.ImagePath,
                DisplayOrder = x.DisplayOrder,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status,
                Highlights = x.ServiceHighlights != null
                    ? x.ServiceHighlights
                        .Where(h => h.Status == Status.Active)
                        .OrderBy(h => h.DisplayOrder)
                        .ThenBy(h => h.Id)
                        .Select(MapHighlightToDto)
                        .ToList()
                    : new List<ServiceHighlightResponseDto>()
            };
        }

        private static ServiceHighlightResponseDto MapHighlightToDto(ServiceHighlightEntity h)
        {
            return new ServiceHighlightResponseDto
            {
                Id = h.Id,
                ServiceId = h.ServiceId,
                HighlightText = h.HighlightText,
                DisplayOrder = h.DisplayOrder,
                CreatedOn = h.CreatedOn,
                UpdatedOn = h.UpdatedOn,
                CreatedBy = h.CreatedBy,
                UpdatedBy = h.UpdatedBy,
                Status = h.Status
            };
        }
    }
}

