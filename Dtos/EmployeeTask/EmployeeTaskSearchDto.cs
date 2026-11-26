using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.EmployeeTask
{
    public class EmployeeTaskSearchDto : PaginationDto
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
