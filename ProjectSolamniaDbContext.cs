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
        public DbSet<TraitExclusive> TraitExclusives{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterTrait>()
                .HasKey(ct => new { ct.CharacterId, ct.TraitId }); // aynı karakterin aynı Id almasını engelliyor

            modelBuilder.Entity<CharacterTrait>()
                .HasOne(ct => ct.Character)
                .WithMany(c => c.CharacterTraits)
                .HasForeignKey(ct => ct.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharacterTrait>()
                .HasOne(ct => ct.Trait)               // her karakterin sadece kendisine ait bir CharacterTraiti olduğunu söylüyor
                .WithMany(t => t.CharacterTraits)    // her bir Charactertraitin pek çok trait taşıyabileceğini söylüyor
                .HasForeignKey(ct => ct.TraitId)   //galiba tablolar arası iletişimi sağlıyor ama emin değilim
                .OnDelete(DeleteBehavior.Restrict);

            //buradan sonrası exclusive traitlerle alakalı


            modelBuilder.Entity<TraitExclusive>()
                .HasKey(te => new { te.TraitId, te.ExclusiveWithTraitId });

            modelBuilder.Entity<TraitExclusive>()
                .HasOne(te => te.Trait)
                .WithMany(t => t.ExclusiveWithTraits)
                .HasForeignKey(te => te.TraitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TraitExclusive>()
                .HasOne(te => te.ExclusiveWithTrait)
                .WithMany()
                .HasForeignKey(te => te.ExclusiveWithTraitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

