using LocalCinema.Core.Services.Interfaces;
using LocalCinema.Data.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using LocalCinema.Data.Repository.Interfaces;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace LocalCinema.Core.Services
{
    public class CinemaCatalogManager : ICinemaCatalogManger
    {
        private readonly ILocalCinemaRepo _localCinemaRepo;

        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;

        private readonly IUnitOfWork _unitOfWork;

        private readonly KeyManager _keyManager;
        public CinemaCatalogManager(ILogger<CinemaCatalogManager> logger, ILocalCinemaRepo localCinemaRepo, IUnitOfWork unitOfWork,HttpClient httpClient,IOptions<KeyManager> keys)
        {
            _localCinemaRepo = localCinemaRepo;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _keyManager = keys.Value;
        }

        public async Task<bool> GetImdbClientAsyncId(string id)
        {
            var httpResponse = await _httpClient.GetAsync($"{_keyManager.Uri}/?i={id}&apikey={_keyManager.AccessToken}");
            if (!httpResponse.IsSuccessStatusCode)
                return false;
            else
                return true;
        }

        public async Task<OperationResult> UpdateMoviePriceAndTime(string id, UpdateMovieCatalog command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            _logger.LogInformation($"incoming Request: {command}");

            var isValidProperties = new List<ValidationResult>();
            var operationResult = new OperationResult<UpdateMovieCatalog>();
            if (command != null && !Validator.TryValidateObject(command, new ValidationContext(command), isValidProperties))
            {
                operationResult.AddErrors(isValidProperties);
            }
            if (!operationResult.IsValid)
                return operationResult;

            await _unitOfWork.ExecuteWithTransactionAsync(async (conn, Commit, Rollback) =>
            {
                try
                {
                    if (!(await GetImdbClientAsyncId(id)))
                    {
                        var notFoundError = new OperationError(ErrorCodes.NotFound,
                            $"Invalid title was provided: {id}");

                        operationResult.AddError(notFoundError);
                        return;
                    }

                    await _localCinemaRepo.UpdateMoviePriceAndTime(id, command);

                    Commit();
                }
                catch (OperationException opEx)
                {
                    operationResult.AddErrorsFromException(opEx);
                }
            });
            return operationResult;
        }
    }


}
