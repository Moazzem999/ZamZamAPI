using ZamZamAPI.Dtos.Product;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<PagedResult<ProductResponseDto>>> GetAll(ProductSearchDto searchDto);
        Task<Result<List<ProductResponseDto>>> GetAllForDisplay(long categoryId);
        Task<Result<ProductResponseDto>> GetById(long id);
        Task<Result<long>> Create(ProductRequestDto dto);
        Task<Result<ProductResponseDto>> Update(ProductRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

