using System.Collections.Generic;
using System.Threading.Tasks;
using DerreksLens.Core.Domain.Entities;

namespace DerreksLens.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(int postId);
        Task<Comment?> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task DeleteAsync(Comment comment); // Soft delete implementation
        Task SaveChangesAsync();
    }
}