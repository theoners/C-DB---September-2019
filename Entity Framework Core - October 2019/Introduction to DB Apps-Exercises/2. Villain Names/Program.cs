using System;
using System.Data.SqlClient;

namespace _2._Villain_Names
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = new SqlConnection(@"Server=ASUS\SQLEXPRESS;Database=MinionsDB;Integrated security=true;");

            connectionString.Open();

            using (connectionString)
            {
                var cmd = new SqlCommand(@"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                                  FROM Villains AS v 
                                                  JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                                  GROUP BY v.Id, v.Name 
                                                  HAVING COUNT(mv.VillainId) > 3 
                                                  ORDER BY COUNT(mv.VillainId)",connectionString);

                var reader =cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0]+" "+reader[1]);
                    }
                }
            }
        }
    }
}
