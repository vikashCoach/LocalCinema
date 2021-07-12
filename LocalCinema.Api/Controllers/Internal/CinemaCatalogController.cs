using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalCinema.Core.Services.Interfaces;
using LocalCinema.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LocalCinema.Api.Internal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaCatalogController : ControllerBase
    {
        

        private readonly ILogger<CinemaCatalogController> _logger;
        private readonly ICinemaCatalogManger _cinemaCatalogManger;

        public CinemaCatalogController(ILogger<CinemaCatalogController> logger,ICinemaCatalogManger cinemaCatalogManger)
        {
            _logger = logger;
            _cinemaCatalogManger = cinemaCatalogManger;
        }

        [HttpPatch("{imdbId:string}")]
        public async Task UpdateMoviePricendTime(string imdbId, [FromBody] UpdateMovieCatalog command)
        {
            await _cinemaCatalogManger.UpdateMoviePriceAndTime(imdbId, command);

            var operationResult = await _clientService.UpdateClientAsync(command);

            if (!operationResult.IsValid)
                return Error(operationResult.Errors);

            return NoContent();
        }
    }
}
