using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Employee
{
    public class EmployeeSearchDto : PaginationDto
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
