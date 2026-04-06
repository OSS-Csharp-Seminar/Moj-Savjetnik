using MyAdvisor.Application.DTOs.Category;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Application.Interfaces.Services.Domain;
using MyAdvisor.Application.Mappers;

namespace MyAdvisor.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, CategoryMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(_mapper.ToDto).ToList();
        }
    }
}
