using System.Collections.Generic;
using System.Threading.Tasks;
using DerreksLens.Core.Domain.Entities;

namespace DerreksLens.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
    }
}