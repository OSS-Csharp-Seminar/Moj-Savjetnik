using MyAdvisor.Application.DTOs.User;

namespace MyAdvisor.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<UserDto> CreateAsync(string username, string email);
    }
}
