using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext Context = new SamuraiContext();

        public static void Main(string[] args)
        {
            //PrePopulaeSamuraisAndBattles();
            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoBattle();
            //EnlistSamuraiIntoBattleUntracked();
            //AddNewSamuraiViaDisConnectedObject();
            //GetSamuraiWithBattle();
            //RemoveJoinBetweenSamuraiAndBattleSimple();
            //RemoveBattleFromSamurai();
            //AddSecretIdentityUsingSamuraiID();
            //AddSecretIdentityToExistingSamurai();
            //EditASecretIdentity();
            //ReplaceASecretIdentity();
            //ReplaceASecretIdentityNotTracked();
            //ReplaceSecretIdentityNotInMemory();
            //CreateSamurai();
            //RetrieveSamuraisCreatedInYesterday();
            //CreateThenEditSamuraiWithQote();
            //RetrieveYearUsingDbBuiltInFunction();
            //RetrieveScalarResult();
            Console.WriteLine("press any key...");
            Console.ReadKey();
        }




        private static void PrePopulaeSamuraisAndBattles()
        {
            Context.AddRange(
             new Samurai { Name = "Kikuchiyo" },
             new Samurai { Name = "Kambei Shimada" },
             new Samurai { Name = "Shichirōji " },
             new Samurai { Name = "Katsushirō Okamoto" },
             new Samurai { Name = "Heihachi Hayashida" },
             new Samurai { Name = "Kyūzō" },
             new Samurai { Name = "Gorōbei Katayama" }
             );

            Context.Battles.AddRange(
             new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) },
             new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
             new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
             new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
             );

            Context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            var sbJoin = new SamuraiBattle { BattleId = 3, SamuraiId = 8 };
            Context.Add(sbJoin);
            Context.SaveChanges();
        }

        private static void EnlistSamuraiIntoBattle()
        {
            var battle = Context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 3 });
            Context.SaveChanges();
        }

        private static void EnlistSamuraiIntoBattleUntracked()
        {
            Battle battle;
            using var newContext = new SamuraiContext();
            battle = newContext.Battles.Find(2);

            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 3 });
            Context.Battles.Attach(battle);
            Context.SaveChanges();

        }

        private static void AddNewSamuraiViaDisConnectedObject()
        {
            Battle battle;
            using var separateOperation = new SamuraiContext();
            battle = separateOperation.Battles.Find(1);

            var newSamurai = new Samurai { Name = "Sampson" };
            battle.SamuraiBattles.Add(new SamuraiBattle
            {
                Samurai = newSamurai
            });
            Context.Battles.Attach(battle);
            Context.SaveChanges();

        }

        private static void GetSamuraiWithBattle()
        {
            var samuraiWBattle = Context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(s => s.Id == 1);

            var battle = samuraiWBattle.SamuraiBattles.FirstOrDefault().Battle;
            var allBattles = new List<Battle>();
            samuraiWBattle.SamuraiBattles.ForEach(b => allBattles.Add(b.Battle));
        }

        private static void GetBattlesForSamuraiInMemory()
        {
            var battle = Context.Battles.Find(1);
            Context.Entry(battle).Collection(b => b.SamuraiBattles).Query().Include(sb => sb.Samurai).Load();
        }

        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 2, SamuraiId = 7 };
            Context.Remove(join);
            Context.SaveChanges();
        }

        private static void RemoveBattleFromSamurai()
        {
            //Goal: Remove join between IDSamurai =3 and battleID=1;
            var samurai = Context.Samurais
                .Include(c => c.SamuraiBattles).ThenInclude(sb => sb.Battle)
                .SingleOrDefault(p => p.Id == 7);

            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 2);
            samurai.SamuraiBattles.Remove(sbToRemove);
            //Context.Remove(sbToRemove); Not preferred
            Context.SaveChanges();

        }

        private static void AddSecretIdentityUsingSamuraiID()
        {
            //Note:SamuraiId 1 does not have a secret identity yet;
            var identity = new SecretIdentity { SamuraiId = 1 };
            Context.Add(identity);
            Context.SaveChanges();
        }

        private static void AddSecretIdentityToExistingSamurai()
        {
            var Samurai = Context.Samurais.Find(2);

            Samurai.SecretIdentity = new SecretIdentity { RealName = "July" };
            Context.Samurais.Attach(Samurai);
            Context.SaveChanges();
        }

        private static void EditASecretIdentity()
        {
            var samurai = Context.Samurais
                .Include(s => s.SecretIdentity)
                .FirstOrDefault(p => p.Id == 1);

            samurai.SecretIdentity.RealName = "Teslaa";

            Context.SaveChanges();
        }

        private static void ReplaceASecretIdentity()
        {
            var samurai = Context.Samurais
                .Include(s => s.SecretIdentity)
                .FirstOrDefault(p => p.Id == 1);

            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            Context.SaveChanges();
        }

        private static void ReplaceASecretIdentityNotTracked()
        {
            Samurai samurai;
            using (var separateOperation = new SamuraiContext())
            {
                samurai = separateOperation.Samurais
                    .Include(s => s.SecretIdentity)
                    .FirstOrDefault(p => p.Id == 2);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Try" };
            Context.Samurais.Update(samurai);
            Context.SaveChanges();
        }

        private static void ReplaceSecretIdentityNotInMemory()
        {
            var samurai = Context.Samurais
                .FirstOrDefault(s => s.SecretIdentity != null);

            samurai.SecretIdentity = new SecretIdentity { RealName = "NotInMemory" };
            Context.SaveChanges();
        }

        private static void CreateSamurai()
        {
            var samurai = new Samurai { Name = "GoodProperty" };
            Context.Samurais.Add(samurai);

            var timeStamp = DateTime.Now;
            Context.Entry(samurai).Property("Created").CurrentValue = timeStamp;
            Context.Entry(samurai).Property("LastNodified").CurrentValue = timeStamp;
            Context.SaveChanges();
        }

        private static void RetrieveSamuraisCreatedInYesterday()
        {
            var yesterday = DateTime.Now.AddDays(-1);
            //var newSamurais = Context.Samurais
            //    .Where(s => EF.Property<DateTime>(s, "Created") >= yesterday)
            //    .ToList();

            ////Using projection
            var newSamurais = Context.Samurais
                .Where(s => EF.Property<DateTime>(s, "Created") >= yesterday)
                .Select(a=> new {a.Id , a.Name , Created = EF.Property<DateTime>(a,"Created")} )
                .ToList();
        }

        private static void CreateThenEditSamuraiWithQote()
        {
            var samurai = new Samurai { Name = "Ronin" };
            var quote = new Quote { Text = "Aren't I Marvelous" };
            samurai.Quotes.Add(quote);
            Context.Samurais.Add(samurai);
            Context.SaveChanges();

            quote.Text += "See what i did there";
            Context.SaveChanges();
        }
    
        //private static void RetrieveAndUpdateBetterName()
        //{
        //    var samurai = Context.Samurais.FirstOrDefault(e=>e.BetterName.FullName =="Black");
        //    samurai.BetterName.GivenName = "Joe";
        //    Context.SaveChanges();

        //}

        private static void GetAllSamurais()
        {
            var allsamurais = Context.Samurais.ToList();
        }

        private static void RetrieveYearUsingDbBuiltInFunction()
        {
            var battles = Context.Battles
                 .Select(b => new { b.Name, b.StartDate.Year }).ToList();
        }

        private static void RetrieveScalarResult()
           => Context.Samurais
                .Select(s => new 
                {
                    s.Name,
                    EarliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id)
                }).ToList();

        private static void FilterWithScalarResult()
            => Context.Samurais
                    .Where(s => EF.Functions.Like(SamuraiContext.EarliestBattleFoughtBySamurai(s.Id), "%Battle%"))
                    .Select(s => new
                    {
                        s.Name,
                        EarliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id)
                    })
                   .ToList();

        private static void SortWithScalar()
        {
            var samurai = Context.Samurais
                .OrderBy(s => SamuraiContext.EarliestBattleFoughtBySamurai(s.Id))
                .Select(s => new { s.Name, EarliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id) })
                .ToList();
        }

        private static void SortWithOutReturningScalar()
        {
            var samurai = Context.Samurais
                .OrderBy(s => SamuraiContext.EarliestBattleFoughtBySamurai(s.Id))
                .ToList();
        }

        private static void RetrieveBattleDays()
        {
            var battle = Context.Battles.Select(b => new { b.Name ,Day = SamuraiContext.DaysInBattle(b.StartDate, b.EndDate) }).ToList();
        }

        private static void RetrieveBattleDaysWithoutDbFunction()
        {
            var battle = Context.Battles.Select(
                b =>new 
                {
                    b.Name,
                    Days = DateDiffDaysPlusOne(b.StartDate , b.EndDate)
                }
                ).ToList();
        }

        private static int DateDiffDaysPlusOne(DateTime start, DateTime end)
            => (int)end.Subtract(start).TotalDays + 1;

    }
}
