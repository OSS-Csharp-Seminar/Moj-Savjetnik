using MyAdvisor.Application.DTOs.User;
using MyAdvisor.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MyAdvisor.Application.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapperIgnoreSource(nameof(User.CreatedAt))]
        public partial UserDto ToDto(User user);
    }
}
