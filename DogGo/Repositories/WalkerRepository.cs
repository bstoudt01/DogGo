using System.Collections.Generic;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

//repository to handle SQL & CSharp interactions using ADO.NET
//ADO.NET brought in with Microsoft.Data.SQLClient nugget packet
// used manage Nugget Packages from Project dropdown to search for and add that package instead of dotnet install from console.
namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        //create private field to hold server address
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. 
        // ..This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
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

        //creates a list to hold all the walkers as it expects recieve back many walkers from the database (SQL)
        public List<Walker> GetAllWalkers()
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
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                    ";
                    //built in method to create reader that can interpret sql response
                    SqlDataReader reader = cmd.ExecuteReader();

                    //creates a new list of walkers to hold data read by the sqldatareader
                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        //properties parsed out 
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        walkers.Add(walker);
                    }
                    //READERS MUST ALWAYS BE CLOSED
                    reader.Close();
                    //return the list as our List representing Walkers.GetAllWalkers()
                    return walkers;
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    //condtional to handle id not existing in table
                    if (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        return walker;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
    }
}