using System;
using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        //walks is join table with walker and dog listed along with the data related to the walk
        //LIST of Walks Taken  by single walker that will include Owner.Name from the join on Dog.OwnerId
        /// <summary>
        /// List GetWalksByWalkerId and String GetTotalWaledByWalkerId used in Walker Details
        /// </summary>
        public List<Walks> GetWalksByWalkerId { get; set; }
        public string GetTotalWalkedByWalkerId { get; set; }
        
        
        public List<Walker> Walkers { get; set; }

        //Single Walker to Profile
        public Walker Walker { get; set; }

        // Single Owner 
        public Owner Owner { get; set; }

        //Single Dog

        public Dog Dog { get; set; }

    }
}