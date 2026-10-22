using ZamZamAPI.Dtos.QuotationRequest;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class QuotationRequestService(IQuotationRequestRepository quotationRequestRepository) : IQuotationRequestService
    {
        private readonly IQuotationRequestRepository quotationRequestRepository = quotationRequestRepository;

        public async Task<Result<PagedResult<QuotationRequestResponseDto>>> GetAll(QuotationRequestSearchDto searchDto)
        {
            return await quotationRequestRepository.GetAll(searchDto);
        }

        public async Task<Result<QuotationRequestResponseDto>> GetById(long id)
        {
            return await quotationRequestRepository.GetById(id);
        }

        public async Task<Result<long>> Create(QuotationRequestRequestDto dto)
        {
            return await quotationRequestRepository.Create(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await quotationRequestRepository.Delete(id);
        }
    }
}
