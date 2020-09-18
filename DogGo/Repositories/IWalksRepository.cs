using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        SqlConnection Connection { get; }

        void AddWalk(Walks newWalk);
        void DeleteWalk(int walkId);
        Walks GetWalkById(int id);
        List<Walks> GetWalksByDogId(int dogId);
        List<Walks> GetWalksByWalkerId(int walkerId);
        void UpdateWalk(Walks walk);
    }
}