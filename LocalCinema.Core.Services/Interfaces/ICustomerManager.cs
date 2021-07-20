using LocalCinema.Data.Model;
using System.Threading.Tasks;

namespace LocalCinema.Core.Services.Interfaces
{
    public interface ICustomerManager
    {
        Task<OperationResult> GetMovieTimes(string movieName,string date);
        Task<OperationResult> GetMovieDetails(string movieName);
    }
}
