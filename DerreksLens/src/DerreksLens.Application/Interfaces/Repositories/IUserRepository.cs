using System.Threading.Tasks;
using DerreksLens.Core.Domain.Entities;

namespace DerreksLens.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}