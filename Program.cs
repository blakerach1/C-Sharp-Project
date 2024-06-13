// See https://aka.ms/new-console-template for more information

using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;
using AutoMapper;
using Dapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelloWorld
{    
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // create an instance of our new class
            // gives us access to dapper and all of it's public methods
            DataContextDapper dapper = new DataContextDapper(config);
            // DataContextEF entityFramework = new DataContextEF(config);

            // DateTime rightNow = dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
            
            // Computer myComputer = new Computer()
            // {
            //     Motherboard = "Z690",
            //     HasWifi = true,
            //     HasLTE = false,
            //     ReleaseDate = DateTime.Now,
            //     Price = 943.87m,
            //     VideoCard = "RTX 2060"
            // };

            // the below would replace the dapper and sql instructions
            // entityFramework.Add(myComputer);
            // entityFramework.SaveChanges();

            // below is an insert statement, including all field names, putting all values in between single quotes
            //@ allows us to right multiple lines in one string. We pass in field names of the table and then values
            
            // string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //     Motherboard,
            //     HasWifi,
            //     HasLTE,
            //     ReleaseDate,
            //     Price,
            //     VideoCard
            // ) VALUES('" + myComputer.Motherboard
            //         + "','" + myComputer.HasWifi
            //         + "','" + myComputer.HasLTE
            //         + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
            //         + "','" + myComputer.Price
            //         + "','" + myComputer.VideoCard
            // + "')";

            // we are going to write the sql to a file and then we are going to read back
            // built in method on File type - we can use to access methods

            // File.WriteAllText("log.txt", "\n" + sql + "\n"); // creates a file inside the project. It will overwrite without streamwriter
            
            // using StreamWriter openFile = new("log.txt", append: true);
            // openFile.WriteLine("\n" + sql + "\n");
            // inserting \n above (after the values or after sql argument) will allow a new line to be inserted between write entries

            // openFile.Close();

            string computersJson = File.ReadAllText("ComputersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
                cfg.CreateMap<ComputerSnake, Computer>()
                .ForMember(destination => destination.ComputerId, options =>
                    options.MapFrom(source => source.computer_id))
                .ForMember(destination => destination.Motherboard, options =>
                    options.MapFrom(source => source.motherboard))
                .ForMember(destination => destination.HasWifi, options =>
                    options.MapFrom(source => source.has_wifi))
                .ForMember(destination => destination.HasLTE, options =>
                    options.MapFrom(source => source.has_lte))
                .ForMember(destination => destination.ReleaseDate, options =>
                    options.MapFrom(source => source.release_date))
                .ForMember(destination => destination.VideoCard, options =>
                    options.MapFrom(source => source.video_card))
                .ForMember(destination => destination.CPUCores, options =>
                    options.MapFrom(source => source.cpu_cores))
                .ForMember(destination => destination.Price, options =>
                    options.MapFrom(source => source.price));
            }));



            // needs to take in a configuration
            // needs a destination and source to map from and to - destination based model (Computer) and source based model (ComputerSnake)
            // need to add options on each property on the models "members"
            // for member in destination model, pick the field that we are mapping to, then another anonymous function
            // which is an options call, telling us where to map from

            // Console.WriteLine(computersJson); - tests access to file data

            // we want to be able to loop through each individual object in the JSON file. 

            // JsonSerializerOptions options = new JsonSerializerOptions()
            // {
            //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // };
            
            // IEnumerable<Computer>? computersNewtonSoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);
            // IEnumerable if you don't need to add. If you need to add to it, List is preferred. 

            IEnumerable<Computer>? computersSystemJsonPropertyMapping = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson);
            // already maps it to a computer, still pulling out of computersJson object, which is pulling from ComputersSnake.json
            // computersSystem is the result of our deserialization

            if (computersSystemJsonPropertyMapping != null)
            {
                foreach (Computer computer in computersSystemJsonPropertyMapping)
                {
                    Console.WriteLine(computer.Motherboard);
                }
            }

            // IEnumerable<ComputerSnake>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);
            
            // if (computersSystem != null)
            // {
            //     IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computersSystem);

            //     foreach (Computer computer in computerResult)
            //     {
            //         Console.WriteLine(computer.Motherboard);
            //     }
            // }

            // conversion of chasing missing from system.text.json - above options fixes it - needed for both deserializing and serializing. 

            // for Newton Soft to ensure JSON preserves camelcasing, instead of pascall case - only needed for serializing - see settings below

            // JsonSerializerSettings settings = new JsonSerializerSettings()
            // {
            //     ContractResolver = new CamelCasePropertyNamesContractResolver()
            // };

            // if (computersNewtonSoft != null)
            // {
            //     foreach(Computer computer in computersNewtonSoft)
            //     {
            //         string releaseDate = computer.ReleaseDate.HasValue 
            //         ? computer.ReleaseDate.Value.ToString("yyyy-MM-dd") 
            //         : "NULL";
                    
            //         // Console.WriteLine(computer.Motherboard);
            //         string sql = @"INSERT INTO TutorialAppSchema.Computer (
            //             Motherboard,
            //             HasWifi,
            //             HasLTE,
            //             ReleaseDate,
            //             Price,
            //             VideoCard
            //         ) VALUES('" + EscapeSingleQuote(computer.Motherboard)
            //                 + "','" + computer.HasWifi
            //                 + "','" + computer.HasLTE
            //                 + "','" + computer.ReleaseDate?.ToString("yyy-MM-dd")
            //                 + "','" + computer.Price
            //                 + "','" + EscapeSingleQuote(computer.VideoCard)
            //         + "')";
            //         dapper.ExecuteSql(sql);
            //     }
            // }

            // string computersCopyNewtonsoft = JsonConvert.SerializeObject(computersNewtonSoft, settings);
            // File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);
            
            // string computersCopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);
            // File.WriteAllText("computersCopySystem.txt", computersCopySystem);

            // below has been written to handle an issue that was discovered during compilation
            // which was related to a single quote
            // In SQL inside of a string, or a VARCHAR in SQL, you can't have a single quote
            // because single quote is the beginning or end of a VARCHAR in SQL
            // below converts single quote to double single quote, which means a single quote will be included in the string

            static string EscapeSingleQuote(string input)
            {
                string output = input.Replace("'", "''");

                return output;
            }
























            // pass sql execute to dapper
            // dbConnection.Execute(sql); // return value of this will be the # of rows affected >0

            // Console.WriteLine(sql);
            // bool result = dapper.ExecuteSql(sql);
            // int result = dapper.ExecuteSqlWithRowCount(sql);
            // Console.WriteLine(result);

            // below is a sql query that will return IEnumerable
            // one row is one instance from our Computer model
            // string sqlSelect = @"
            // SELECT 
            //     Computer.ComputerId,
            //     Computer.Motherboard,
            //     Computer.HasWifi,
            //     Computer.HasLTE,
            //     Computer.ReleaseDate,
            //     Computer.Price,
            //     Computer.VideoCard 
            // FROM TutorialAppSchema.Computer";

            // IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);
            
            //ToList() is a method available for IEnumerable, however it already is the most efficient data type

            // Console.WriteLine("'ComputerId','Motherboard', 'HasWifi', 'HasLTE'" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
            //         + ", 'Price','VideoCard'");
            
            // foreach(Computer singleComputer in computers)
            // {
            //     Console.WriteLine("'" + singleComputer.ComputerId 
            //         + "','" + singleComputer.Motherboard
            //         + "','" + singleComputer.HasWifi
            //         + "','" + singleComputer.HasLTE
            //         + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
            //         + "','" + singleComputer.Price
            //         + "','" + singleComputer.VideoCard
            // + "'");
            // }

            // entity framework equivalent instruction below. Question marks to handle null values.
            // IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();

            // if (computersEf != null) 
            // {

            //     Console.WriteLine("'ComputerId','Motherboard', 'HasWifi', 'HasLTE'" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
            //             + ", 'Price','VideoCard'");
                
            //     foreach(Computer singleComputer in computersEf)
            //     {
            //         Console.WriteLine("'" + singleComputer.ComputerId 
            //             + "','" + singleComputer.Motherboard
            //             + "','" + singleComputer.HasWifi
            //             + "','" + singleComputer.HasLTE
            //             + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
            //             + "','" + singleComputer.Price
            //             + "','" + singleComputer.VideoCard
            //     + "'");

            //     }
        }

           

    }
}
