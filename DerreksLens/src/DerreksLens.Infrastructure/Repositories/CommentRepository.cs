using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DerreksLens.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User) // Eager load Author
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}