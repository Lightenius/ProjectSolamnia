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
        public DbSet<Holding> Holdings { get; set; }

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

            modelBuilder.Entity<Holding>()
                .Property(h => h.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Holding>()
                .HasMany(h => h.AssignedCharacters)
                .WithOne(c => c.AssignedHolding)
                .HasForeignKey(c => c.AssignedHoldingId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Character>()
                .HasOne(c => c.AssignedHolding)
                .WithMany(h => h.AssignedCharacters)
                .HasForeignKey(c => c.AssignedHoldingId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Trait>().HasData(
                new Trait
                {
                    Id = 1,
                    Name = "Ambitious",
                    Type = TraitType.Personality,
                    Description = "This character burns with a relentless drive to rise above all others. They crave greatness and will stop at nothing to carve their name into history—even if it means stepping on friends, family, or tradition.",
                    BonusDiplomacy = 1,
                    BonusMartial = 1,
                    BonusStewardship = 1,
                    BonusIntrigue = 1,
                    BonusLearning = 1,
                    BonusProwess = 1
                },

                new Trait
                {
                    Id = 2,
                    Name = "Arbitrary",
                    Type = TraitType.Personality,
                    Description = "Justice and reason are strangers to this character. Their whims govern their rule, their moods define their verdicts. To serve them is to live in constant uncertainty.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 3,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 3,
                    Name = "Arrogant",
                    Type = TraitType.Personality,
                    Description = "Convinced of their own superiority, this character dismisses the opinions of others with a smirk. Humility is a word they’ve heard but never needed.",
                    BonusDiplomacy = 1,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 4,
                    Name = "Brave",
                    Type = TraitType.Personality,
                    Description = "Fear holds no sway over this character. They stand tall in the face of danger, charging into the fray with sword in hand and heart unyielding—even if wisdom says otherwise.",
                    BonusDiplomacy = 0,
                    BonusMartial = 2,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 3
                },

                new Trait
                {
                    Id = 5,
                    Name = "Callous",
                    Type = TraitType.Personality,
                    Description = "Empathy is weakness in this character’s eyes. They view pain and loss as tools, not tragedies, and rarely pause to consider the emotional toll of their actions.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 6,
                    Name = "Calm",
                    Type = TraitType.Personality,
                    Description = "Unshaken by chaos and unbothered by insult, this character is a pillar of serenity. Whether in war or council, their steady hand guides with grace.",
                    BonusDiplomacy = 1,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 1,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 7,
                    Name = "Chaste",
                    Type = TraitType.Personality,
                    Description = "This character resists earthly pleasures, placing purity and discipline above passion. Their restraint is both a virtue and, at times, a wedge between them and others.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 2,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 8,
                    Name = "Compassionate",
                    Type = TraitType.Personality,
                    Description = "A heart that aches for the suffering of others beats within this character. Their kindness may inspire loyalty, but it also exposes them to exploitation and sorrow.",
                    BonusDiplomacy = 2,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = -2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 9,
                    Name = "Content",
                    Type = TraitType.Personality,
                    Description = "This character desires little beyond what they already possess. Their peaceful nature brings them happiness—but ambitionless calm can be a dangerous flaw in a turbulent world.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = -1,
                    BonusLearning = 2,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 10,
                    Name = "Craven",
                    Type = TraitType.Personality,
                    Description = "Self-preservation guides every step of this character. They shrink from conflict and avoid risk, preferring to survive in the shadows rather than die in the light.",
                    BonusDiplomacy = 0,
                    BonusMartial = -2,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = -3
                },

                new Trait
                {
                    Id = 11,
                    Name = "Deceitful",
                    Type = TraitType.Personality,
                    Description = "This character weaves lies like a spider spins webs. Deception is their tool, and truth is a burden they rarely carry. To trust them is to invite betrayal.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 4,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 12,
                    Name = "Diligent",
                    Type = TraitType.Personality,
                    Description = "Industrious and relentless, this character pours their soul into every task. Their work ethic inspires others, but it can also lead them to exhaustion.",
                    BonusDiplomacy = 1,
                    BonusMartial = 1,
                    BonusStewardship = 1,
                    BonusIntrigue = 1,
                    BonusLearning = 1,
                    BonusProwess = 1
                },

                new Trait
                {
                    Id = 13,
                    Name = "Eccentric",
                    Type = TraitType.Personality,
                    Description = "Unbound by norms, this character marches to a beat only they can hear. Their odd behavior confounds others, sometimes charming, often confusing.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 14,
                    Name = "Fickle",
                    Type = TraitType.Personality,
                    Description = "Inconstant and changeable, this character’s loyalties and moods shift like the wind. Their unpredictability makes alliances both risky and fleeting.",
                    BonusDiplomacy = 1,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 15,
                    Name = "Forgiving",
                    Type = TraitType.Personality,
                    Description = "This character’s heart is quick to pardon, even when justice might demand otherwise. Their mercy heals wounds, but it can also enable wrongdoing.",
                    BonusDiplomacy = 2,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = -2,
                    BonusLearning = 1,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 16,
                    Name = "Generous",
                    Type = TraitType.Personality,
                    Description = "Gold flows from this character’s hands like water. Whether out of kindness or a desire to be loved, they give freely, sometimes to their own detriment.",
                    BonusDiplomacy = 3,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 17,
                    Name = "Gluttonous",
                    Type = TraitType.Personality,
                    Description = "Pleasure and indulgence are this character’s constant companions. They feast, drink, and consume without thought for moderation or judgment.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 18,
                    Name = "Greedy",
                    Type = TraitType.Personality,
                    Description = "This character hungers for wealth above all else. Coins are their closest companions, and they are rarely seen giving without expecting more in return.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 19,
                    Name = "Gregarious",
                    Type = TraitType.Personality,
                    Description = "Charming and outgoing, this character thrives in the company of others. Their laughter is infectious, their presence magnetic.",
                    BonusDiplomacy = 2,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 20,
                    Name = "Honest",
                    Type = TraitType.Personality,
                    Description = "This character values truth as a sacred duty. Their candor is refreshing to some and dangerous to others, especially in a world ruled by secrets.",
                    BonusDiplomacy = 2,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = -4,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 21,
                    Name = "Humble",
                    Type = TraitType.Personality,
                    Description = "This character carries little pride, preferring to downplay their own achievements. Their humility is admirable—but it can be mistaken for weakness.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 22,
                    Name = "Impatient",
                    Type = TraitType.Personality,
                    Description = "This character has little tolerance for delay or indecision. They act quickly—sometimes recklessly—and despise idle waiting or long debates.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 23,
                    Name = "Just",
                    Type = TraitType.Personality,
                    Description = "Guided by law and fairness, this character strives to do what is right—even when the cost is high. Justice is their compass, and compromise is not in their nature.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 2,
                    BonusIntrigue = -3,
                    BonusLearning = 1,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 24,
                    Name = "Lustful",
                    Type = TraitType.Personality,
                    Description = "Driven by carnal desires, this character embraces passion without restraint. Whether through charm or indulgence, they rarely deny themselves pleasure.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 25,
                    Name = "Lazy",
                    Type = TraitType.Personality,
                    Description = "This character avoids effort whenever possible, preferring rest over toil. Duties are often neglected in favor of comfort and ease.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 26,
                    Name = "Paranoid",
                    Type = TraitType.Personality,
                    Description = "Suspicious of all, this character sees plots and betrayal around every corner. Trust is a foreign concept—and fear is their only constant ally.",
                    BonusDiplomacy = 0,
                    BonusMartial = 3,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 27,
                    Name = "Patient",
                    Type = TraitType.Personality,
                    Description = "Slow to anger and deliberate in action, this character believes that good things come to those who wait—and they are always willing to wait.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 2,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 28,
                    Name = "Sadistic",
                    Type = TraitType.Personality,
                    Description = "This character finds satisfaction in the suffering of others. Whether through cruelty or vengeance, they revel in power over pain.",
                    BonusDiplomacy = 0,
                    BonusMartial = 2,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = 1
                },

                new Trait
                {
                    Id = 29,
                    Name = "Shy",
                    Type = TraitType.Personality,
                    Description = "Quiet and reserved, this character avoids the spotlight. Crowds are discomforting, and small talk is a source of anxiety.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 30,
                    Name = "Stubborn",
                    Type = TraitType.Personality,
                    Description = "Once this character has made up their mind, it is nearly impossible to sway them. Their resolve is ironclad, for better or worse.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 3,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 31,
                    Name = "Temperate",
                    Type = TraitType.Personality,
                    Description = "Moderation defines this character’s habits. They avoid indulgence and temptation, living a life of balance and restraint.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 2,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 32,
                    Name = "Trusting",
                    Type = TraitType.Personality,
                    Description = "Naive or idealistic, this character gives others the benefit of the doubt. Unfortunately, this often leads them into dangerous traps.",
                    BonusDiplomacy = 2,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = -2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 33,
                    Name = "Vengeful",
                    Type = TraitType.Personality,
                    Description = "This character never forgets a slight. Grudges are cherished like treasures, and revenge is a promise they always intend to keep.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 34,
                    Name = "Wrathful",
                    Type = TraitType.Personality,
                    Description = "Quick-tempered and easily provoked, this character is a storm waiting to explode. Their fury inspires fear—and often regret.",
                    BonusDiplomacy = -1,
                    BonusMartial = 2,
                    BonusStewardship = 0,
                    BonusIntrigue = -1,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 35,
                    Name = "Zealous",
                    Type = TraitType.Personality,
                    Description = "Fanatical in their beliefs, this character serves a higher power with unwavering devotion. To them, heresy is not a difference of opinion—it's a crime.",
                    BonusDiplomacy = 0,
                    BonusMartial = 2,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 36,
                    Name = "Naive Appeaser",
                    Type = TraitType.Education,
                    Description = "Though trained in diplomacy, this character’s clumsy attempts at negotiation often result in embarrassment. Their intentions are good, but the execution falls painfully short.",
                    BonusDiplomacy = 2,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 37,
                    Name = "Adequate Bargainer",
                    Type = TraitType.Education,
                    Description = "This character has found some success in the diplomatic arena, though it’s unclear whether it’s through talent or sheer luck. Still, they hold their own in courtly matters.",
                    BonusDiplomacy = 4,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 38,
                    Name = "Charismatic Negotiator",
                    Type = TraitType.Education,
                    Description = "Gifted with silvered words and natural charm, this character navigates diplomacy with confidence. Courtiers hang on their every word, and few can deny their appeals.",
                    BonusDiplomacy = 6,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 39,
                    Name = "Grey Eminence",
                    Type = TraitType.Education,
                    Description = "This master of diplomacy commands influence behind the scenes with unmatched skill. Subtle and refined, they bend even the most stubborn wills without lifting a sword.",
                    BonusDiplomacy = 8,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 40,
                    Name = "Virtuoso Arbitrator",
                    Type = TraitType.Education,
                    Description = "With eloquence honed to a fine edge, this character can untangle the most volatile disputes and charm the bitterest rivals. Diplomacy isn’t just a skill—it’s their art.",
                    BonusDiplomacy = 10,
                    BonusMartial = 3,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 41,
                    Name = "Misguided Warrior",
                    Type = TraitType.Education,
                    Description = "Armed with theory but lacking in practice, this character's martial education is more of a liability than an asset. They often do more harm to themselves than their foes.",
                    BonusDiplomacy = 0,
                    BonusMartial = 2,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 42,
                    Name = "Tough Soldier",
                    Type = TraitType.Education,
                    Description = "This character has a working grasp of martial matters and some experience in the field, though they still have much to learn before mastering the art of war.",
                    BonusDiplomacy = 0,
                    BonusMartial = 4,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 43,
                    Name = "Skilled Tactician",
                    Type = TraitType.Education,
                    Description = "A proven battlefield leader, this character commands with confidence and knows how to turn the tide of war through calculated tactics.",
                    BonusDiplomacy = 0,
                    BonusMartial = 6,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 44,
                    Name = "Brilliant Strategist",
                    Type = TraitType.Education,
                    Description = "War is a game, and this character is a grandmaster. No martial detail escapes their attention, and every campaign is a well-orchestrated symphony of steel.",
                    BonusDiplomacy = 0,
                    BonusMartial = 8,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 45,
                    Name = "Exalted Warlord",
                    Type = TraitType.Education,
                    Description = "Renowned across kingdoms, this character leads armies with unmatched brilliance. Their feats in battle are the stuff of legend, and entire wars bend to their will.",
                    BonusDiplomacy = 0,
                    BonusMartial = 10,
                    BonusStewardship = 3,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 46,
                    Name = "Indulgent Wastrel",
                    Type = TraitType.Education,
                    Description = "Educated in stewardship but devoid of restraint, this character is more likely to spend wealth than manage it. Prosperity slips through their fingers like sand.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 2,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 47,
                    Name = "Thrifty Clerk",
                    Type = TraitType.Education,
                    Description = "Competent and careful, this character can manage a budget and oversee estates, even if their talents occasionally fall short.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 4,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 48,
                    Name = "Fortune Builder",
                    Type = TraitType.Education,
                    Description = "Savvy and detail-oriented, this character excels in managing lands and treasuries, turning modest ventures into wealth-generating enterprises.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 6,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 49,
                    Name = "Midas Touched",
                    Type = TraitType.Education,
                    Description = "Everything this character touches turns to gold. A paragon of stewardship, they manage resources with unrivaled efficiency and vision.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 8,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 50,
                    Name = "Golden Sovereign",
                    Type = TraitType.Education,
                    Description = "An economic mastermind, this character elevates even failing realms into beacons of prosperity. Their brilliance shines brightest in coin and crown.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 10,
                    BonusIntrigue = 0,
                    BonusLearning = 3,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 51,
                    Name = "Amateurish Plotter",
                    Type = TraitType.Education,
                    Description = "While trained in subterfuge, this character often blunders their schemes. Their plots are usually transparent and ineffective, but at least they try.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 2,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 52,
                    Name = "Flamboyant Trickster",
                    Type = TraitType.Education,
                    Description = "Dramatic and cunning, this character knows their way around intrigue but often takes flashy risks that may backfire.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 4,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 53,
                    Name = "Intricate Webweaver",
                    Type = TraitType.Education,
                    Description = "This character has a mind for manipulation and secrets, spinning lies as easily as others breathe. Their reach is long and subtle.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 6,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 54,
                    Name = "Elusive Shadow",
                    Type = TraitType.Education,
                    Description = "Few have seen their true face. This master of secrecy and deception leaves little trace, ruling through whispers and threats.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 8,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 55,
                    Name = "Conniving Puppetmaster",
                    Type = TraitType.Education,
                    Description = "Every move this character makes pulls on a hidden string. Their mastery of intrigue rivals kings—rulers may sit on the throne, but this one pulls the strings.",
                    BonusDiplomacy = 3,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 10,
                    BonusLearning = 0,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 56,
                    Name = "Conscientious Scribe",
                    Type = TraitType.Education,
                    Description = "Their grasp of letters is solid, but beyond that lies confusion. This character has studied learning, yet true insight continues to elude them.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 2,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 57,
                    Name = "Insightful Thinker",
                    Type = TraitType.Education,
                    Description = "This character has cultivated a respectable understanding of faith, reason, and the written word. They are a thoughtful student of the world’s mysteries.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 4,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 58,
                    Name = "Astute Intellectual",
                    Type = TraitType.Education,
                    Description = "Possessing both curiosity and discipline, this character dives deep into philosophical and theological debates, often leading scholars in discussion.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 6,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 59,
                    Name = "Mastermind Philosopher",
                    Type = TraitType.Education,
                    Description = "This brilliant mind sees connections others miss. Their insights shape doctrine and reform, often setting the course of intellectual history.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 8,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 60,
                    Name = "Erudite Oracle",
                    Type = TraitType.Education,
                    Description = "Revered in scholarly and spiritual circles alike, this character’s wisdom guides kings and clergy. They are a living library of divine and secular knowledge.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 3,
                    BonusLearning = 10,
                    BonusProwess = 0
                },

                new Trait
                {
                    Id = 61,
                    Name = "Bumbling Squire",
                    Type = TraitType.Education,
                    Description = "Though technically trained, this character struggles with the basics of combat. Their clumsy swordplay is more likely to amuse than intimidate.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 1
                },

                new Trait
                {
                    Id = 62,
                    Name = "Confident Knight",
                    Type = TraitType.Education,
                    Description = "Having earned their spurs, this character is a competent warrior who knows their way around a battlefield and commands respect in melee.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 2
                },

                new Trait
                {
                    Id = 63,
                    Name = "Formidable Banneret",
                    Type = TraitType.Education,
                    Description = "Tested in raids and skirmishes, this character leads with grit and strikes with power. Their presence inspires their comrades to press on.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 3
                },

                new Trait
                {
                    Id = 64,
                    Name = "Famous Champion",
                    Type = TraitType.Education,
                    Description = "A legend among warriors, this character’s name echoes across arenas and battlefields. Tales of their feats inspire awe—and fear.",
                    BonusDiplomacy = 0,
                    BonusMartial = 0,
                    BonusStewardship = 0,
                    BonusIntrigue = 0,
                    BonusLearning = 0,
                    BonusProwess = 4
                }



            );

        }
    }

}

