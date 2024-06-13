using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HelloWorld.Data

{

    // semicolon for inherting class on the right of the statement
    public class DataContextEF : DbContext
    {

        private IConfiguration _config;
        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }
        //DB set below. Methods that connect into this db 
        public DbSet<Computer>? Computer { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // to reset the default schema - alternative is the commented out code below
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<Computer>()
                // .HasNoKey();
                .HasKey(c => c.ComputerId); // if there is a PK
                // .ToTable("Computer", "TutorialAppSchema");
                // .ToTable("TableName", "SchemaName");
            // added a new entity for our computer model that hooks to our DB set
            // to instruct it to look at the correct schema - second argument of the above ToTable method
        }

    }

}