using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DerreksLens.Application.Interfaces.Repositories;
using DerreksLens.Core.Domain.Entities;
using DerreksLens.Core.Domain.Enums;
using DerreksLens.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DerreksLens.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetBySlugAsync(string slug)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments) // Basic include, pagination handled later
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<Post>> GetRecentPostsAsync(int count)
        {
            return await _context.Posts
                .Where(p => p.Status == PostStatus.Published)
                .Include(p => p.Author)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPublishedAsync()
        {
            return await _context.Posts
               .Where(p => p.Status == PostStatus.Published)
               .Include(p => p.Category)
               .OrderByDescending(p => p.CreatedAt)
               .ToListAsync();
        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
        }

        public Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Post post)
        {
            _context.Posts.Remove(post);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}