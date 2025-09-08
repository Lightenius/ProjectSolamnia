using Microsoft.EntityFrameworkCore;
namespace ProjectSolamnia
{
    public class DbReset
    {
        private readonly ProjectSolamniaDbContext _dbContext;

        public DbReset(ProjectSolamniaDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        /// <summary>Reset only Characters (and their join rows). Holdings & Traits remain.</summary>
        public void ResetCharacters()
        {
            Console.WriteLine("Reset CHARACTERS only. This deletes all Characters and their CharacterTraits.");
            if (!ConsoleHelpers.Confirm("Proceed? [y/N]: ")) { Console.WriteLine("Cancelled."); return; }
            CreateBackup();
            Console.WriteLine("Backup created. Proceeding with characters reset...");

            try
            {
                // If you’re worried about cascade with ExecuteDelete on SQLite, you can delete join rows first:
                _dbContext.CharacterTraits.ExecuteDelete();

                // Unassign holdings first (extra safety even though FK is SetNull on Character side)
                _dbContext.Characters.ExecuteUpdate(setters => setters.SetProperty(c => c.AssignedHoldingId, (int?)null));

                // Now delete characters
                _dbContext.Characters.ExecuteDelete();

                try
                {
                    _dbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Characters';");
                }
                catch{ }

                Console.WriteLine("All Characters deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: " + ex.Message);
            }
        }

        /// <summary>Reset only Traits. Characters remain; their CharacterTraits are removed first. Also clears TraitExclusive.</summary>
        public void ResetTraits()
        {
            Console.WriteLine("Reset TRAITS only. This deletes all Traits, their Exclusives, and CharacterTraits links.");
            if (!ConsoleHelpers.Confirm("Proceed? [y/N]: ")) { Console.WriteLine("Cancelled."); return; }
            CreateBackup();
            Console.WriteLine("Backup created. Proceeding with traits reset...");

            try
            {
                // Must remove CharacterTraits first (FK to Trait is Restrict)
                _dbContext.CharacterTraits.ExecuteDelete();

                // Remove exclusivity relations (both sides are Restrict, so delete all)
                _dbContext.TraitExclusives.ExecuteDelete();

                // Now remove traits
                _dbContext.Traits.ExecuteDelete();

                try
                {
                    _dbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Traits';");
                }

                catch { }


                Console.WriteLine("All Traits and related links deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: " + ex.Message);
            }
        }

        /// <summary>Reset only Holdings. Characters remain but are unassigned from holdings.</summary>
        public void ResetHoldings()
        {
            Console.WriteLine("Reset HOLDINGS only. This deletes all Holdings and unassigns Characters from them.");
            if (!ConsoleHelpers.Confirm("Proceed? [y/N]: ")) { Console.WriteLine("Cancelled."); return; }
            CreateBackup();
            Console.WriteLine("Backup created. Proceeding with holdings reset...");

            try
            {
                // Proactively detach characters from holdings (safer than relying on SetNull during bulk delete)
                _dbContext.Characters.ExecuteUpdate(setters => setters.SetProperty(c => c.AssignedHoldingId, (int?)null));

                // Now delete holdings
                _dbContext.Holdings.ExecuteDelete();

                try
                {
                    _dbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Traits';");
                }

                catch { }

                Console.WriteLine("All Holdings deleted, characters unassigned.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: " + ex.Message);
            }
        }
        public void FullReset()
        {
            Console.WriteLine("FULL RESET: This will DROP and RECREATE the database.");
            if (!ConsoleHelpers.Confirm("Are you absolutely sure? [y/N]: ")) { Console.WriteLine("Cancelled."); return; }
            CreateBackup();
            Console.WriteLine("Backup created. Proceeding with full reset...");

            try
            {

                _dbContext.ChangeTracker.Clear();
                try { _dbContext.Database.GetDbConnection().Close(); } catch { /* ignore */ }

                Console.WriteLine("Dropping database...");
                _dbContext.Database.EnsureDeleted();

                Console.WriteLine("Recreating schema (migrations)...");
                _dbContext.Database.Migrate();

                // Optionally call a seeder here if you have one:
                // SeedBaselineData();

                Console.WriteLine("Full reset completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: " + ex.Message);
            }
        }
        private void CreateBackup()
        {
            const string dbPath = "solamnia.db";
            if (File.Exists(dbPath))
            {
                var ts = DateTime.Now.ToString("yyyyMMdd-HHmmssfff");
                var backupPath = $"solamnia.backup-{ts}.db";
                File.Copy(dbPath, backupPath, overwrite: false);
                Console.WriteLine($"Backup created: {backupPath}");
            }
            else
            {
                Console.WriteLine("No existing DB file found; skipping backup.");
            }
        }

    }
}
