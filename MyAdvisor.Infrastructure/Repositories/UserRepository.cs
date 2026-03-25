using Microsoft.EntityFrameworkCore;
using MyAdvisor.Application.Interfaces.Repositories;
using MyAdvisor.Domain.Entities;
using MyAdvisor.Infrastructure.Persistence;

namespace MyAdvisor.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<User?> GetByIdAsync(int id)
            => _db.DomainUsers.FirstOrDefaultAsync(u => u.Id == id);

        public Task<User?> GetByEmailAsync(string email)
            => _db.DomainUsers.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            await _db.DomainUsers.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.DomainUsers.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
