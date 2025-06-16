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
        public DbSet<CharacterTrait> CharacterTraits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterTrait>()
                .HasKey(ct => new { ct.CharacterId, ct.TraitId }); // aynı karakterin aynı Id almasını engelliyor

            modelBuilder.Entity<CharacterTrait>()
                .HasOne(ct => ct.Character)
                .WithMany(c => c.CharacterTraits)
                .HasForeignKey(ct => ct.CharacterId);

            modelBuilder.Entity<CharacterTrait>()
                .HasOne(ct => ct.Trait)               // her karakterin sadece kendisine ait bir CharacterTraiti olduğunu söylüyor
                .WithMany(t => t.CharacterTraits)    // her bir Charactertraitin pek çok trait taşıyabileceğini söylüyor
                .HasForeignKey(ct => ct.TraitId);    //galiba tablolar arası iletişimi sağlıyor ama emin değilim
        }
    }

}

