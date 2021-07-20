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
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        private readonly ICustomerManager _customerManager;

        public CustomerController(ILogger<CustomerController> logger, ICustomerManager customerManager)
        {
            _logger = logger;
            _customerManager = customerManager;
        }

        [HttpGet("{moviename:string}/{date:string}")]
        public async Task<IActionResult> GetMovieTimes(string movieName, string date)
        {
            var operationResult = await _customerManager.GetMovieTimes(movieName, date);

            if (!operationResult.IsValid)
            {
                _logger.LogInformation($"{operationResult.Errors}");
                var response = new ResponseApiModel
                {
                    Errors = operationResult.Errors
                };
                return BadRequest(response);
            }
            return Ok(new ResponseApiModel { Data = operationResult.Data });
        }
        [HttpGet("{moviename:string}/{date:string}")]
        public async Task<IActionResult> GetMovieDetails(string movieName)
        {
            var operationResult = await _customerManager.GetMovieDetails(movieName);

            if (!operationResult.IsValid)
            {
                _logger.LogInformation($"{operationResult.Errors}");
                var response = new ResponseApiModel
                {
                    Errors = operationResult.Errors
                };
                return BadRequest(response);
            }
            return Ok(new ResponseApiModel { Data = operationResult.Data });
        }

    }
}