using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        [MaxLength(35)]
        public string Name { get; set; }


        //bring in foriegn key and you need to declare a property to hold the object that key represents
        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }
        
        [DisplayName("Photo")]
        [Url]
        public string ImageUrl { get; set; }
        //object to hold the results from the FK table representing the values of that foreign key Id
        public Neighborhood Neighborhood { get; set; }
    }
}
