using System.Collections.Generic;
using System.Threading.Tasks;
using DerreksLens.Core.Domain.Entities;

namespace DerreksLens.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetBySlugAsync(string slug);
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetRecentPostsAsync(int count);
        Task<IEnumerable<Post>> GetAllPublishedAsync();
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Post post);
        Task SaveChangesAsync();
    }
}