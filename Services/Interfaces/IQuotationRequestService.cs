using ZamZamAPI.Dtos.QuotationRequest;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IQuotationRequestService
    {
        Task<Result<PagedResult<QuotationRequestResponseDto>>> GetAll(QuotationRequestSearchDto searchDto);
        Task<Result<QuotationRequestResponseDto>> GetById(long id);
        Task<Result<long>> Create(QuotationRequestRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
