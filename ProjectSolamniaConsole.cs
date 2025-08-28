using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectSolamnia;
using ProjectSolamnia.Migrations;

namespace ProjectSolamnia
{
    // Console application entry point and main program logic
    public class Program
    {
        private static ServiceProvider provider = null!;
        private static ProjectSolamniaDbContext _dbContext = null!;
        private static TraitService _traitService = null!;
        private static CharacterService _characterService = null!;
        private static HoldingService _holdingService = null!;

        public static void Main(string[] args)
        {
            //      if (args.Length != 0 && args[0] == "-s")
            //   {
            //       Server.SolamniaServer.Run();
            //   }
            //                 Server.SolamniaServer.Run();

            //          Console.WriteLine("Project Solamnia Console Application");
            //          Console.WriteLine("Press any key to start...");
            //          Console.ReadKey();
            //          Console.Clear();
            //          ShowLoadingAnimation();
            //          Console.Clear();
            //          Console.WriteLine("Initialization complete. Starting application...\n");
            //          Console.WriteLine("Loading services and database...");
            //          Console.WriteLine("Please wait...");
            //          Thread.Sleep(4000); 
            //           Console.Clear();

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
                Console.WriteLine("7. Generate Random Character");
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
                    case "7":
                        GenerateRandomCharacter();
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

            Console.WriteLine("Level (default is 1):");
            if (int.TryParse(Console.ReadLine(), out var level))
                character.Level = level;
            else
                character.Level = 1;

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
        }

        private static void CreateCharacter()
        {
            Console.WriteLine("Creating a new character...");
            var newChar = new Character();
            GetCharacterDetails(newChar);

            // Trait ID'lerini kullanıcıdan al
            Console.WriteLine("Enter Trait IDs separated by comma (e.g. 1,3,5,36 for 3 Personality + 1 Education):");
            var idsInput = Console.ReadLine();
            var traitIds = ParseIdList(idsInput);

            // Sadece servis çağrısı ve sonuç gösterimi
            if (_characterService.CreateCharacter(newChar, traitIds, out var err))
                Console.WriteLine("Character created successfully.");
            else
                Console.WriteLine("Error: " + err);
        }

        private static void UpdateCharacter()
        {
            ListCharactersBrief();
            Console.WriteLine("\n=== Update Character ===");

            // Get Character ID to update
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

                var exists = _characterService.GetCharacterById(charId);
                if (exists != null) break;

                else
                {
                    Console.WriteLine($"No character found with ID {charId}. Please try again.");
                }
            }

            var current = _characterService.GetCharacterById(charId);

            // Show current details
            Console.WriteLine($"\nUpdating character: {current.Name}");
            Console.WriteLine($"Current traits: {string.Join(", ", current.CharacterTraits.Select(ct => ct.Trait.Name))}");

            // Get updated details
            Console.WriteLine("\nEnter new details (leave blank to keep current value):");

            String? name = ReadOptionalString($"Name [{current.Name}]: ");
            int? age = ReadOptionalInt($"Age [{current.Age}]: ");
            String? rank = ReadOptionalString($"Rank [{current.Rank}]: ");

            Console.Write($"Status [{current.Status}] \n 0: AD, 1: KIA, 2: MIA, 3: POW, 4: DOW, 5: AWOL, 6: DES ");
            StatusType? status = ReadOptionalEnum<StatusType>("New Status: ");

            string? activeDuty = ReadOptionalString($"Active Duty [{current.ActiveDuty}]: ");
            int? level = ReadOptionalInt($"Level [{current.Level}]: ");

            // Get new trait IDs (3 Personality + 1 Education)
            Console.Write("\nEnter 3 Personality + 1 Education Trait IDs (comma separated): ");
            var traitIds = ParseIdList(Console.ReadLine());

            // Prepare update DTO
            var dto = new CharacterUpdateDto
            {
                Name = name,
                Age = age,
                Rank = rank,
                Status = status,
                ActiveDuty = activeDuty,
                Level = level
            };
            // Call service to update
            if (_characterService.UpdateCharacter(charId, dto, traitIds, out var err))
            {
                var updated = _characterService.GetCharacterById(charId)!;
                Console.WriteLine("\nCharacter updated successfully!");
                Console.WriteLine($"New traits: {string.Join(", ", updated.CharacterTraits.Select(ct => ct.Trait.Name))}");
            }
            else
            {
                Console.WriteLine("Error updating character: " + err);
            }
        }

        private static void DeleteCharacter()
        {
            ListCharactersBrief();
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

        private static void CreateTrait()
        {
            Console.WriteLine("Creating a new trait...");
            var newTrait = new Trait { Name = "" };

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

        private static void GenerateRandomCharacter()
        {
            Console.WriteLine("Generating a random character...");
            var random = new Random();

            // 1. Generate base attributes (1-12 with precise bell curve distribution)
            int RollBaseAttribute()
            {
                int roll = random.Next(1, 10001);
                return roll switch
                {
                    <= 277 => 2,    // 2.77%
                    <= 832 => 3,    // 5.55% 
                    <= 1665 => 4,   // 8.33% 
                    <= 2776 => 5,   // 11.11% 
                    <= 4164 => 6,   // 13.88% 
                    <= 5830 => 7,   // 16.66% 
                    <= 7218 => 8,   // 13.88% 
                    <= 8329 => 9,   // 11.11% 
                    <= 9162 => 10,  // 8.33% 
                    <= 9717 => 11,  // 5.55% 
                    _ => 12         // 2.83% 
                };
            }

            // 2. Generate age (16-70) with weighted distribution toward prime years
            int age = random.Next(100) switch
            {
                < 15 => random.Next(16, 26),  // 15% young (16-25)
                < 70 => random.Next(26, 46),  // 55% prime (26-45)
                < 90 => random.Next(46, 61),  // 20% middle-aged (46-60)
                _ => random.Next(61, 71)      // 10% old (61-70)
            };

            // 3. Create character with base attributes
            var newChar = new Character
            {
                Name = GetRandomName(random) ?? "Unknown",
                Age = age,
                Rank = "",
                Status = StatusType.AD,
                ActiveDuty = "",
                Diplomacy = RollBaseAttribute(),
                Martial = RollBaseAttribute(),
                Stewardship = RollBaseAttribute(),
                Intrigue = RollBaseAttribute(),
                Learning = RollBaseAttribute(),
                Prowess = RollBaseAttribute(),
                Level = random.Next(1, 6)
            };

            // 4. education trait

            var educationType = random.Next(6) switch
            {
                0 => "Diplomacy",
                1 => "Martial",
                2 => "Stewardship",
                3 => "Intrigue",
                4 => "Learning",
                _ => "Prowess"
            };

            var educationTrait = _traitService.GetAllTraits()
                .Where(t => t.Type == TraitType.Education)
                .Where(t => IsInEducationCategory(t, educationType))
                .FirstOrDefault(random.Next(100) switch
                {
                    < 35 => t => GetTier(t, educationType) == 1,  // Tier 1 (35%)
                    < 65 => t => GetTier(t, educationType) == 2,  // Tier 2 (30%)
                    < 85 => t => GetTier(t, educationType) == 3,  // Tier 3 (20%)
                    < 95 => t => GetTier(t, educationType) == 4,  // Tier 4 (10%)
                    _ => t => GetTier(t, educationType) == 5      // Tier 5 (5%)
                });

            static bool IsInEducationCategory(Trait trait, string category)
            {
                return category switch
                {
                    "Diplomacy" => trait.BonusDiplomacy > 0 && trait.BonusMartial == 0,
                    "Martial" => trait.BonusMartial > 0 && trait.BonusStewardship == 0,
                    "Stewardship" => trait.BonusStewardship > 0 && trait.BonusIntrigue == 0,
                    "Intrigue" => trait.BonusIntrigue > 0 && trait.BonusLearning == 0,
                    "Learning" => trait.BonusLearning > 0 && trait.BonusProwess == 0,
                    "Prowess" => trait.BonusProwess > 0,
                    _ => false
                };
            }

            static int GetTier(Trait trait, string category)
            {
                return category switch
                {
                    "Diplomacy" => trait.BonusDiplomacy / 2,
                    "Martial" => trait.BonusMartial / 2,
                    "Stewardship" => trait.BonusStewardship / 2,
                    "Intrigue" => trait.BonusIntrigue / 2,
                    "Learning" => trait.BonusLearning / 2,
                    "Prowess" => trait.BonusProwess, // Prowess uses direct values (1-4)
                    _ => 1
                };
            }

            // 5. Select 3 personality traits
            var personalityTraits = _traitService.GetAllTraits()
                .Where(t => t.Type == TraitType.Personality)
                .OrderBy(_ => random.Next())
                .Take(3)
                .ToList();

            // 6. Prepare trait IDs
            var selectedTraitIds = personalityTraits?
                .Select(t => t.Id)
                .ToList() ?? new List<int>();

            if (educationTrait != null)
            {
                selectedTraitIds.Add(educationTrait.Id);
            }

            // 7. Save and display results
            if (_characterService.UpdateCharacter(newChar, selectedTraitIds, out var err))
            {
                Console.WriteLine("\n=== RANDOM CHARACTER ===");
                Console.WriteLine($"{newChar.Name}, {newChar.Age} years old");
                Console.WriteLine($"Rank: {newChar.Rank}, Status: {newChar.Status}");

                Console.WriteLine("\nBase Attributes:");
                Console.WriteLine($"DIP: {newChar.Diplomacy} | MAR: {newChar.Martial} | STE: {newChar.Stewardship}");
                Console.WriteLine($"INT: {newChar.Intrigue} | LEA: {newChar.Learning} | PRO: {newChar.Prowess}");

                Console.WriteLine("\nEffective Attributes:");
                Console.WriteLine($"DIP: {EffectiveAttributeCalculator.EffectiveDiplomacy(newChar)}");
                Console.WriteLine($"MAR: {EffectiveAttributeCalculator.EffectiveMartial(newChar)}");
                Console.WriteLine($"STE: {EffectiveAttributeCalculator.EffectiveStewardship(newChar)}");
                Console.WriteLine($"INT: {EffectiveAttributeCalculator.EffectiveIntrigue(newChar)}");
                Console.WriteLine($"LEA: {EffectiveAttributeCalculator.EffectiveLearning(newChar)}");
                Console.WriteLine($"PRO: {EffectiveAttributeCalculator.EffectiveProwess(newChar)} (Level {newChar.Level})");

                Console.WriteLine("\nTraits:");
                Console.WriteLine($"Education: {educationTrait?.Name ?? "None"}");
                Console.WriteLine($"Personality: {string.Join(", ", personalityTraits.Select(t => t.Name))}");
            }
            else
            {
                Console.WriteLine("Error generating character: " + err);
            }
        }

        // Helpers to read optional inputs
        private static void ListCharactersBrief()
        {
            var list = _characterService.GetAllCharacters(); // yoksa servise ekleyelim
            if (list == null || !list.Any())
            {
                Console.WriteLine("No characters in DB.");
                return;
            }

            Console.WriteLine("\n-- Characters --");
            foreach (var c in list)
                Console.WriteLine($"ID={c.Id}  Name={c.Name}  Level={c.Level}  Status={c.Status}");
        }

        private static String? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        private static int? ReadOptionalInt(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s, out var val) ? val : (int?)null;
        }

        private static TEnum? ReadOptionalEnum<TEnum>(string prompt) where TEnum : struct
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return Enum.TryParse<TEnum>(s, out var val) ? val : (TEnum?)null;
        }

                // Trait ID'lerini string'den listeye çeviren yardımcı fonksiyon
        private static List<int> ParseIdList(string? input)
        {
            return input?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x.Trim(), out var id) ? id : -1)
                .Where(x => x != -1).ToList() ?? new List<int>();
        }

        private static void ShowLoadingAnimation()
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

        private static string GetRandomName(Random random)
        {
            string[] maleNames =
            {
                "Aaron", "Abbo", "Abel", "Abraham", "Absalom", "Achard", "Achilles", "Acledulf", "Aclefrid", "Aclehard",
                "Acleman", "Aclemund", "Actard", "Actwin", "Adalald", "Adalbald", "Adalbod", "Adalfrid", "Adalgrim",
                "Adalhar", "Adalhelm", "Adalmar", "Adalmund", "Adalrad", "Adalwald", "Adam", "Adelard", "Ademar",
                "Adolf", "Adrian", "Adrulf","Ağda", "Aicard", "Ailbert", "Ailhard", "Ainard", "Alain", "Alaric", "Alban",
                "Alberic", "Albert", "Albo", "Aldebrand", "Aldemar", "Aldrich", "Aldwin", "Alexander", "Alfgar",
                "Alfhelm", "Alfred", "Alfwin", "Alphonse", "Alric", "Alvaro", "Alwin", "Amadeus", "Ambrose", "Amis",
                "Ancel", "Andrew", "Anselm", "Ansgar", "Anzo", "Apollonius", "Archibald", "Aristotle", "Arnold",
                "Arnulf", "Artald", "Arthur", "Athelstan", "Aubrey", "Audoen", "August", "Aurelian", "Aurelius",
                "Austin", "Averroes", "Avo", "Aylmer", "Baldwin", "Balthasar", "Barnabas", "Bartholomew", "Basil",
                "Bastian", "Benedict", "Benjamin", "Bernard", "Berengar", "Bertram", "Bjorn", "Blaise", "Bodo",
                "Boguslav", "Boleslav", "Boniface", "Boso", "Brand", "Brian", "Brice", "Bruno", "Cadell", "Cadwallon",
                "Caesar", "Caius", "Casimir", "Cassian", "Charles", "Christian", "Christopher", "Claudian", "Conrad",
                "Constantine", "Corbinian", "Crispin", "Cuthbert", "Cyprian", "Cyril", "Dagobert", "Damian", "Daniel",
                "David", "Denis", "Dietrich", "Dominic", "Donald", "Drogo", "Dunstan", "Edgar", "Edmund", "Edward",
                "Edwin", "Elias", "Eliezer", "Emery", "Engelbert", "Ephraim", "Erik", "Ernest", "Eugene", "Eustace",
                "Everard", "Favian", "Felix", "Ferdinand", "Fulk", "Gabriel", "Cumcision", "Gawain", "Geoffrey", "George", "Gerard",
                "Gervase", "Gilbert", "Giles", "Godfrey", "Godric", "Godwin", "Gregory", "Grimbald", "Gualter", "Gunnar",
                "Guy", "Harold", "Hector", "Henry", "Herbert", "Hildebrand", "Hincmar", "Hugh", "Humbert", "Humphrey",
                "Ivo", "Jasper", "Jerome", "John", "Jolan", "Joseph", "Joshua", "Julian", "Julius", "Justus", "Kenelm",
                "Lambert", "Laurence", "Leif", "Leonard", "Leopold", "Lothar", "Louis", "Lucian", "Ludovic", "Magnus",
                "Malcolm", "Marcus", "Martin", "Matthew", "Maurice", "Michael", "Nicholas", "Odo", "Oliver", "Orson",
                "Oswald", "Otho", "Otto", "Pascal", "Patrick", "Paul", "Percival", "Peter", "Philip", "Raimond", "Ralph",
                "Raymond", "Reginald", "Reinbald", "Richard", "Robert", "Roderick", "Roger", "Roland", "Rolf", "Rupert",
                "Samson", "Sebastian", "Siegfried", "Sigismund", "Simon", "Stephen", "Tancred", "Theobald", "Theodore",
                "Theodoric", "Thomas", "Thurstan", "Tiberius", "Timothy", "Tobias", "Torsten", "Tristan", "Ulrich",
                "Ulysses", "Valentin", "Victor", "Vincent", "Virgil", "Vitalis", "Vivian", "Waleran", "Walter", "Warin",
                "Wenceslas", "Wilfred", "William", "Wulfric", "Xavier", "Yves", "Zachary"
            };

            string[] femaleNames =
            {
                "Adelaide", "Adelina", "Agatha", "Agnes", "Alba", "Aldith", "Alexandra", "Alice", "Amabel", "Amalia",
                "Amice", "Anastasia", "Andrea", "Angela", "Anna", "Anne", "Avelina", "Beatrice", "Berenice", "Brigid",
                "Cecilia", "Clarimond", "Constance", "Drusilla", "Eleanor", "Elizabeth", "Emmeline", "Eugenia",
                "Euphemia", "Felicia", "Florence", "Genevieve", "Gisela", "Gratiana", "Helena", "Hildegard", "Idony",
                "Isabel", "Joan", "Juliana", "Katherine", "Leah", "Lucia", "Margaret", "Maria", "Matilda", "Mirabel",
                "Olivia", "Philippa", "Rosamund", "Sabina", "Sophia", "Theodora", "Ursula", "Valentina", "Winifred", "Ysabel"
            };

            string[] homeLand =
            {
                "uth Duskhollow", "uth Ironreach", "uth Wolfshearth", "uth Bleakmarsh", "uth Rivenrock", "uth Thornwold",
                "uth Shadowcrest", "uth Stonemark", "uth Kjeldur", "uth Ulfdale", "uth Hargoth’s Stand", "uth Vexmire",
                "uth Grimspire", "uth Duskrend", "uth Frostbite", "uth Raven’s Maw", "uth Briarstoke", "uth Witchmelt",
                "uth Ashthroat", "uth Deadspan", "uth Hearthscar", "uth Blightstoke", "uth Moorgrave", "uth Blackmire",
                "uth Stoneharrow", "uth Wyrmfen", "uth Frostsink", "uth Grimbreach", "uth Vaelmoor", "uth Stormcrag",
                "uth Caergoth", "uth Edgerton", "uth Harrying", "uth Hamilton", "uth Lockhart", "uth Starport",
                "uth Stimpton", "uth Restglen", "uth Rening", "uth O'Call", "uth Di Estra", "uth Firstward", "uth Gorbie",
                "uth Wtdel", "uth Ironrock", "uth Portsmith", "uth Deepdel", "uth Gwyntarr", "uth Lytburg", "uth Thelgaard",
                "uth Brasdel", "uth Luinstat", "uth Sage", "uth Kyre", "uth Vex", "uth Ravenscar", "uth Cairngorn",
                "uth di Caela", "uth Solanthus", "uth Arnal", "uth Patina", "uth Tresvka", "uth Hartford", "uth Jansburg",
                "uth Auchunan", "uth Egaard", "uth Valoria", "uth Forestedge", "uth Delgaard", "uth Relgoth", "uth Ryn",
                "uth Arngrim", "uth Brightblade", "uth Southford", "uth Naergoth", "uth Tearford", "uth Starmont",
                "uth Gaarlus", "uth Navarre", "uth Viranesh", "uth Ligett", "uth Vogler", "uth Kalaman", "uth Witdell",
                "uth Manydell", "uth Gander", "uth Hargoth", "uth Winterholm", "uth Potter's Mill", "uth Korval",
                "uth Godnest", "uth Palanthas", "uth Dawnfort", "uth Highrule", "uth Varus", "di Calea", "de Montrefeltrp",
                "Boyle", "Ashworth", "Winslow", "Pathwarden", "Donner"
             };

            bool isMale = random.Next(2) == 0;
            string firstName = isMale
                ? maleNames[random.Next(maleNames.Length)]
                : femaleNames[random.Next(femaleNames.Length)];

            string title = isMale ? "Sir" : "Dame";
            string home = homeLand.Length > 0
                ? homeLand[random.Next(homeLand.Length)]
                : "Homeland"; // Fallback if empty

            return $"{title} {firstName} {home}";
        }
    }
}

