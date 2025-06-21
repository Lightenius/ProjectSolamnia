using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectSolamnia;
using ProjectSolamnia.Migrations;

namespace ProjectSolamnia
{
    public class Program
    {
        private static ServiceProvider provider = null!;
        private static ProjectSolamniaDbContext _dbContext = null!;
        private static TraitService _traitService = null!;
        private static CharacterService _characterService = null!;
        private static HoldingService _holdingService = null!;

        public static void Main(string[] args)
        {
            if (args[0] == "-s")
            {
                Server.SolamniaServer.Run();
            }

            
            Console.WriteLine("Project Solamnia Console Application");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            Console.Clear();
            ShowLoadingAnimation();
            Console.Clear();
            Console.WriteLine("Initialization complete. Starting application...\n");
            Console.WriteLine("Loading services and database...");
            Console.WriteLine("Please wait...");
            Thread.Sleep(4000); // Simulate loading time
            Console.Clear();

            InitializeServices();
            RunApplication();
        }

        private static void InitializeServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ProjectSolamniaDbContext>(options =>
                options.UseSqlite("Data Source=solamnia.db"));
            services.AddScoped<TraitService>();
            services.AddScoped<CharacterService>();
            services.AddScoped<HoldingService>();

            provider = services.BuildServiceProvider();

            var scope = provider.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ProjectSolamniaDbContext>();
            _dbContext.Database.Migrate();

            _traitService = scope.ServiceProvider.GetRequiredService<TraitService>();
            _characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();
            _holdingService = scope.ServiceProvider.GetRequiredService<HoldingService>();
        }

        private static void RunApplication()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Project Solamnia ===");
                Console.WriteLine("1. Characters");
                Console.WriteLine("2. Traits");
                Console.WriteLine("3. New Character / Update");
                Console.WriteLine("4. New Trait / Update");
                Console.WriteLine("5. Holdings");
                Console.WriteLine("6. New Holding / Update");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        var chars = _characterService.GetAllCharacters();
                        foreach (var c in chars)
                        {
                            Console.WriteLine($"#{c.Id} {c.Name}, Rank: {c.Rank}, Age: {c.Age}");
                            Console.WriteLine($" Status: {c.Status}, Holding: {c.AssignedHolding?.Name ?? "None"}");
                            Console.WriteLine($" Traits: {string.Join(", ", c.CharacterTraits.Select(t => t.Trait.Name))}");
                            Console.WriteLine(
                                    $"Attributes: \nDIP={EffectiveAttributeCalculator.EffectiveDiplomacy(c)} " +
                                    $"\nMAR={EffectiveAttributeCalculator.EffectiveMartial(c)} " +
                                    $"\nSTE={EffectiveAttributeCalculator.EffectiveStewardship(c)} " +
                                    $"\nINT={EffectiveAttributeCalculator.EffectiveIntrigue(c)} " +
                                    $"\nLEA={EffectiveAttributeCalculator.EffectiveLearning(c)} " +
                                    $"\nPRO={EffectiveAttributeCalculator.EffectiveProwess(c)}");
                            Console.WriteLine(new string('-', 40));
                        }
                        break;

                    case "2":
                        var traits = _traitService.GetAllTraits();
                        foreach (var t in traits)
                        {
                            Console.WriteLine($"#{t.Id} {t.Name} ({t.Type})");
                            Console.WriteLine($" Desc: {t.Description}");
                            Console.WriteLine($" Image URL: {t.ImageUrl}");
                            Console.WriteLine($" Bonuses: \nDIP={t.BonusDiplomacy}, \nMAR={t.BonusMartial}, \nSTE={t.BonusStewardship}, \nINT={t.BonusIntrigue}, \nLEA={t.BonusLearning}, \nPRO={t.BonusProwess}");
                            if (t.ExclusiveWithTraits.Any())
                            {
                                Console.WriteLine($" Exclusive With: {string.Join(", ", t.ExclusiveWithTraits.Select(e => e.ExclusiveWithTrait.Name))}");
                            }
                            Console.WriteLine(new string('-', 40));
                        }
                        break;

                    case "3":
                        CreateOrUpdateCharacter();
                        break;

                    case "4":
                        CreateOrUpdateTrait();
                        break;

                    case "5":
                        var holdings = _holdingService.GetAllHoldings();
                        foreach (var h in holdings)
                        {
                            Console.WriteLine($"#{h.Id} {h.Name} ({h.Type}) - Region: {h.Region}, Supply: {h.SupplyLevel}, Troops: {h.TroopsCount}");
                            Console.WriteLine(" Assigned Characters:");
                            foreach (var c in h.AssignedCharacters)
                            {
                                Console.WriteLine($" - {c.Name} (Rank: {c.Rank}, Age: {c.Age})");
                            }
                        }
                        break;

                    case "6":
                        CreateOrUpdateHolding();
                        break;

                    case "0":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void CreateOrUpdateCharacter()
        {
            Console.WriteLine("Enter character name:");
            var name = Console.ReadLine();

            Console.WriteLine("Age:");
            var ageInput = Console.ReadLine();
            int age = int.TryParse(ageInput, out var a) ? a : 30;

            Console.WriteLine("Rank:");
            var rank = Console.ReadLine();

            Console.WriteLine("Status 0: AD - Active Duty, \n1: KIA - Killed in action, \n2: MIA - Missing in action, \n3: POW - Prisoner of war, \n4: DOW - Died of Wounds, \n5: AWOL - Absent Without Leave, \n6: DES - Deserter:");
            var statusInput = Console.ReadLine();
            StatusType status = Enum.TryParse<StatusType>(statusInput, out var s) ? s : StatusType.AD;

            Console.WriteLine("Assigned Holding ID (or leave empty):");
            var holdingIdInput = Console.ReadLine();
            int holdingId = int.TryParse(holdingIdInput, out var hId) ? hId : 0;

            Console.WriteLine("Enter Character ID to update (or leave empty for new character):");
            var charIdInput = Console.ReadLine();
            int charIdt = int.TryParse(charIdInput, out var cId) ? cId : 0;

            Console.WriteLine("Enter Trait IDs separated by comma (e.g. 1,3,5):");
            var idsInput = Console.ReadLine();
            var ids = idsInput?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();

            Console.WriteLine("Enter attributes separated by spaces (Diplomacy Martial Stewardship Intrigue Learning Prowess), e.g. '5 5 5 5 5 5':");
            var input = Console.ReadLine();

            // girilen mesajı 6 parçaya ayır ve her birini int'e çevir
            // eğer boşsa 5 olarak ayarla
            int[] attributes = new int[6];
            if (!string.IsNullOrWhiteSpace(input))
            {
                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < 6; i++)
                {
                    if (i < parts.Length && int.TryParse(parts[i], out var val))
                        attributes[i] = val;
                    else
                        attributes[i] = 5;  // default değer
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    attributes[i] = 5; // default hepsi 5
            }


            var newChar = new Character
            {
                Id = cId,
                Name = name ?? "",
                Age = age,
                Rank = rank ?? "",
                Status = status,
                AssignedHoldingId = holdingId,
                Activeduty = "Yes",
                Diplomacy = attributes[0],
                Martial = attributes[1],
                Stewardship = attributes[2],
                Intrigue = attributes[3],
                Learning = attributes[4],
                Prowess = attributes[5]
            };

            if (_characterService.UpdateCharacter(newChar, ids, out var err))
                Console.WriteLine("Character added/updated successfully.");
            else
                Console.WriteLine("Error: " + err);
        }

        private static void CreateOrUpdateTrait()
        {
            Console.Write("Trait name: ");
            var name = Console.ReadLine();

            Console.Write("Description: ");
            var desc = Console.ReadLine();

            Console.Write("Trait type (0: Personality, 1: Education, 2: Other): ");
            var tTypeInput = Console.ReadLine();
            TraitType tType = Enum.TryParse<TraitType>(tTypeInput, out var tt) ? tt : TraitType.Personality;

            Console.Write("Image URL: ");
            var imageUrl = Console.ReadLine();

            Console.WriteLine("Enter stat bonuses (empty = 0):");
            int Read(string s) => int.TryParse(s, out var x) ? x : 0;

            Console.Write("Diplomacy: ");
            int dip = Read(Console.ReadLine() ?? "");

            Console.Write("Martial: ");
            int mar = Read(Console.ReadLine() ?? "");

            Console.Write("Stewardship: ");
            int ste = Read(Console.ReadLine() ?? "");

            Console.Write("Intrigue: ");
            int intg = Read(Console.ReadLine() ?? "");

            Console.Write("Learning: ");
            int lea = Read(Console.ReadLine() ?? "");

            Console.Write("Prowess: ");
            int pro = Read(Console.ReadLine() ?? "");

            Console.Write("Enter Exclusive Trait IDs separated by comma (e.g. 2,5): ");
            var exclInput = Console.ReadLine();
            var exclIds = exclInput?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();

            var newTrait = new Trait
            {
                Name = name ?? "",
                Description = desc,
                Type = tType,
                BonusDiplomacy = dip,
                BonusMartial = mar,
                BonusStewardship = ste,
                BonusIntrigue = intg,
                BonusLearning = lea,
                BonusProwess = pro
            };

            _traitService.AddTraitWithExclusives(newTrait, exclIds);
            Console.WriteLine("Trait added.");
        }

        private static void CreateOrUpdateHolding()
        {
            Console.Write("Holding name: ");
            var name = Console.ReadLine();

            Console.WriteLine("Holding type (0: Castle, 1: Fort, 2: Outpost, 3: Village, 4: Town, 5: City): ");
            var holdingTypeInput = Console.ReadLine();
            HoldingType hType = Enum.TryParse<HoldingType>(holdingTypeInput, out var ht) ? ht : HoldingType.Castle;

            Console.Write("Region: ");
            var region = Console.ReadLine();

            Console.Write("Description: ");
            var desc = Console.ReadLine();

            Console.Write("Supply level (0: Low, 1: Medium, 2: High): ");
            var supplyInput = Console.ReadLine();
            SupplyLevel supply = Enum.TryParse<SupplyLevel>(supplyInput, out var sl) ? sl : SupplyLevel.Moderate;

            Console.Write("Troop count: ");
            var troopsInput = Console.ReadLine();
            int.TryParse(troopsInput, out var troops);

            var newHolding = new Holding
            {
                Name = name ?? "",
                Type = hType,
                Region = region,
                Description = desc,
                SupplyLevel = supply,
                TroopsCount = troops
            };

            _holdingService.AddHolding(newHolding);
            Console.WriteLine("Holding added.");
        }

        static void ShowLoadingAnimation()
        {
            Console.Write("Initializing please wait ");
            var dots = new[] { ".", "..", "..." };
            var start = DateTime.Now;
            int i = 0;

            while ((DateTime.Now - start).TotalSeconds < 5)
            {
                var currentDots = dots[i % dots.Length];
                Console.Write(currentDots);
                Thread.Sleep(650);
                Console.SetCursorPosition(Console.CursorLeft - currentDots.Length, Console.CursorTop);
                Console.Write(new string(' ', currentDots.Length));
                Console.SetCursorPosition(Console.CursorLeft - currentDots.Length, Console.CursorTop);
                i++;
            }
        }
    }
}
