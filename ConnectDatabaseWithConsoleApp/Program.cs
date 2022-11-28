using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. Address of SQL Server and Database
            // Resource : https://www.c-sharpcorner.com/UploadFile/5089e0/how-to-create-single-connection-string-in-console-applicatio/
            string connectionString = "Data Source=DESKTOP-VJPRRLA;Initial Catalog=UserDatabase;Integrated Security=True";
            //2. Establish Connection
            SqlConnection connection = new SqlConnection(connectionString);
            //3. Open Connection
            connection.Open();

            //4. Prepare query
            Console.Write("Enter FirstName : ");
            string FirstName= Console.ReadLine();
            Console.Write("Enter LastName : ");
            string LastName = Console.ReadLine();

            string Query = "insert into Users(FirstName, LastName) values('"+FirstName+"', '"+LastName+"')";

            //5. Execute Query
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.ExecuteNonQuery();

            //6. reads through the data, writing it out to the console window
            SqlDataReader reader;
            int id;
            connection = new SqlConnection(Properties.Settings.Default.connectionStr);
            connection.Open();
            Console.Write("Enter User Id : ");
            id = int.Parse(Console.ReadLine());
            reader = new SqlCommand("select * from Users where ID=" + id, connection).ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("UserID | FirstName | LastName \n {0}  |   {1}  |   {2}", reader.GetInt32(0),
                    reader.GetString(1), reader.GetString(2));
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }

            //7. Close Connection
            connection.Close();
        }
    }
}
