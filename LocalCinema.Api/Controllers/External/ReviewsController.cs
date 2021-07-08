using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LocalCinema.Api.Internal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewsController : ControllerBase
    {
        

        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(ILogger<ReviewsController> logger)
        {
            _logger = logger;
        }

        
    }
}