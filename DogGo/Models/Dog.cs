using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//Owner Model of SQL Owner table column names and values 
namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        [MaxLength(35)]
        [DisplayName("Name")]
        public string Name { get; set; }

        
        [Required]
        [DisplayName("Breed")]
        public string Breed { get; set; }

        public string Notes { get; set; }

        [DisplayName("Photo")]
        [Url]
        public string ImageUrl { get; set; }
        //object to hold the results from the FK table representing the values of that foreign key Id
        
        [DisplayName("Owner")]
        public int OwnerId { get; set; }
        //bring in foriegn key and you need to declare a property to hold the object that key represents
        public Owner Owner { get; set; }

    }
}
