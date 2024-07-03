using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image); 
    }
}
