using System;
using System.ComponentModel.DataAnnotations;

namespace LocalCinema.Data.Model
{
    public class MovieCatalog
    {

    }
    public class UpdateMovieCatalog
    {
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public Decimal Price { get; set; }
    }
}
