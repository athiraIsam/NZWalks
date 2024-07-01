using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
        Task<Walk> GetById(Guid id);
        Task<Walk?> UpdateAsync (Guid id, Walk walk);
        Task<Walk?> DeleteAsync (Guid id);
    }
}
