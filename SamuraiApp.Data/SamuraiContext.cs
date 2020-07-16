using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext() { }

        public SamuraiContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Samurai> Samurais { get; set; }

        public static ILoggerFactory ConsoleLoggerFactory
         => LoggerFactory.Create(builder =>
         {
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

            #region Shadowing Property
            ////Shadowing property:to control the EFCore Data.
            ///this method for only one property.
            //modelBuilder.Entity<Samurai>().Property<DateTime>("Created");   
            //modelBuilder.Entity<Samurai>().Property<DateTime>("LastNodified");

            ////to track all entities by iterating 
            ////property modelBuilder.Model.GetEntityTypes()
            ///the add shadow property for each of them.
            modelBuilder.Model.GetEntityTypes()
                .ToList().ForEach(x =>
                {
                    modelBuilder.Entity(x.Name).Property<DateTime>("Created");
                    modelBuilder.Entity(x.Name).Property<DateTime>("LastModified");
                });
            #endregion

            #region Mapping Complex Type=>Owned Type

            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).ToTable("");
            ////will generate new table with name BetterNames, if use ToTable
            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).ToTable("BetterNames");
            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName)
            //    .Property(b => b.GivenName).HasColumnName("GivenName");

            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName)
            //    .Property(b => b.SureName).HasColumnName("SurName");

            #endregion

            #region Mapping One To One

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

            #endregion

        }


        [DbFunction(Schema = "dbo")]
        public static string EarliestBattleFoughtBySamurai(int samuraiId)
        {
            throw new Exception();
        }

        [DbFunction(Schema = "dbo")]
        public static int DaysInBattle(DateTime start, DateTime end)
            => (int)end.Subtract(start).TotalDays + 1;

        #region override savechange to work w/ OwnedType
        //// Override to audit data for all entities.
        ////the Owned Type work-around does not work with InMemory provider!!

        //public override int SaveChanges()
        //{
        //    ChangeTracker.DetectChanges();
        //    var timeStamp = DateTime.Now;
        //    ChangeTracker.Entries()
        //        .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified)
        //               && !e.Metadata.IsOwned())
        //        .ToList().ForEach(entry =>
        //        {
        //            entry.Property("LastModified").CurrentValue = timeStamp;

        //            if (entry.State == EntityState.Added)
        //                entry.Property("Created").CurrentValue = timeStamp;

        //            if (entry.Entity is Samurai)
        //            {
        //                if (entry.Reference("BetterName").CurrentValue == null)
        //                    entry.Reference("BetterName").CurrentValue = FullNameSamurai.Empty();

        //                entry.Reference("BetterName").TargetEntry.State = entry.State;
        //            }
        //        });
        //    return base.SaveChanges();
        //}
        #endregion
    }
}
