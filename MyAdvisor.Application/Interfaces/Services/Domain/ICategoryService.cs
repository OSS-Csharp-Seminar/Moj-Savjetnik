using MyAdvisor.Application.DTOs.Category;

namespace MyAdvisor.Application.Interfaces.Services.Domain
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync();
    }
}
