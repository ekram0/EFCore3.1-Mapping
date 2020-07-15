using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Data
{
    public class SamuraiContext :DbContext
    {
        public SamuraiContext() {}

        public SamuraiContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Samurai> Samurais { get; set; }

        public static ILoggerFactory ConsoleLoggerFactory
         => LoggerFactory.Create(builder => {
             builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information).AddConsole();
         });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer("Server=. ;Database=SamuriaApp; Trusted_Connection=True; MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(fk => new { fk.BattleId, fk.SamuraiId });

            ////Shadowing property:to control the EFCore Data.
            modelBuilder.Entity<Samurai>().Property<DateTime>("Created");   
            modelBuilder.Entity<Samurai>().Property<DateTime>("LastNodified");

            //// //Mapping (One to ONe ) shadow property, nullable foreign key SamuraiId 
            //modelBuilder.Entity<Samurai>()
            //    .HasOne(s => s.SecretIdentity)
            //    .WithOne(p => p.Samurai)
            //    .HasForeignKey<SecretIdentity>("SamuraiId ");


            ///Mapping unconventionally named foreign key property
            /// Special syntax (parameterless WithOne, HFK<SecretIdentity>
            /// are because I have no Samurai navigation property
            //modelBuilder.Entity<Samurai>()
            //    .HasOne(i => i.SecretIdentity)
            //    .WithOne()
            //    .HasForeignKey<SecretIdentity>(i => i.SamuraiFK);


        }

    }
}
