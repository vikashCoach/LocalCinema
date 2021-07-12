using LocalCinema.Data.Model;
using System.Threading.Tasks;

namespace LocalCinema.Core.Services.Interfaces
{
    public interface ICinemaCatalogManger
    {
        Task<OperationResult> UpdateMoviePriceAndTime(string id, UpdateMovieCatalog command);
        Task<bool> GetImdbClientAsync(string id);
    }
}
