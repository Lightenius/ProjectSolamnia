using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


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
        private static CharacterGenerationService _charGenService = null!;
        private static WisdomService _wisdomService = null!;
        private static DbReset _dbReset = null!;




        public static void Main(string[] args)
        {
                  if (args.Length != 0 && args[0] == "-s")
               {
                   Server.SolamniaServer.Run();
               }
                             Server.SolamniaServer.Run();

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
                options.UseSqlite("Data Source=data/projectsolamnia.db"));
            services.AddScoped<TraitService>();
            services.AddScoped<CharacterService>();
            services.AddScoped<HoldingService>();
            services.AddScoped<CharacterGenerationService>();
            services.AddScoped<WisdomService>();
            services.AddScoped<DbReset>();

            provider = services.BuildServiceProvider();

            var scope = provider.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ProjectSolamniaDbContext>();
            _dbContext.Database.Migrate();

            _traitService = scope.ServiceProvider.GetRequiredService<TraitService>();
            _characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();
            _holdingService = scope.ServiceProvider.GetRequiredService<HoldingService>();
            _charGenService = scope.ServiceProvider.GetRequiredService<CharacterGenerationService>();
            _wisdomService = scope.ServiceProvider.GetRequiredService<WisdomService>();
            _dbReset = scope.ServiceProvider.GetRequiredService<DbReset>();
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
                Console.WriteLine("8. Dev: DB Reset Menu (DANGER!)");
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

                    case "8":
                        ResetMenu();
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
                Console.WriteLine($" Status: {c.Status}, Holding: {c.AssignedHolding?.Name ?? "None"}, Mission: {c.Mission}");
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

            
            while (true)
            {
                Console.Write("Enter an ID to view full sheet (or press Enter / '0' to exit): ");
                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input) || string.Equals(input, "0", StringComparison.OrdinalIgnoreCase))
                    break;

                if (!int.TryParse(input, out var charId))
                {
                    Console.WriteLine("Invalid ID.");
                    continue;
                }

                var c = _characterService.GetCharacterById(charId);
                if (c == null)
                {
                    Console.WriteLine("Character not found.");
                    continue;
                }

                // Ensure wisdom is up to date before computing effective stats
                _wisdomService.TopUpWisdom(c);

                // 3) Full character sheet
                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"Character Sheet for #{c.Id}: {c.Name}");
                Console.WriteLine(new string('=', 60));

                Console.WriteLine($" Rank:   {c.Rank}");
                Console.WriteLine($" Age:    {c.Age}");
                Console.WriteLine($" Status: {c.Status}");
                Console.WriteLine($" Level:  {c.Level}");
                Console.WriteLine($" Holding: {c.AssignedHolding?.Name ?? "None"}");
                Console.WriteLine($" Mission: {c.Mission}");

                Console.WriteLine();
                Console.WriteLine("Base Attributes:");
                Console.WriteLine($" DIP = {c.BaseDiplomacy}");
                Console.WriteLine($" MAR = {c.BaseMartial}");
                Console.WriteLine($" STE = {c.BaseStewardship}");
                Console.WriteLine($" INT = {c.BaseIntrigue}");
                Console.WriteLine($" LEA = {c.BaseLearning}");
                Console.WriteLine($" PRO = {c.BaseProwess}");

                Console.WriteLine();
                Console.WriteLine("Effective Attributes:");
                Console.WriteLine($" DIP = {EffectiveAttributeCalculator.EffectiveDiplomacy(c)}");
                Console.WriteLine($" MAR = {EffectiveAttributeCalculator.EffectiveMartial(c)}");
                Console.WriteLine($" STE = {EffectiveAttributeCalculator.EffectiveStewardship(c)}");
                Console.WriteLine($" INT = {EffectiveAttributeCalculator.EffectiveIntrigue(c)}");
                Console.WriteLine($" LEA = {EffectiveAttributeCalculator.EffectiveLearning(c)}");
                Console.WriteLine($" PRO = {EffectiveAttributeCalculator.EffectiveProwess(c)}");

                Console.WriteLine();
                Console.WriteLine("Wisdom allocation (cumulative):");
                Console.WriteLine($" DIP +{c.WisDip}, MAR +{c.WisMar}, STE +{c.WisSte}, INT +{c.WisInt}, LEA +{c.WisLea}, PRO +{c.WisPro}");
                Console.WriteLine($" Wisdom points applied: {c.WisdomPointsApplied}");

                Console.WriteLine();
                Console.WriteLine("-------- TRAITS --------");
                foreach (var ct in c.CharacterTraits.Select(x => x.Trait))
                    Console.WriteLine(TraitService.FormatTraitBrief(ct));

                Console.WriteLine(new string('=', 60));
                Console.WriteLine(); // blank line before the next prompt
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

            Console.WriteLine("Mission:");
            character.Mission = Console.ReadLine() ?? "";

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

            character.BaseDiplomacy = attributes[0];
            character.BaseMartial = attributes[1];
            character.BaseStewardship = attributes[2];
            character.BaseIntrigue = attributes[3];
            character.BaseLearning = attributes[4];
            character.BaseProwess = attributes[5];
        }

        private static void CreateCharacter()
        {
            Console.WriteLine("Creating a new character...");
            var newChar = new Character();
            GetCharacterDetails(newChar);

            // Trait ID'lerini kullanıcıdan al
            Console.WriteLine("Enter Trait IDs separated by comma (e.g. 1,3,5,36 for 3 Personality + 1 Education):");
            var idsInput = Console.ReadLine();
            var traitIds = ConsoleHelpers.ParseIdList(idsInput);

            // Sadece servis çağrısı ve sonuç gösterimi
            if (_characterService.CreateCharacter(newChar, traitIds, out var err))
                Console.WriteLine("Character created successfully.");
            else
                Console.WriteLine("Error: " + err);
        }

        private static void UpdateCharacter()
        {
            ConsoleHelpers.PrintCharactersBrief(_characterService.GetAllCharactersBrief());
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

            String? name = ConsoleHelpers.ReadOptionalString($"Name [{current.Name}]: ");
            int? age = ConsoleHelpers.ReadOptionalInt($"Age [{current.Age}]: ");
            String? rank = ConsoleHelpers.ReadOptionalString($"Rank [{current.Rank}]: ");

            Console.Write($"Status [{current.Status}] \n 0: AD, 1: KIA, 2: MIA, 3: POW, 4: DOW, 5: AWOL, 6: DES ");
            StatusType? status = ConsoleHelpers.ReadOptionalEnum<StatusType>("New Status: ");

            string? mission = ConsoleHelpers.ReadOptionalString($"Mission [{current.Mission}]: ");
            int? level = ConsoleHelpers.ReadOptionalInt($"Level [{current.Level}]: ");

            // Get new trait IDs (3 Personality + 1 Education)
            Console.Write("\nEnter 3 Personality + 1 Education Trait IDs (comma separated): ");
            var traitIds = ConsoleHelpers.ParseIdList(Console.ReadLine());

            // Prepare update DTO
            var dto = new CharacterUpdateDto
            {
                Name = name,
                Age = age,
                Rank = rank,
                Status = status,
                Mission = mission,
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
            ConsoleHelpers.PrintCharactersBrief(_characterService.GetAllCharactersBrief());
            Console.WriteLine("Enter Character ID(s) to delete (e.g., 1,2,3):");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Cancelled.");
                return;
            }
            var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var ids = new List<int>();
            foreach (var part in parts)
            {
                if (int.TryParse(part.Trim(), out var id))
                    ids.Add(id);
            }

            if (ids.Count == 0)
            {
                Console.WriteLine("No valid IDs entered. Cancelled.");
                return;
            }

            if (!ConsoleHelpers.Confirm($"This will permanently delete {ids.Count} character(s). Are you sure? [y/N]: "))
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            var deleted = new List<int>();
            var failed = new List<(int Id, string err)>();

            foreach (var id in ids)
            {
                if (_characterService.DeleteCharacter(id, out var err))
                    deleted.Add(id);
                else
                    failed.Add((id, err));
            }

            if (deleted.Count > 0)
                Console.WriteLine($"Successfully deleted characters: {string.Join(", ", deleted)}");
            if (failed.Count > 0)
            {
                Console.WriteLine("Failed to delete the following characters:");
                foreach (var (id, err) in failed)
                {
                    Console.WriteLine($" - ID {id}: {err}");
                }
            }
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

            var (newChar, selectedTraitIds, educationTrait, personalityTraits) = _charGenService.Generate();

            if (_characterService.CreateCharacter(newChar, selectedTraitIds, out var err))
            {
                var saved = _characterService.GetCharacterById(newChar.Id)!;
                _wisdomService.TopUpWisdom(saved);
                Console.WriteLine("\n=== RANDOM CHARACTER ===");
                Console.WriteLine($"{newChar.Name}, {newChar.Age} years old");
                Console.WriteLine($"Rank: {newChar.Rank}, Status: {newChar.Status}");

                Console.WriteLine("\nBase Attributes:");
                Console.WriteLine($"DIP: {newChar.BaseDiplomacy} | MAR: {newChar.BaseMartial} | STE: {newChar.BaseStewardship}");
                Console.WriteLine($"INT: {newChar.BaseIntrigue} | LEA: {newChar.BaseLearning} | PRO: {newChar.BaseProwess}");

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

        private static void ResetMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Dev Reset Menu ===");

                Console.WriteLine("1) Reset CHARACTERS only");
                Console.WriteLine("2) Reset TRAITS only");
                Console.WriteLine("3) Reset HOLDINGS only");
                Console.WriteLine("4) FULL reset");
                Console.WriteLine("0) Back");
                Console.Write("Choice: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        _dbReset.ResetCharacters();
                        Pause();
                        break;
                    case "2":
                        _dbReset.ResetTraits();
                        Pause();
                        break;
                    case "3":
                        _dbReset.ResetHoldings();
                        Pause();
                        break;
                    case "4":
                        _dbReset.FullReset();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

    

        // Helpers to read optional inputs

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

    }
}

