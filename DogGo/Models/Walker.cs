using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //bring in foriegn key and you need to declare a property to hold the object that key represents
        public int NeighborhoodId { get; set; }
        public string ImageUrl { get; set; }
        //object to hold the results from the FK table representing the values of that foreign key Id
        public Neighborhood Neighborhood { get; set; }
    }
}
