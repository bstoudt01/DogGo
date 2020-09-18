using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        void AddWalk(Walks newWalk);
        void DeleteWalk(int walkId);
        Walks GetWalkById(int id);
        List<Walks> GetWalksByDogId(int dogId);
        List<Walks> GetWalksByWalkerId(int walkerId);
        void UpdateWalk(Walks walk);
        Owner GetOwnerByDogId(int id);

        public string GetTotalWalkedByWalkerId(int walkerId);

    }
}