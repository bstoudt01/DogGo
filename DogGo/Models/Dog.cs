using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Owner Model of SQL Owner table column names and values 
namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        //bring in foriegn key and you need to declare a property to hold the object that key represents
        public string Breed { get; set; }
        public string Notes { get; set; }
        public string ImageURL { get; set; }
        //object to hold the results from the FK table representing the values of that foreign key Id
        public Owner Owner { get; set; }

    }
}
