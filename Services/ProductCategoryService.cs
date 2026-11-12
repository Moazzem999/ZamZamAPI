using ZamZamAPI.Dtos.ProductCategory;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class ProductCategoryService(IProductCategoryRepository productCategoryRepository) : IProductCategoryService
    {
        private readonly IProductCategoryRepository productCategoryRepository = productCategoryRepository;

        public async Task<Result<PagedResult<ProductCategoryResponseDto>>> GetAll(ProductCategorySearchDto searchDto)
        {
            return await productCategoryRepository.GetAll(searchDto);
        }

        public async Task<Result<List<ProductCategoryResponseDto>>> GetAllForDisplay()
        {
            return await productCategoryRepository.GetAllForDisplay();
        }

        public async Task<Result<ProductCategoryResponseDto>> GetById(long id)
        {
            return await productCategoryRepository.GetById(id);
        }

        public async Task<Result<List<ProductCategoryResponseDto>>> GetByName(string name)
        {
            return await productCategoryRepository.GetByName(name);
        }

        public async Task<Result<long>> Create(ProductCategoryRequestDto dto)
        {
            return await productCategoryRepository.Create(dto);
        }

        public async Task<Result<ProductCategoryResponseDto>> Update(ProductCategoryRequestDto dto)
        {
            return await productCategoryRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await productCategoryRepository.Delete(id);
        }
    }
}

