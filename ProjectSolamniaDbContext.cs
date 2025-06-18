using Microsoft.EntityFrameworkCore;

namespace ProjectSolamnia
{
    public class ProjectSolamniaDbContext : DbContext
    {
        public ProjectSolamniaDbContext(DbContextOptions<ProjectSolamniaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Trait> Traits { get; set; }
        public DbSet<Holding> Holdings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Character>().ToTable("Characters");
            modelBuilder.Entity<Trait>().ToTable("Traits");
            modelBuilder.Entity<Holding>().ToTable("Holdings");

        }



        public Character? getCharacter(int Id)
        {
            return Characters.FirstOrDefault(c => c.Id == Id);
        }

        public void addCharacter(Character c)
        {
            Characters.Add(c);
            SaveChanges();
        }
    }

}

