using ZamZamAPI.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace ZamZamAPI.Entities.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options,
    ICurrentUserService currentUserService) : BaseDbContext(options, currentUserService)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BrandImageEntity> BrandImages { get; set; }
        public DbSet<AboutUsEntity> AboutUs { get; set; }
    }
}
