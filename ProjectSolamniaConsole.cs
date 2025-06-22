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
            //       if (args[0] == "-s")
            //       {
            //           Server.SolamniaServer.Run();
            //       }


            //        Console.WriteLine("Project Solamnia Console Application");
            //        Console.WriteLine("Press any key to start...");
            //        Console.ReadKey();
            //        Console.Clear();
            //        ShowLoadingAnimation();
            //        Console.Clear();
            //        Console.WriteLine("Initialization complete. Starting application...\n");
            //        Console.WriteLine("Loading services and database...");
            //        Console.WriteLine("Please wait...");
            //        Thread.Sleep(4000); // Simulate loading time
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
                Console.WriteLine("3. Holdings");
                Console.WriteLine("4. Manage Characters");
                Console.WriteLine("5. Manage Traits");
                Console.WriteLine("6. Manage Holdings");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ListCharacters();
                        break;

                    case "2":
                        ListTraits();
                        break;

                    case "3":
                        ListHoldings();
                        break;

                    case "4":
                        ManageCharacters();
                        break;

                    case "5":
                        ManageTraits();
                        break;

                    case "6":
                        ManageHoldings();
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
        private static void ListCharacters()
        {
            var characters = _characterService.GetAllCharacters();
            foreach (var c in characters)
            {
                Console.WriteLine($"#{c.Id} {c.Name}, Rank: {c.Rank}, Age: {c.Age}");
                Console.WriteLine($" Status: {c.Status}, Holding: {c.AssignedHolding?.Name ?? "None"}, Active Duty: {c.ActiveDuty}");
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

        }

        private static void ListTraits()
        {
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
        }

        private static void ListHoldings()
        {
            var holdings = _holdingService.GetAllHoldings();
            foreach (var h in holdings)
            {
                Console.WriteLine($"#{h.Id} {h.Name} ({h.Type}) - Region: {h.Region}, Supply: {h.SupplyLevel}, Troops: {h.TroopsCount}");
                Console.WriteLine(" Assigned Characters:");
                foreach (var c in h.AssignedCharacters)
                {
                    Console.WriteLine($" - {c.Name} (Rank: {c.Rank}, Age: {c.Age})");
                }
                Console.WriteLine(new string('-', 40));
            }
        }


        private static void ManageCharacters()
        {
            Console.WriteLine("=== Manage Characters ===");
            Console.WriteLine("1. Create New Character");
            Console.WriteLine("2. Update Character");
            Console.WriteLine("3. Delete Character");
            Console.WriteLine("0. Back");
            Console.Write("Your choice: ");

            var choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    CreateCharacter();
                    break;

                case "2":
                    UpdateCharacter();
                    break;

                case "3":
                    DeleteCharacter();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static void ManageTraits()
        {
            Console.WriteLine("=== Manage Traits ===");
            Console.WriteLine("1. Create New Trait");
            Console.WriteLine("2. Update Trait");
            Console.WriteLine("3. Delete Trait");
            Console.WriteLine("0. Back");
            Console.Write("Your choice: ");

            var choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    CreateTrait();
                    break;

                case "2":
                    UpdateTrait();
                    break;

                case "3":
                    DeleteTrait();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static void ManageHoldings()
        {
            Console.WriteLine("=== Manage Holdings ===");
            Console.WriteLine("1. Create New Holding");
            Console.WriteLine("2. Update Holding");
            Console.WriteLine("3. Delete Holding");
            Console.WriteLine("0. Back");
            Console.Write("Your choice: ");

            var choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    CreateHolding();
                    break;

                case "2":
                    UpdateHolding();
                    break;

                case "3":
                    DeleteHolding();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static void CreateCharacter()
        {
            Console.WriteLine("Creating a new character...");
            var newChar = new Character { Id = 0 };

            GetCharacterDetails(newChar);

            // Get trait IDs separately since we need them for validation
            Console.WriteLine("Enter Trait IDs separated by comma (e.g. 1,3,5,36 for 3 Personality + 1 Education):");
            var idsInput = Console.ReadLine();
            var traitIds = idsInput?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();

            if (_characterService.UpdateCharacter(newChar, traitIds, out var err))
                Console.WriteLine("Character created successfully.");
            else
                Console.WriteLine("Error: " + err);
        }


        private static void UpdateCharacter()
        {
            Console.WriteLine("\n=== Update Character ===");
            
            // Get character ID with validation
            int charId;
            while (true)
            {
                Console.Write("\nEnter Character ID to update (0 to cancel): ");
                if (!int.TryParse(Console.ReadLine(), out charId))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (charId == 0) return; // Exit if user cancels

                var existingCharacter = _characterService.GetCharacterById(charId);
                if (existingCharacter != null) break;
                
                Console.WriteLine($"No character found with ID {charId}. Please try again.");
            }

            var character = _characterService.GetCharacterById(charId);
            
            // Show current details
            Console.WriteLine($"\nUpdating character: {character.Name}");
            Console.WriteLine($"Current traits: {string.Join(", ", character.CharacterTraits.Select(ct => ct.Trait.Name))}");

            // Get updated details
            Console.WriteLine("\nEnter new details (leave blank to keep current value):");
            
            Console.Write($"Name [{character.Name}]: ");
            var nameInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nameInput))
                character.Name = nameInput;

            Console.Write($"Age [{character.Age}]: ");
            if (int.TryParse(Console.ReadLine(), out var newAge))
                character.Age = newAge;

            Console.Write($"Rank [{character.Rank}]: ");
            var rankInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(rankInput))
                character.Rank = rankInput;

            Console.WriteLine($"Status (Current: {character.Status})");
            Console.WriteLine("0: AD, 1: KIA, 2: MIA, 3: POW, 4: DOW, 5: AWOL, 6: DES");
            Console.Write("New status: ");
            if (Enum.TryParse<StatusType>(Console.ReadLine(), out var newStatus))
                character.Status = newStatus;
            
            Console.Write($"Rank [{character.ActiveDuty}]: ");
            var activeDutyInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(activeDutyInput))
                character.ActiveDuty = activeDutyInput;

            // Get new traits
            List<int> traitIds;
            while (true)
            {
                Console.Write("\nEnter 3 Personality + 1 Education Trait IDs (comma separated): ");
                var idsInput = Console.ReadLine();
                traitIds = idsInput?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                    .Where(x => x != -1).ToList() ?? new List<int>();

                // Validate trait selection
                var traits = _traitService.GetAllTraits().Where(t => traitIds.Contains(t.Id)).ToList();
                var personalityCount = traits.Count(t => t.Type == TraitType.Personality);
                var educationCount = traits.Count(t => t.Type == TraitType.Education);

                if (personalityCount == 3 && educationCount == 1) break;
                
                Console.WriteLine($"Invalid selection. Need exactly 3 Personality + 1 Education traits (you entered {personalityCount}+{educationCount}).");
            }

            // Update character
            if (_characterService.UpdateCharacter(character, traitIds, out var err))
            {
                Console.WriteLine("\nCharacter updated successfully!");
                Console.WriteLine($"New traits: {string.Join(", ", character.CharacterTraits.Select(ct => ct.Trait.Name))}");
            }
            else
            {
                Console.WriteLine($"\nError updating character: {err}");
            }
        }

        private static void DeleteCharacter()
        {
            Console.WriteLine("Enter Character ID to delete:");
            var charIdInput = Console.ReadLine();
            if (!int.TryParse(charIdInput, out var charId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (_characterService.DeleteCharacter(charId, out var err))
                Console.WriteLine("Character deleted successfully.");
            else
                Console.WriteLine("Error: " + err);
        }

        private static void GetCharacterDetails(Character character)
        {
            Console.WriteLine("Enter character name:");
            character.Name = Console.ReadLine() ?? "";

            Console.WriteLine("Age:");
            if (int.TryParse(Console.ReadLine(), out var age))
                character.Age = age;

            Console.WriteLine("Rank:");
            character.Rank = Console.ReadLine() ?? "";

            Console.WriteLine("Status 0: AD - Active Duty, \n1: KIA - Killed in action, \n2: MIA - Missing in action, \n3: POW - Prisoner of war, \n4: DOW - Died of Wounds, \n5: AWOL - Absent Without Leave, \n6: DES - Deserter:");
            if (Enum.TryParse<StatusType>(Console.ReadLine(), out var status))
                character.Status = status;

            Console.WriteLine("Assigned Holding ID (or leave empty):");
            if (int.TryParse(Console.ReadLine(), out var holdingId))
                character.AssignedHoldingId = holdingId;

            Console.WriteLine("Active Duty:");
            character.ActiveDuty = Console.ReadLine() ?? "";

            Console.WriteLine("Enter attributes separated by spaces (Diplomacy Martial Stewardship Intrigue Learning Prowess), e.g. '5 5 5 5 5 5':");
            var input = Console.ReadLine();

            int[] attributes = new int[6];
            if (!string.IsNullOrWhiteSpace(input))
            {
                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < 6; i++)
                {
                    if (i < parts.Length && int.TryParse(parts[i], out var val))
                        attributes[i] = val;
                    else
                        attributes[i] = 5;
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    attributes[i] = 5;
            }

            character.Diplomacy = attributes[0];
            character.Martial = attributes[1];
            character.Stewardship = attributes[2];
            character.Intrigue = attributes[3];
            character.Learning = attributes[4];
            character.Prowess = attributes[5];
            character.ActiveDuty = "";
        }

        private static void CreateTrait()
        {
            Console.WriteLine("Creating a new trait...");
            var newTrait = new Trait{Name = ""};

            GetTraitDetails(newTrait);

            _traitService.AddTraitWithExclusives(newTrait, new List<int>());
            Console.WriteLine("Trait created successfully.");
        }

        private static void UpdateTrait()
        {
            Console.WriteLine("Enter Trait ID to update:");
            var traitIdInput = Console.ReadLine();
            if (!int.TryParse(traitIdInput, out var traitId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var existingTrait = _traitService.GetTraitById(traitId);
            if (existingTrait == null)
            {
                Console.WriteLine($"Trait with ID {traitId} not found.");
                return;
            }

            Console.WriteLine($"Updating trait: {existingTrait.Name}");
            GetTraitDetails(existingTrait);

            Console.Write("Enter Exclusive Trait IDs separated by comma (e.g. 2,5): ");
            var exclInput = Console.ReadLine();
            var exclIds = exclInput?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();

            if (_traitService.UpdateTrait(existingTrait, exclIds, out var err))
                Console.WriteLine("Trait updated successfully.");
            else
                Console.WriteLine("Error: " + err);
        }

        private static void DeleteTrait()
        {
            Console.WriteLine("Enter Trait ID to delete:");
            var traitIdInput = Console.ReadLine();
            if (!int.TryParse(traitIdInput, out var traitId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            _traitService.DeleteTrait(traitId);
            Console.WriteLine("Trait deleted successfully.");
        }

        private static void GetTraitDetails(Trait trait)
        {
            Console.Write("Trait name: ");
            trait.Name = Console.ReadLine() ?? "";

            Console.Write("Description: ");
            trait.Description = Console.ReadLine();

            Console.Write("Trait type (0: Personality, 1: Education, 2: Other): ");
            if (Enum.TryParse<TraitType>(Console.ReadLine(), out var tType))
                trait.Type = tType;

            Console.Write("Image URL: ");
            trait.ImageUrl = Console.ReadLine();

            Console.WriteLine("Enter stat bonuses (empty = 0):");
            int Read(string s) => int.TryParse(s, out var x) ? x : 0;

            Console.Write("Diplomacy: ");
            trait.BonusDiplomacy = Read(Console.ReadLine() ?? "");

            Console.Write("Martial: ");
            trait.BonusMartial = Read(Console.ReadLine() ?? "");

            Console.Write("Stewardship: ");
            trait.BonusStewardship = Read(Console.ReadLine() ?? "");

            Console.Write("Intrigue: ");
            trait.BonusIntrigue = Read(Console.ReadLine() ?? "");

            Console.Write("Learning: ");
            trait.BonusLearning = Read(Console.ReadLine() ?? "");

            Console.Write("Prowess: ");
            trait.BonusProwess = Read(Console.ReadLine() ?? "");

            Console.Write("Enter Exclusive Trait IDs separated by comma (e.g. 2,5): ");
            var exclInput = Console.ReadLine();
            var exclIds = exclInput?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();
        }

        private static void CreateHolding()
        {
            Console.WriteLine("Creating a new holding...");
            var newHolding = new Holding();

            GetHoldingDetails(newHolding);

            _holdingService.AddHolding(newHolding);
            Console.WriteLine("Holding created successfully.");
        }

        private static void UpdateHolding()
        {
            Console.WriteLine("Enter Holding ID to update:");
            var holdingIdInput = Console.ReadLine();
            if (!int.TryParse(holdingIdInput, out var holdingId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var existingHolding = _holdingService.GetHoldingById(holdingId);
            if (existingHolding == null)
            {
                Console.WriteLine($"Holding with ID {holdingId} not found.");
                return;
            }

            Console.WriteLine($"Updating holding: {existingHolding.Name}");
            GetHoldingDetails(existingHolding);

            _holdingService.UpdateHolding(existingHolding);
            Console.WriteLine("Holding updated successfully.");
        }

        private static void DeleteHolding()
        {
            Console.WriteLine("Enter Holding ID to delete:");
            var holdingIdInput = Console.ReadLine();
            if (!int.TryParse(holdingIdInput, out var holdingId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            _holdingService.DeleteHolding(holdingId);
            Console.WriteLine("Holding deleted successfully.");
        }

        private static void GetHoldingDetails(Holding holding)
        {
            Console.Write("Holding name: ");
            holding.Name = Console.ReadLine() ?? "";

            Console.WriteLine("Holding type (0: Castle, 1: Fort, 2: Outpost, 3: Village, 4: Town, 5: City): ");
            if (Enum.TryParse<HoldingType>(Console.ReadLine(), out var hType))
                holding.Type = hType;

            Console.Write("Region: ");
            holding.Region = Console.ReadLine();

            Console.Write("Description: ");
            holding.Description = Console.ReadLine();

            Console.Write("Supply level (0: Critical, 1: Low, 2: Moderate, 3: High): ");
            if (Enum.TryParse<SupplyLevel>(Console.ReadLine(), out var supply))
                holding.SupplyLevel = supply;

            Console.Write("Troop count: ");
            if (int.TryParse(Console.ReadLine(), out var troops))
                holding.TroopsCount = troops;
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
                Thread.Sleep(800);
                Console.SetCursorPosition(Console.CursorLeft - currentDots.Length, Console.CursorTop);
                Console.Write(new string(' ', currentDots.Length));
                Console.SetCursorPosition(Console.CursorLeft - currentDots.Length, Console.CursorTop);
                i++;
            }
        }
    }
}

