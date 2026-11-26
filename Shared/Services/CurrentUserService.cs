namespace ZamZamAPI.Shared.Services
{
    public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor contextAccessor = contextAccessor;

        private string? GetClaimValue(string claimType)
        {
            return CurrentContext?.User?.Claims?.FirstOrDefault(x => x.Type == claimType)?.Value;
        }

        private long? GetNullableLongClaim(string claimType)
        {
            var value = GetClaimValue(claimType);
            return long.TryParse(value, out var parsed) ? parsed : null;
        }

        private int GetUserId()
        {
            var userId = GetClaimValue("UserId");

            if (userId is not null)
                return Convert.ToInt32(userId);

            return -1;
        }

        public HttpContext CurrentContext
        {
            get => GetCurrentContext();
        }

        private HttpContext GetCurrentContext()
        {
            return contextAccessor.HttpContext!;
        }

        public int UserId
        {
            get => GetUserId();
        }

        public long? RoleId => GetNullableLongClaim("RoleId");
        public string? RoleName => GetClaimValue("RoleName");
        public long? EmployeeId => GetNullableLongClaim("EmployeeId");

    }

    public interface ICurrentUserService
    {
        public int UserId { get; }
        public long? RoleId { get; }
        public string? RoleName { get; }
        public long? EmployeeId { get; }
    }
}
