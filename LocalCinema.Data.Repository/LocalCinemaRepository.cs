using LocalCinema.Data.Model;
using LocalCinema.Data.Repository.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Linq;
namespace LocalCinema.Data.Repository
{
    public class LocalCinemaRepository : ILocalCinemaRepo
    {
        private readonly IDbConnectionContext _dbConnectionContext;

        private readonly ILogger<LocalCinemaRepository> _logger;

        public LocalCinemaRepository(IDbConnectionContext dbConnectionContext, ILogger<LocalCinemaRepository> logger)
        {
            _dbConnectionContext = dbConnectionContext;
            _logger = logger;
        }

        public async Task<OperationResult> GetMovieTimes(string movieName, string date)
        {
            var opResult = new OperationResult();
            var operationError = new OperationError();
            try
            {
                const string sp_GetMovieTimes = @"sp_getMovieTimeSlots";
                var isValid = false;
                // select movieName,DateTime from dbo.Movies where movieName='@movieName' and movieData = '@date';
                using var conn = _dbConnectionContext.GetOpenConnection();
                var result = await conn.QueryAsync<MovieTimeSlots>(sp_GetMovieTimes, commandType: CommandType.StoredProcedure);

                isValid = (result != null && result.Count() > 0) ? true : false;

                if (!isValid)
                {
                    operationError.Code = ErrorCodes.InvalidInput.ToString();
                    operationError.Message = $"invalid movieName:{movieName} or timeSlot: {date}";

                }

                return new OperationResult
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task UpdateMoviePriceAndTime(string id, UpdateMovieCatalog updateMovieCatalog)
        {
            try
            {
                const string sp_UpdateMoviePriceAndTime = @"sp_UpdateMoviePriceAndTime";

                // update dbo.Movies set moviePrice = @price,movieTime = 'movietime' where id= @id;
                using var conn = _dbConnectionContext.GetOpenConnection();
                var param = new DynamicParameters();
                param.Add("id", id);
                param.Add("moviePrice", updateMovieCatalog.Price);
                param.Add("movieTime", updateMovieCatalog.dateTime);
                await conn.ExecuteAsync(sp_UpdateMoviePriceAndTime, param: param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }
    }
}
