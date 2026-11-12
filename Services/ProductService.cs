using ZamZamAPI.Dtos.Product;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository productRepository = productRepository;

        public async Task<Result<PagedResult<ProductResponseDto>>> GetAll(ProductSearchDto searchDto)
        {
            return await productRepository.GetAll(searchDto);
        }

        public async Task<Result<List<ProductResponseDto>>> GetAllForDisplay(long categoryId)
        {
            return await productRepository.GetAllForDisplay(categoryId);
        }

        public async Task<Result<List<ProductResponseDto>>> GetAllForDisplay(string searchTerm)
        {
            return await productRepository.GetAllForDisplay(searchTerm);
        }

        public async Task<Result<ProductResponseDto>> GetById(long id)
        {
            return await productRepository.GetById(id);
        }

        public async Task<Result<long>> Create(ProductRequestDto dto)
        {
            return await productRepository.Create(dto);
        }

        public async Task<Result<ProductResponseDto>> Update(ProductRequestDto dto)
        {
            return await productRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await productRepository.Delete(id);
        }
    }
}

