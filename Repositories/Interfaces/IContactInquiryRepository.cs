using ZamZamAPI.Dtos.ContactInquiry;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IContactInquiryRepository
    {
        Task<Result<PagedResult<ContactInquiryResponseDto>>> GetAll(ContactInquirySearchDto searchDto);
        Task<Result<ContactInquiryResponseDto>> GetById(long id);
        Task<Result<long>> Create(ContactInquiryRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

