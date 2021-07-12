using LocalCinema.Core.Services.Interfaces;
using LocalCinema.Data.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using LocalCinema.Data.Repository.Interfaces;

namespace LocalCinema.Core.Services
{
    public class CinemaCatalogManager : ICinemaCatalogManger
    {
        private readonly ICinemaCatalogManger _cinemaCatalogManger;

        private readonly ILogger _logger;

        private readonly IUnitOfWork _unitOfWork;
        public CinemaCatalogManager(ILogger<CinemaCatalogManager> logger, ICinemaCatalogManger cinemaCatalogManger, IUnitOfWork unitOfWork)
        {
            _cinemaCatalogManger = cinemaCatalogManger;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public asyn Task<bool> GetImdbClientAsync(string id)
        {
            throw new NotImplementedException();
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
                    if (!(await _cinemaCatalogManger.GetImdbClientAsync(id)))
                    {
                        var notFoundError = new OperationError(ErrorCodes.NotFound,
                            $"Invalid title was provided: {id}");

                        operationResult.AddError(notFoundError);
                        return;
                    }

                    await _cinemaCatalogManger.UpdateMoviePriceAndTime(id, command);

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
