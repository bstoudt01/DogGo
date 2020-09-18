using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        //walks is join table with walker and dog listed along with the data related to the walk
        //LIST of Walks Taken  by single walker that will include Owner.Name from the join on Dog.OwnerId
        public List<Walks> Walks { get; set; }

        //Single Walker to Profile
        public Walker Walker { get; set; }
 
    }
}