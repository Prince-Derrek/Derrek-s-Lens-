using System.Threading.Tasks;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DerreksLens.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            // In a Unit of Work pattern, SaveChanges is usually separate. 
            // For this phase, we save immediately or allow the service to call Save.
            // We'll expose SaveChanges in the interface (done in Phase 2).
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}