using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//Owner Model of SQL Owner table column names and values 
namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        [MaxLength(35)]
        [DisplayName("Owner Name")]
        public string Name { get; set; }

        //HOW CAN I GET THIS ERROR MESSAGE TO DISPLAY AFTER more than 1 thing has been put into the box?
        [StringLength(55, MinimumLength = 5)]
        [Required(ErrorMessage = "Hmmm... Your address is to short...minimum of 5 letters and a maximum of 55.")]
        public string Address { get; set; }
       
        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        //bring in foriegn key and you need to declare a property to hold the object that key represents
        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }
        //object to hold the results from the FK table representing the values of that foreign key Id
        public Neighborhood Neighborhood { get; set; }
    }
}
