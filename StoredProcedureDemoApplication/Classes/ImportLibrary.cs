using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using CsvHelper;
using StoredProcedureDemoApplication.DTOS;

namespace StoredProcedureDemoApplication.Classes
{
    public class ImportLibrary
    {
        public static List<Movie> GetMoviesFromCSV()
        {
            ;
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            exePath = exePath + "\\SampleData\\SampleMoviesCsv.csv";

            var retval = new List<Movie>();


            //SampleMoviesCsv
            var reader = File.OpenText(exePath);
            var csv = new CsvReader(reader);
            var kount = 0;
            csv.Read(); //skip 1st record
            while (csv.Read())
            {
                ++kount;
                var newMovie = new Movie();

                var externalId = csv.GetField(0);
                if (externalId == "NULL")
                {
                    newMovie.ExternalId = -1;
                }
                else
                {
                    newMovie.ExternalId = Convert.ToInt32(csv.GetField(0));
                }

                var movieName = csv.GetField(1);
                if (movieName.Length > 1000)
                {
                    newMovie.Name = movieName.Substring(0, 900);
                    //newMovie.Name = movieName;
                }
                else
                {
                    newMovie.Name = movieName;
                }

                newMovie.Year = csv.GetField(3);
                if (newMovie.Year.Length > 4)
                {
                    newMovie.Year = "9999";
                }


                retval.Add(newMovie);
            }

            return retval;
        }

        public static int WriteSingleRecords(List<Movie> movies, int numberOfRecords)
        {
            var connString = "Server=(local);Database=StoredProcedureDemo;Trusted_Connection=True;";
            var conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand();
            cmd.CommandText = "AddSingleMovieWithOutput";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;


            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ExternalId",
                SqlDbType = SqlDbType.Int
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Size = 1000
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Year",
                SqlDbType = SqlDbType.Char,
                Size = 4
            });
            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@NewId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            });


            var kount = 0;
            foreach (var movie in movies)
            {
                ++kount;
                cmd.Parameters["@ExternalId"].Value = movie.ExternalId;
                cmd.Parameters["@Name"].Value = movie.Name;
                cmd.Parameters["@Year"].Value = movie.Year;
                cmd.ExecuteNonQuery();
                if (kount == numberOfRecords)
                {
                    break;
                }

                //if (kount % 10000 == 0)
                //{
                //    Console.WriteLine(cmd.Parameters["@newId"].Value);
                //}
               // 
            }


            conn.Close();

            return kount;
        }

        public static int WriteBulkRecords(List<Movie> movies, int numberOfRecords)
        {
            var dt = new DataTable("MovieData");
            dt.Columns.Add("ExternalId");
            dt.Columns.Add("Name");
            dt.Columns.Add("Year");

            var kount = 0;
            foreach (var movie in movies)
            {
                ++kount;
                var row = dt.NewRow();
                row["ExternalId"] = movie.ExternalId;
                row["Name"] = movie.Name;
                row["Year"] = movie.Year;
                dt.Rows.Add(row);

                if (kount == numberOfRecords)
                {
                    break;
                }
            }


            var connString = "Server=(local);Database=StoredProcedureDemo;Trusted_Connection=True;";
            var conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand();
            cmd.CommandText = "AddMoviesBulkLoad";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;


            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@MovieData",
                SqlDbType = SqlDbType.Structured
            });

            cmd.Parameters["@MovieData"].Value = dt;
            cmd.ExecuteNonQuery();


            conn.Close();

            return kount;
        }


        public static ObservableCollection<Movie> GetMoviesByYears(string years)
        {

            var dt = new DataTable("YearDataABCD");
            dt.Columns.Add("StringValue");

            foreach (var year in years.Split('|'))
            {
                var row = dt.NewRow();
                row["StringValue"] = year;
                dt.Rows.Add(row);
            }


            var connString = "Server=(local);Database=StoredProcedureDemo;Trusted_Connection=True;";
            var conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand();
            cmd.CommandText = "GetMoviesByYears";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;


            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@YearList",
                SqlDbType = SqlDbType.Structured
            });

            cmd.Parameters["@YearList"].Value = dt;

            var retval = Mapping.StoredProcedureToObservableCollection<Movie>(cmd);

            conn.Close();

            
            return retval;
        }


        public static ObservableCollection<Movie> GetMoviesByYearsAndName(string name,string years)
        {

            var dt = new DataTable("YearData");
            dt.Columns.Add("StringValue");

            foreach (var year in years.Split('|'))
            {
                var row = dt.NewRow();
                row["StringValue"] = year;
                dt.Rows.Add(row);
            }


            var connString = "Server=(local);Database=StoredProcedureDemo;Trusted_Connection=True;";
            var conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand();
            cmd.CommandText = "GetMoviesByYearsAndName";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;


            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@YearList",
                SqlDbType = SqlDbType.Structured
            });

            cmd.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Size = 1000
            });

            cmd.Parameters["@YearList"].Value = dt;
            cmd.Parameters["@Name"].Value = $"%{name}%";

            var retval = Mapping.StoredProcedureToObservableCollection<Movie>(cmd);

            conn.Close();


            return retval;
        }

    }
}