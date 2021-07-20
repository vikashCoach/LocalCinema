using LocalCinema.Core.Services.Interfaces;
using LocalCinema.Data.Model;
using LocalCinema.Data.Repository;
using LocalCinema.Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LocalCinema.Core.Services.Manager
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ILocalCinemaRepo _repo;

        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;

        private readonly IUnitOfWork _unitOfWork;

        private readonly KeyManager _keyManager;
        public CustomerManager(ILogger<CustomerManager> logger, ILocalCinemaRepo repo, IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<KeyManager> keys)
        {
            _repo = repo;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _keyManager = keys.Value;
        }

        public async Task<OperationResult> GetMovieDetails(string movieName)
        {
            var operationResult = new OperationResult();
            var httpResponse = new HttpResponseMessage();
            try
            {
                httpResponse = await _httpClient.GetAsync($"");

                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    operationResult.Data = JsonConvert.DeserializeObject<MovieCatalog>(content);
                }
                throw new Exception($"Bad Request");
            }
            catch (Exception ex)
            {
                _logger.LogError($"responseCode:{httpResponse.StatusCode}, {ex.Message}");
            }
            return operationResult;
        }

        public async Task<OperationResult> GetMovieTimes(string movieName, string date)
        {
            var operationResult = new OperationResult();
            if (!DateTime.TryParse(date, out DateTime dateTime))
            {
                operationResult.AddError(new OperationError { Message = "invalid date time" });
                return operationResult;
            }
            else
                return await _repo.GetMovieTimes(movieName, date);
        }
    }
}
