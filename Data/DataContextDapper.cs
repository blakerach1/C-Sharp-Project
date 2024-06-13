using System.Data;
using Dapper;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.Data

{

    public class DataContextDapper
    {
        // private IConfiguration _config;
        private string _connectionString; 
        public DataContextDapper(IConfiguration config)
        {
            // _config = config;
            _connectionString = config.GetConnectionString("DefaultConnection")+"";
        }
        
        //connecting to database, passing details of server, database, ssl and credentials, which for microsoft is trusted_connection = true
        // underscore because we could technically have something that takes an argument or recreates, another variation that has the same name
        // as this "outer" variable. As long as we don't have any underscore in any of our methods, we will avoid confusion

        // below methods to emulate sql
        // methods are "public", to ensure that they are accessible on any instance of this class, because we are
        // going to create an instance of this class inside our program.cs and we want to be able to call this method  

        // generics to make it dynamic <T> - so we can use the same method to return multiple types
        // alt + arrow key to move lines around in VSCode
        // ctrl + '.' to bring up context menu, i.e. to import required modules for example
        // underscore on connectionString so that it uses the private field of this class when we instantiate it

        public IEnumerable<T> LoadData<T>(string sql)
        {
            // IDbConnection dbConnection = new SqlConnection(_connectionString);
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql);
        }

        // you've got to define the type when the method is called
        // we need a way to tell the method, what type T is going to be when you call it
        // This is done by including the "T" after the method: LoadData<T>(string sql)
        // we will then call the method, define the type and pass in the argument


        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sql);
        }
        
        // bool - have we affected just one row?
        // int - for instances where we do want to know how many rows have been affected
        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql) > 0;
        }
        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql);
        }



    }
}
