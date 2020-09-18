using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    //MODEL representing a Join Table of Dogs and the people that walk them, Walkers. + supporting info for the walk
    // created a walks repository to handle the queries we need for our viewmodels
    //created an instance of this class in WalkersController for Details View of the WalkProfileViewModel properties / methods.
    public class Walks
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        public int DogId { get; set; }
        public Dog Dog { get; set; }
    }
}
