using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Owner Model of SQL Owner table column names and values 
namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        //bring in foriegn key and you need to declare a property to hold the object that key represents
        public int NeighborhoodId { get; set; }
        public string Phone { get; set; }
        //object to hold the results from the FK table representing the values of that foreign key Id
        public Neighborhood Neighborhood { get; set; }
    }
}
