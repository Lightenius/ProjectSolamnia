using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ProjectSolamnia
{
    // Service to manage Trait entities and their exclusives
    public class TraitService
    {
        private readonly ProjectSolamniaDbContext _dbContext;

        public TraitService(ProjectSolamniaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Trait> GetAllTraits()
        {
            return _dbContext.Traits
                .Include(t => t.ExclusiveWithTraits)
                .ToList();
        }

        public Trait? GetTraitById(int id)
        {
            return _dbContext.Traits
                .Include(t => t.ExclusiveWithTraits)
                .FirstOrDefault(t => t.Id == id);
        }

        public void AddTrait(Trait trait)
        {
            _dbContext.Traits.Add(trait);
            _dbContext.SaveChanges();
        }

        public bool UpdateTrait(Trait trait, List<int> exclusiveTraitIds, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var existingTrait = _dbContext.Traits
                    .Include(t => t.ExclusiveWithTraits)
                    .FirstOrDefault(t => t.Id == trait.Id);

                if (existingTrait == null)
                {
                    errorMessage = "Trait not found";
                    return false;
                }

                // Update basic properties
                existingTrait.Name = trait.Name;
                existingTrait.Description = trait.Description;
                existingTrait.Type = trait.Type;
                existingTrait.ImageUrl = trait.ImageUrl;
                existingTrait.BonusDiplomacy = trait.BonusDiplomacy;
                existingTrait.BonusMartial = trait.BonusMartial;
                existingTrait.BonusStewardship = trait.BonusStewardship;
                existingTrait.BonusIntrigue = trait.BonusIntrigue;
                existingTrait.BonusLearning = trait.BonusLearning;
                existingTrait.BonusProwess = trait.BonusProwess;

                // Update exclusive traits (symmetric)
                var currentExclusives = existingTrait.ExclusiveWithTraits.ToList();
                var newExclusives = exclusiveTraitIds.Select(id => new TraitExclusive
                {
                    TraitId = trait.Id,
                    ExclusiveWithTraitId = id
                }).ToList();

                foreach (var existing in currentExclusives)
                {
                    if (!newExclusives.Any(ne => ne.ExclusiveWithTraitId == existing.ExclusiveWithTraitId))
                        _dbContext.TraitExclusives.Remove(existing);
                }

                foreach (var ne in newExclusives)
                {
                    if (!currentExclusives.Any(ce => ce.ExclusiveWithTraitId == ne.ExclusiveWithTraitId))
                        _dbContext.TraitExclusives.Add(ne);
                }

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        // AddTraitWithExclusives: eklerken exclusive ilişkilerini iki yönlü kur
        public void AddTraitWithExclusives(Trait trait, List<int> exclusiveWithTraitIds)
        {
            _dbContext.Traits.Add(trait);
            _dbContext.SaveChanges();

            foreach (var exclId in exclusiveWithTraitIds)
            {
                var relation1 = new TraitExclusive { TraitId = trait.Id, ExclusiveWithTraitId = exclId };
                var relation2 = new TraitExclusive { TraitId = exclId, ExclusiveWithTraitId = trait.Id };
                _dbContext.TraitExclusives.Add(relation1);
                _dbContext.TraitExclusives.Add(relation2);
            }

            _dbContext.SaveChanges();
        }

        public void DeleteTrait(int traitId)
        {
            var trait = _dbContext.Traits
                .Include(t => t.ExclusiveWithTraits)
                .FirstOrDefault(t => t.Id == traitId);

            if (trait != null)
            {
                _dbContext.TraitExclusives.RemoveRange(trait.ExclusiveWithTraits);
                _dbContext.Traits.Remove(trait);
                _dbContext.SaveChanges();
            }
        }

        // Exclusive conflict check
        public bool ValidateTraits(List<Trait> traits, out string errorMessage)
        {
            errorMessage = "";

            foreach (var trait in traits)
            {
                foreach (var exRelation in trait.ExclusiveWithTraits)
                {
                    if (traits.Any(t => t.Id == exRelation.ExclusiveWithTraitId))
                    {
                        var conflicting = traits.First(t => t.Id == exRelation.ExclusiveWithTraitId);
                        errorMessage = $"Trait '{trait.Name}' cannot coexist with '{conflicting.Name}'.";
                        return false;
                    }
                }
            }
            return true;
        }

        public Dictionary<TraitType, List<Trait>> GetAllTraitsGroupedByType()
        {
            return _dbContext.Traits
                .GroupBy(t => t.Type)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
        public static string FormatTraitBrief(Trait t)
        {
            string typeLabel = t.Type switch
            {
                TraitType.Personality => "Personality Trait",
                TraitType.Education => "Education Trait",
                _ => "Trait"
            };

            var parts = new List<string>();
            void Add(string name, int val)
            {
                if (val != 0) parts.Add($"{name} {(val > 0 ? "+" : "")}{val}");
            }

            Add("Diplomacy",  t.BonusDiplomacy);
            Add("Martial",    t.BonusMartial);
            Add("Stewardship",t.BonusStewardship);
            Add("Intrigue",   t.BonusIntrigue);
            Add("Learning",   t.BonusLearning);
            Add("Prowess",    t.BonusProwess);

            var bonuses = parts.Count > 0 ? string.Join(", ", parts) : "no attribute bonuses";
            return $"{t.Name}: {typeLabel}; {bonuses}";
        }
    }
}
