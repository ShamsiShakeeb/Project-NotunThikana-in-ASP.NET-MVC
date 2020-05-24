using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectNotunThikana.Models
{
    public class Database
    {

        private  String Connection = "";
        public  SqlConnection connection;
        public  SqlDataReader reading;

        public SqlConnection DatabaseCon(String Connection)
        {

            this.Connection = Connection;

            ///   this.Connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection("Server=.;Database=" + Connection + "; Integrated Security=true");
         ///   connection = new SqlConnection("Server=SQL5002.site4now.net,1433;Database=DB_A3FB49_"+Connection+";User Id=DB_A3FB49_NotunThikana_admin;Password=Sa01762120546");

            connection.Open();

            return connection;
        }

        
       

        public void setData(String Query)
        {

           //// DatabaseCon(this.Connection);
            SqlConnection sc = new SqlConnection();
            SqlCommand com = new SqlCommand();
            sc.ConnectionString = ("Data Source=.;Database=" + this.Connection + ";Integrated Security=True");
              /// sc.ConnectionString = ("Server=SQL5002.site4now.net,1433;Database=DB_A3FB49_" + this.Connection + ";User Id=DB_A3FB49_NotunThikana_admin;Password=Sa01762120546");
            sc.Open();
            com.Connection = sc;


            com.CommandText = (Query);
            com.ExecuteNonQuery();
            sc.Close();
        }

        public void getData(String Query)
        {
             

            SqlCommand comand = new SqlCommand(

            Query, connection);

            reading = comand.ExecuteReader();

           
            
        }
        public int Empty(String x)
        {

            char[] a = x.ToCharArray();

            int len = x.Length;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == ' ')
                {
                    len--;
                }
            }
            return len;
        }



    }
}
