using ZamZamAPI.Dtos.ContactInquiry;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class ContactInquiryService(IContactInquiryRepository contactInquiryRepository) : IContactInquiryService
    {
        private readonly IContactInquiryRepository contactInquiryRepository = contactInquiryRepository;

        public async Task<Result<PagedResult<ContactInquiryResponseDto>>> GetAll(ContactInquirySearchDto searchDto)
        {
            return await contactInquiryRepository.GetAll(searchDto);
        }

        public async Task<Result<ContactInquiryResponseDto>> GetById(long id)
        {
            return await contactInquiryRepository.GetById(id);
        }

        public async Task<Result<long>> Create(ContactInquiryRequestDto dto)
        {
            return await contactInquiryRepository.Create(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await contactInquiryRepository.Delete(id);
        }
    }
}

