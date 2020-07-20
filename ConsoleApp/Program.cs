using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiContext context = new SamuraiContext();

        static void Main(string[] args)
        {
            QuerySamuraiBattleStats();
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai
            {
                Name = "Sampson"
            };

            var samurai2 = new Samurai
            {
                Name = "Tasha"
            };

            var samurai3 = new Samurai
            {
                Name = "Jhon"
            };

            var samurai4 = new Samurai
            {
                Name = "Micheal"
            };

            context.Samurais.AddRange(samurai,
                                      samurai2,
                                      samurai3,
                                      samurai4);
            context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai
            {
                Name = "Samurai 1"
            };

            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void GetSamurais()
        {
            var samurais = context.Samurais.ToList();
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
                Console.WriteLine(Environment.NewLine);
            }
        }

        private static void QueryFilters()
        {
            var name = "Sampson";
            var samurais = context.Samurais.Where(s => s.Name == name).ToList();
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            context.SaveChanges();
        }

        private static void InsertBattle()
        {
            context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });

            context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertNewSamuraiWithQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kembei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote
                    {
                        Text = "I've come to save you"
                    }
                }
            };

            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });

            context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?",
                SamuraiId = samuraiId
            };

            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = context.Samurais.Where(s => s.Name.Contains("Julie"))
                                                    .Include(s => s.Quotes).FirstOrDefault();            
        }        

        private static void QuerySamuraiBattleStats()
        {
             var stats = context.SamuraiBattleStats.ToList();            
        }
    }
}
