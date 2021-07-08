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
    public class CinemaCatalogController : ControllerBase
    {
        

        private readonly ILogger<CinemaCatalogController> _logger;

        public CinemaCatalogController(ILogger<CinemaCatalogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<dynamic> GetMoviesCatalog()
        {
             throw new NotImplementedException();
        }
    }
}
