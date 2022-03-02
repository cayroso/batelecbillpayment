using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.App.Models.Breedings;
using Data.App.Models.Cages;
using Data.App.Models.Chats;
using Data.App.Models.Contacts;
using Data.App.Models.Farms;
using Data.App.Models.Rabbits;
using Data.App.Models.Seasons;
using Data.App.Models.Teams;
using Data.Providers;

namespace Data.App.DbContext
{
    public static class AppDbContextInitializer
    {
        static Random _rnd = new Random((int)DateTime.UtcNow.Ticks);

        public static void Initialize(AppDbContext ctx, IEnumerable<ProvisionUserRole> provisionUserRoles)
        {
            ctx.SaveChanges();

            GenerateUsersAndTeams(ctx, provisionUserRoles);

            GenerateContacts(ctx);

            var farmId = GenerateFarm(ctx);
            GenerateBloodlines(ctx);
            GenerateCages(ctx, farmId);

            var jan1 = DateTime.Now.AddMonths(-DateTime.Now.Month);
            var season1 = new Season
            {
                SeasonId = "season1",
                Name = "Season 1",
                DateStart = jan1,
                DateEnd = jan1.AddMonths(12),
            };
            ctx.Add(season1);

            GenerateRabbits(ctx, farmId, season1.SeasonId);
        }

        static void GenerateUsersAndTeams(AppDbContext ctx, IEnumerable<ProvisionUserRole> provisionUserRoles)
        {
            var teamId = NewId();

            var team = new Team
            {
                TeamId = teamId,
                Name = "Default",
                Description = "Default Team",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Members = provisionUserRoles.Select(e => new TeamMember
                {
                    MemberId = e.User.UserId,
                }).ToList(),
                Chat = new Chat
                {
                    ChatId = teamId,
                    Title = "Chat: Default",
                    Receivers = provisionUserRoles.Select(e => new ChatReceiver
                    {
                        ReceiverId = e.User.UserId
                    }).ToList()
                }
            };

            ctx.Add(team);
        }

        static void GenerateContacts(AppDbContext ctx)
        {
            var foo = GetNames().Select(e => new Contact
            {
                ContactId = NewId(),
                FirstName = e.Item1,
                MiddleName = "",
                Email = $"{e.Item1}@{e.Item2}.com",
                ReferralSource = "facebook",
                Title = "Mr/Ms",
                Website = $"www.{e.Item1}.com",
                LastName = e.Item2,
                HomePhone = e.Item3,
                MobilePhone = "",
                BusinessPhone = "",
                Fax = "12345",
                Industry = "IT",
                Rating = 3,

                DateOfInitialContact = DateTime.UtcNow,
                Address = "102 Main Street",
                AnnualRevenue = 1.2M,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Status = Enums.EnumContactStatus.Lead,
            });

            ctx.AddRange(foo);
        }

        static void GenerateBloodlines(AppDbContext ctx)
        {
            var items = new[]
            {
                new Bloodline
                {
                     BloodlineId = "newzealand", Name="New Zealand"
                },
                new Bloodline
                {
                    BloodlineId = "california", Name = "California"
                },
                new Bloodline
                {
                    BloodlineId = "flemishgiant", Name = "Flemish Giant"
                }
            };

            ctx.AddRange(items);
        }

        static void GenerateCages(AppDbContext ctx, string farmId)
        {
            var items = new[]
            {
                new Cage
                {
                    CageId = "cage1", Name = "Cage #1",  FarmId = farmId,
                },
                new Cage
                {
                    CageId = "cage2", Name= "Cage #2", FarmId = farmId,
                },
                new Cage
                {
                    CageId = "cage3", Name= "Cage #3", FarmId = farmId,
                },
                new Cage
                {
                    CageId = "cage4", Name= "Cage #4", FarmId = farmId,
                },
                new Cage
                {
                    CageId = "cage5", Name= "Cage #5", FarmId = farmId,
                }
            };

            ctx.AddRange(items);
        }

        static string GenerateFarm(AppDbContext ctx)
        {
            var farm = new Farm
            {
                FarmId = "default-farm",
                Name = "Default Farm",
                Description = "The default farm",
            };

            ctx.Add(farm);

            return farm.FarmId;
        }

        static void GenerateRabbits(AppDbContext ctx, string farmId, string seasonId)
        {
            var items = new[]
            {
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "sire1", Name = "Sire #1", Gender = Enums.EnumRabbitGender.Buck,
                },
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "dam1", Name= "Dam #1",Gender = Enums.EnumRabbitGender.Doe,
                },
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "buck1", Name= "Buck #1", BreedingId = "breeding1",Gender = Enums.EnumRabbitGender.Buck,
                },
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "buck2", Name= "Buck #2", BreedingId = "breeding1",Gender = Enums.EnumRabbitGender.Buck,
                },
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "doe1", Name= "Doe #1",BreedingId = "breeding1",Gender = Enums.EnumRabbitGender.Doe,
                },
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "doe2", Name= "Doe #2",BreedingId = "breeding1",Gender = Enums.EnumRabbitGender.Doe,
                },
                new Rabbit
                {
                    FarmId = farmId, RabbitId = "doe3", Name= "Doe #3",BreedingId = "breeding1",Gender = Enums.EnumRabbitGender.Doe,
                }
            };

            ctx.AddRange(items);

            var breedings = new[]
            {
                new Breeding
                {
                    BreedingId = "breeding1",
                    SeasonId = seasonId,
                    FarmId = farmId,
                    BreedingDate = DateTime.UtcNow.AddMonths(-1),
                    CageId = "cage1",
                    Bucks=1,
                    Does =1,
                    Alive = 2, Dead = 1,
                    BirthDate = DateTime.UtcNow,
                    BreedingSuccessful = true,
                    Active = true,
                    Parents = new List<BreedingParent>
                    {
                        new BreedingParent
                        {
                            RabbitId = "sire1", RabbitGender = Enums.EnumRabbitGender.Buck,
                        },

                        new BreedingParent
                        {
                            RabbitId = "dam1", RabbitGender = Enums.EnumRabbitGender.Doe,
                        },
                    }
                }
            };

            ctx.AddRange(breedings);
        }

        static List<Tuple<string, string, string, string>> GetNames()
        {
            var list = new List<Tuple<string, string, string, string>>();

            //list.Add(new Tuple<string, string, string, string>("Juan", "Dela Cruz", "09191234567", "105 Paz Street, Barangay 11, Balayan, Batangas City"));
            //list.Add(new Tuple<string, string, string, string>("Pening", "Garcia", "09191234567", "101 Subdivision 202 Street, Barangay, Town, City, Philippines"));
            //list.Add(new Tuple<string, string, string, string>("Nadia", "Cole", "09191234567", "301 Main Street, Barangay 3, Balayan, Batangas City"));
            //list.Add(new Tuple<string, string, string, string>("Chino", "Pacia", "09191234567", "202 Subdivision 303 Street, Barangay, Town, City, Philippines"));
            //list.Add(new Tuple<string, string, string, string>("Vina", "Ruruth", "09191234567", "501 Main Street, Barangay 5, Balayan, Batangas City"));
            //list.Add(new Tuple<string, string, string, string>("Lina", "Mutac", "09191234567", "601 Main Street, Barangay 6, Balayan, Batangas City"));

            list.Add(new Tuple<string, string, string, string>("Penny", "Garcia", "09191234567", "101 Subdivision 202 Street, Barangay, Town, City, Philippines"));
            list.Add(new Tuple<string, string, string, string>("Gino", "Pacia", "09191234567", "202 Subdivision 303 Street, Barangay, Town, City, Philippines"));

            return list;
        }

        static string NewId()
        {
            return Guid.NewGuid().ToString().ToLower();
        }

        static string NewCouponCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = _rnd;
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
    }
}
