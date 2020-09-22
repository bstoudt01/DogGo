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
    public class DogRepository : IDogRepository
    {
        //create private field to hold server address
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. 
        // ..This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
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

        //GET LIST ALL Dogs
        //creates a list to hold all the dogs as it expects recieve back many walkers from the database (SQL)
        public List<Dog> GetAllDogs()
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
                    SELECT Id, [Name], Breed, Notes, ImageUrl, OwnerId 
                    FROM Dog
                    ";
                    //built in method to create reader that can interpret sql response
                    SqlDataReader reader = cmd.ExecuteReader();

                    //creates a new list of dogs to hold data read by the sqldatareader
                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {

                        //properties parsed out 
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                        //conditionals to handle null values that are allowed to be stored in the DB based on Database rules for those rows
                        //.IsDBNull returns T/F 
                        //if the value of the column for this row returns NOT null (== false), then do the code and set the property... else do no thing and move on..
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        //same thing but with ! for T/F declaration
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        };

                        //add this dog to the list dogs
                        dogs.Add(dog);
                    }
                    //READERS MUST ALWAYS BE CLOSED
                    reader.Close();
                    //return the list as our List representing Walkers.GetAllWalkers()
                    return dogs;
                }
            }
        }

        //GET SINGLE Dog by Id
        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Breed, Notes, ImageUrl, OwnerId
                        FROM Dog
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        };

                        reader.Close();
                        return dog;
                    }

                    reader.Close();
                    return null;
                }
            }
        }

        //CREATE DOG
        public void AddDog(Dog newDog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog ([Name], Breed, Notes, ImageUrl, OwnerId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @breed, @notes, @imageUrl, @ownerId);
                ";
                 
                    cmd.Parameters.AddWithValue("@name", newDog.Name);
                    cmd.Parameters.AddWithValue("@breed", newDog.Breed);
                    
                    cmd.Parameters.AddWithValue("@ownerId", newDog.OwnerId);

                    // LOOK AT THIS
                    //DOES THE SAME THING AS THE IF STATEMENT BELOW?
                    cmd.Parameters.AddWithValue("@notes", newDog.Notes ?? "");


                    //DBNull.Value declares the null type (a non existant value) that our code can translate to SQL and handle
                    //conditional to capture if the value of the input box was null
                    //if Notes is null then assign the value the database understands as null
                    
                    if (newDog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", newDog.ImageUrl);
                    }
                    int id = (int)cmd.ExecuteScalar();

                    newDog.Id = id;
                }
            }
        }

        //EDIT Dog
        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Dog
                            SET 
                                [Name] = @name, 
                                Breed = @breed, 
                                Notes = @notes, 
                                ImageUrl = @imageUrl, 
                                OwnerId = @ownerId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@id", dog.Id);
                    if (dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                    }

                    // LOOK AT THIS
                    if (dog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);
                    }


                    cmd.ExecuteNonQuery();
                }
            }
        }

        //DELETE Dog
        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //GET LIST of Dogs by Owner.Id
        public List<Dog> GetDogsByOwnerId(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT d.Id, d.Name, d.Breed, d.Notes, d.ImageUrl, d.OwnerId,
                    o.[Name] AS OwnerName
                FROM Dog d
                LEFT JOIN Owner o ON d.OwnerId = o.Id
                WHERE d.OwnerId = @ownerId
            ";

                    cmd.Parameters.AddWithValue("OwnerId", ownerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };

                        // Check if optional columns are null
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }
    }
}