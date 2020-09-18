using System;
using System.Collections.Generic;
using System.Data;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

//repository to handle SQL & CSharp interactions using ADO.NET
//ADO.NET brought in with Microsoft.Data.SQLClient nugget packet
// used manage Nugget Packages from Project dropdown to search for and add that package instead of dotnet install from console.
namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        //create private field to hold server address
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. 
        // ..This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }

        //method to create new connection to SQL database based on the "DefaultConnection" string matching in the appsetting.json for a connectionString: 
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        //GET LIST of Walks by Walker.Id
        //creates a list to hold all the dogs as it expects recieve back many walkers from the database (SQL)
        public List<Walks> GetWalksByWalkerId(int walkerId)
        {
            //implement "using statement" to create a SQLconnection variable
            using (SqlConnection conn = Connection)
            {
                //run Open method inside SqlConnection to start new connection 
                conn.Open();
                //implement "using statement" to create a command to pass through the open connection
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //command sent to sql server (sql query as @"string")
                    cmd.CommandText = @"
                        SELECT 
                            w.Id, w.Date, w.Duration, w.WalkerId, w.DogId,
                            o.Id, o.Name, o.Email, o.Address, o.Phone, o.NeighborhoodId, 
                            d.Name AS DogName, d.Id AS SingleDogId, d.OwnerId, d.Breed, d.Notes, d.ImageUrl
                        FROM Walks w
                            JOIN Walker dw ON w.WalkerId = dw.Id
                            JOIN Dog d ON w.DogId = d.Id          
                            JOIN [Owner] o ON d.OwnerId = o.Id
                        WHERE w.WalkerId = @walkerId
                    ";
                    //built in method to create reader that can interpret sql response
                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    //creates a new list of dogs to hold data read by the sqldatareader
                    List<Walks> walksTaken = new List<Walks>();
                  
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),

                            Dog = new Dog()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("SingleDogId")),
                                    Name = reader.GetString(reader.GetOrdinal("DogName")),
                                    Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                    OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Owner = new Owner()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                                    }
                                },
                        };

                        //add this dog to the list dogs
                        walksTaken.Add(walk);
                        
                    }
                    //READERS MUST ALWAYS BE CLOSED
                    reader.Close();
                    //return the list as our List representing Walkers.GetAllWalkers()
                    return walksTaken;
                }
            }
        }

        //GET SINGLE Walk by Id
        public Walks GetWalkById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Date, Duration, WalkerId, DogId
                        FROM Walks
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Walks walks = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))

                        };

                        reader.Close();
                        return walks;
                    }

                    reader.Close();
                    return null;
                }
            }
        }
        

        //CREATE DOG
        public void AddWalk(Walks newWalk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog (Date, Duration, WalkerId, DogId)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @duration, @walkerId, @dogId);
                ";

                    cmd.Parameters.AddWithValue("@date", newWalk.Date);
                    cmd.Parameters.AddWithValue("@duration", newWalk.Duration);

                    cmd.Parameters.AddWithValue("@walkerId", newWalk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", newWalk.DogId);

                    int id = (int)cmd.ExecuteScalar();

                    newWalk.Id = id;
                }
            }
        }

        //EDIT Walk
        public void UpdateWalk(Walks walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Walk
                            SET 
                                Date = @date, 
                                Duration = @duration, 
                                WalkerId = @walkerId, 
                                DogId = @dogId, 
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duation", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    cmd.Parameters.AddWithValue("@id", walk.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //DELETE Walk
        public void DeleteWalk(int walkId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Walk
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", walkId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //GET LIST of Walks by Dog.Id
        public List<Walks> GetWalksByDogId(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, Date, Duration, WalkerId, DogId
                    FROM Walks
                    WHERE DogId = @dogId
                    ";
                    //built in method to create reader that can interpret sql response
                    cmd.Parameters.AddWithValue("@dogId", dogId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    //creates a new list of dogs to hold data read by the sqldatareader
                    List<Walks> walksTaken = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))

                        };

                        //add this dog to the list dogs
                        walksTaken.Add(walk);
                    }
                    //READERS MUST ALWAYS BE CLOSED
                    reader.Close();
                    //return the list as our List representing Walkers.GetAllWalkers()
                    return walksTaken;
                }
            }
        }


        public Owner GetOwnerByDogId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                         SELECT o.Id, o.Name, o.Email, o.Address, o.Phone, o.NeighborhoodId
                         FROM Walks w
                            JOIN Walker dw ON w.WalkerId = dw.Id
                            JOIN Dog d ON w.DogId = d.Id          
                            JOIN [Owner] o ON d.OwnerId = o.Id
                        WHERE w.WalkerId = @id";
    
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Owner owner = new Owner()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        return owner;
                    }

                    reader.Close();
                    return null;
                }
            }
        }
    }
}