using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalCinema.Core.Services.Interfaces;
using LocalCinema.Data.Model;
using Microsoft.AspNetCore.Authorization;
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

        public CinemaCatalogController(ILogger<CinemaCatalogController> logger, ICinemaCatalogManger cinemaCatalogManger)
        {
            _logger = logger;
            _cinemaCatalogManger = cinemaCatalogManger;
        }
        //[Authorize] commented this out but this end point will have any mode of auth
        [HttpPatch("{imdbId:string}")]
        public async Task<ActionResult> UpdateMoviePricendTime(string imdbId, [FromBody] UpdateMovieCatalog command)
        {

            var operationResult = await _cinemaCatalogManger.UpdateMoviePriceAndTime(imdbId, command);

            if (!operationResult.IsValid)
            {
                _logger.LogInformation($"{operationResult.Errors}");
                var response = new ResponseApiModel
                {
                    Errors = operationResult.Errors
                };
                return BadRequest(response);
            }
            return NoContent();
        }
    }
}
