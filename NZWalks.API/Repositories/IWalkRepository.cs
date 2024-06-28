using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync();
        Task<Walk> GetById(Guid id);
        Task<Walk?> UpdateAsync (Guid id, Walk walk);
        Task<Walk?> DeleteAsync (Guid id);
    }
}
