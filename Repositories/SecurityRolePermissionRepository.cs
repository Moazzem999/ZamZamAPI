using Microsoft.EntityFrameworkCore;
using ZamZamAPI.Dtos.SecurityRolePermission;
using ZamZamAPI.Entities;
using ZamZamAPI.Entities.Context;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories
{
    public class SecurityRolePermissionRepository(AppDbContext context) : ISecurityRolePermissionRepository
    {
        private readonly AppDbContext context = context;

        public async Task<Result<List<SecurityRolePermissionResponseDto>>> GetAll(SecurityRolePermissionSearchDto searchDto)
        {
            var query = context.SecurityRolePermissions.AsNoTracking()
                .Include(x => x.Role)
                .Include(x => x.SecurityManu)
                .Where(x => x.Status == Status.Active);

            if (searchDto.RoleId.HasValue && searchDto.RoleId.Value > 0)
            {
                query = query.Where(x => x.RoleId == searchDto.RoleId.Value);
            }

            if (searchDto.SecurityManuId.HasValue && searchDto.SecurityManuId.Value > 0)
            {
                query = query.Where(x => x.SecurityManuId == searchDto.SecurityManuId.Value);
            }

            var data = await query
                .OrderBy(x => x.RoleId)
                .ThenBy(x => x.SecurityManu.Sort ?? int.MaxValue)
                .ThenBy(x => x.SecurityManuId)
                .ToListAsync();

            var permissions = data.Select(MapToDto).ToList();
            return await Result<List<SecurityRolePermissionResponseDto>>.SuccessAsync("Security role permissions retrieved successfully.", permissions);
        }

        public async Task<Result<List<long>>> Create(List<SecurityRolePermissionRequestDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
            {
                return await Result<List<long>>.BadRequestAsync("Permission list cannot be empty.");
            }

            if (dtos.Any(d => d.RoleId <= 0 || d.SecurityManuId <= 0))
            {
                return await Result<List<long>>.BadRequestAsync("Valid RoleId and SecurityManuId are required for all items.");
            }

            var duplicateInRequest = dtos
                .GroupBy(d => new { d.RoleId, d.SecurityManuId })
                .Any(g => g.Count() > 1);

            if (duplicateInRequest)
            {
                return await Result<List<long>>.BadRequestAsync("Duplicate RoleId and SecurityManuId entries found in request.");
            }

            var roleIds = dtos.Select(d => d.RoleId).Distinct().ToList();
            var validRoleCount = await context.Roles.AsNoTracking()
                .CountAsync(r => roleIds.Contains(r.Id) && r.Status == Status.Active);

            if (validRoleCount != roleIds.Count)
            {
                return await Result<List<long>>.RecordNotFoundAsync("One or more roles not found.");
            }

            var manuIds = dtos.Select(d => d.SecurityManuId).Distinct().ToList();
            var validManuCount = await context.SecurityManus.AsNoTracking()
                .CountAsync(m => manuIds.Contains(m.Id) && m.Status == Status.Active);

            if (validManuCount != manuIds.Count)
            {
                return await Result<List<long>>.RecordNotFoundAsync("One or more security menus not found.");
            }

            var existingPairs = await context.SecurityRolePermissions.AsNoTracking()
                .Where(x => x.Status == Status.Active && roleIds.Contains(x.RoleId) && manuIds.Contains(x.SecurityManuId))
                .Select(x => new { x.RoleId, x.SecurityManuId })
                .ToListAsync();

            if (dtos.Any(d => existingPairs.Any(e => e.RoleId == d.RoleId && e.SecurityManuId == d.SecurityManuId)))
            {
                return await Result<List<long>>.RecordAlreadyExistAsync("One or more role permissions already exist.");
            }

            var newEntities = dtos.Select(d => new SecurityRolePermissionEntity
            {
                RoleId = d.RoleId,
                SecurityManuId = d.SecurityManuId,
                IsView = d.IsView,
                IsInsert = d.IsInsert,
                IsModify = d.IsModify,
                IsDelete = d.IsDelete,
                IsReport = d.IsReport
            }).ToList();

            await context.SecurityRolePermissions.AddRangeAsync(newEntities);
            await context.SaveChangesAsync();

            var ids = newEntities.Select(e => e.Id).ToList();
            return await Result<List<long>>.SuccessAsync("Security role permissions successfully created.", ids);
        }

        public async Task<Result<List<SecurityRolePermissionResponseDto>>> Update(List<SecurityRolePermissionRequestDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
            {
                return await Result<List<SecurityRolePermissionResponseDto>>.BadRequestAsync("Permission list cannot be empty.");
            }

            if (dtos.Any(d => d.RoleId <= 0 || d.SecurityManuId <= 0))
            {
                return await Result<List<SecurityRolePermissionResponseDto>>.BadRequestAsync("Valid RoleId and SecurityManuId are required for all items.");
            }

            var duplicateInRequest = dtos
                .GroupBy(d => new { d.RoleId, d.SecurityManuId })
                .Any(g => g.Count() > 1);

            if (duplicateInRequest)
            {
                return await Result<List<SecurityRolePermissionResponseDto>>.BadRequestAsync("Duplicate RoleId and SecurityManuId entries found in request.");
            }

            var roleIds = dtos.Select(d => d.RoleId).Distinct().ToList();
            var validRoleCount = await context.Roles.AsNoTracking()
                .CountAsync(r => roleIds.Contains(r.Id) && r.Status == Status.Active);

            if (validRoleCount != roleIds.Count)
            {
                return await Result<List<SecurityRolePermissionResponseDto>>.RecordNotFoundAsync("One or more roles not found.");
            }

            var manuIds = dtos.Select(d => d.SecurityManuId).Distinct().ToList();
            var validManuCount = await context.SecurityManus.AsNoTracking()
                .CountAsync(m => manuIds.Contains(m.Id) && m.Status == Status.Active);

            if (validManuCount != manuIds.Count)
            {
                return await Result<List<SecurityRolePermissionResponseDto>>.RecordNotFoundAsync("One or more security menus not found.");
            }

            var requestedIds = dtos.Where(d => d.Id > 0).Select(d => d.Id).Distinct().ToList();
            var existingById = requestedIds.Count > 0
                ? await context.SecurityRolePermissions
                    .Where(x => requestedIds.Contains(x.Id) && x.Status == Status.Active)
                    .ToDictionaryAsync(x => x.Id)
                : new Dictionary<long, SecurityRolePermissionEntity>();

            if (requestedIds.Any(id => !existingById.ContainsKey(id)))
            {
                return await Result<List<SecurityRolePermissionResponseDto>>.RecordNotFoundAsync("One or more security role permissions not found.");
            }

            var existingByPair = await context.SecurityRolePermissions
                .Where(x => x.Status == Status.Active && roleIds.Contains(x.RoleId) && manuIds.Contains(x.SecurityManuId))
                .ToListAsync();

            var trackedEntities = new List<SecurityRolePermissionEntity>();

            foreach (var dto in dtos)
            {
                SecurityRolePermissionEntity? entity = null;

                if (dto.Id > 0)
                {
                    entity = existingById[dto.Id];
                }
                else
                {
                    entity = existingByPair.FirstOrDefault(x => x.RoleId == dto.RoleId && x.SecurityManuId == dto.SecurityManuId);
                }

                if (entity != null)
                {
                    entity.RoleId = dto.RoleId;
                    entity.SecurityManuId = dto.SecurityManuId;
                    entity.IsView = dto.IsView;
                    entity.IsInsert = dto.IsInsert;
                    entity.IsModify = dto.IsModify;
                    entity.IsDelete = dto.IsDelete;
                    entity.IsReport = dto.IsReport;

                    context.SecurityRolePermissions.Update(entity);
                    trackedEntities.Add(entity);
                }
                else
                {
                    var newEntity = new SecurityRolePermissionEntity
                    {
                        RoleId = dto.RoleId,
                        SecurityManuId = dto.SecurityManuId,
                        IsView = dto.IsView,
                        IsInsert = dto.IsInsert,
                        IsModify = dto.IsModify,
                        IsDelete = dto.IsDelete,
                        IsReport = dto.IsReport
                    };

                    await context.SecurityRolePermissions.AddAsync(newEntity);
                    trackedEntities.Add(newEntity);
                }
            }

            await context.SaveChangesAsync();

            var savedIds = trackedEntities.Select(e => e.Id).ToList();
            var updatedData = await context.SecurityRolePermissions.AsNoTracking()
                .Include(x => x.Role)
                .Include(x => x.SecurityManu)
                .Where(x => savedIds.Contains(x.Id))
                .OrderBy(x => x.RoleId)
                .ThenBy(x => x.SecurityManu.Sort ?? int.MaxValue)
                .ThenBy(x => x.SecurityManuId)
                .ToListAsync();

            var resultDtos = updatedData.Select(MapToDto).ToList();
            return await Result<List<SecurityRolePermissionResponseDto>>.SuccessAsync("Security role permissions successfully updated.", resultDtos);
        }

        private static SecurityRolePermissionResponseDto MapToDto(SecurityRolePermissionEntity x)
        {
            return new SecurityRolePermissionResponseDto
            {
                Id = x.Id,
                RoleId = x.RoleId,
                RoleName = x.Role?.Name ?? string.Empty,
                SecurityManuId = x.SecurityManuId,
                SecurityManuName = x.SecurityManu?.ManuName ?? string.Empty,
                ParentManuId = x.SecurityManu?.ParentManuId,
                IsView = x.IsView,
                IsInsert = x.IsInsert,
                IsModify = x.IsModify,
                IsDelete = x.IsDelete,
                IsReport = x.IsReport,
                CreatedOn = x.CreatedOn,
                UpdatedOn = x.UpdatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Status = x.Status
            };
        }
    }
}
