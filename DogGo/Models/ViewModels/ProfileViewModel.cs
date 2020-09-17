using System;
using System.Collections.Generic;

//class that does not directly model our database instead it..
// displays what we want on the DOM from the database by adding the pieces of each model we need
namespace DogGo.Models.ViewModels
{
    //we are creating a profile view 
    //we are bringing in the owner object , and we can use that as the single / main thing we are profiling (parent model?)
    // we bring in 2 lists from the other models, so those appear to be supporting data for the profile
    public class ProfileViewModel
    {
        //entire owner model from owner model
        public Owner Owner { get; set; }
        //list of walkers  from walkers model
        public List<Walker> Walkers { get; set; }

        //list of dogs from the dog model
        public List<Dog> Dogs { get; set; }
    }
}