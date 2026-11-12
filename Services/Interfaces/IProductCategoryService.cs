using ZamZamAPI.Dtos.ProductCategory;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<Result<PagedResult<ProductCategoryResponseDto>>> GetAll(ProductCategorySearchDto searchDto);
        Task<Result<List<ProductCategoryResponseDto>>> GetAllForDisplay();
        Task<Result<ProductCategoryResponseDto>> GetById(long id);
        Task<Result<List<ProductCategoryResponseDto>>> GetByName(string name);
        Task<Result<long>> Create(ProductCategoryRequestDto dto);
        Task<Result<ProductCategoryResponseDto>> Update(ProductCategoryRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

