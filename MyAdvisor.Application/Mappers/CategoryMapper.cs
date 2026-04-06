using MyAdvisor.Application.DTOs.Category;
using MyAdvisor.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MyAdvisor.Application.Mappers
{
    [Mapper]
    public partial class CategoryMapper
    {
        public partial CategoryDto ToDto(Category category);
    }
}
