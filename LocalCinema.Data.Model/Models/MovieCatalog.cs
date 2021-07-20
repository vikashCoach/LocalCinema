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

    public class KeyManager { 
    
        public string Uri { get; set; }

        public string AccessToken { get; set; }
    }
    public class MovieTimeSlots
    {
        public string MovieName { get; set; }

        public DateTime DateTime { get; set; }
    }
}
