using Microsoft.EntityFrameworkCore;
using ProjectSolamnia;

namespace ProjectSolamnia{}
//buraya trait yaratımı kurallarını yazacağım

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

    public void UpdateTrait(Trait trait)
    {
        _dbContext.Traits.Update(trait);
        _dbContext.SaveChanges();
    }

// AddTraitWithExclusives trait eklerken aynı zamanda exclusive traitlerle ilişkisini de kurar
// exclusiveWithTraitIds parametresi, traitin exclusive olduğu traitlerin ID'lerini alır
    public void AddTraitWithExclusives(Trait trait, List<int> exclusiveWithTraitIds)
    {
        _dbContext.Traits.Add(trait);
        _dbContext.SaveChanges();

        foreach (var exclId in exclusiveWithTraitIds)
        {
            var relation1 = new TraitExclusive
            {
                TraitId = trait.Id,
                ExclusiveWithTraitId = exclId
            };

            var relation2 = new TraitExclusive
            {
                TraitId = exclId,
                ExclusiveWithTraitId = trait.Id
            };

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
            // Silmeden önce bağlı exclusive'leri temizle
            _dbContext.TraitExclusives.RemoveRange(trait.ExclusiveWithTraits);

            _dbContext.Traits.Remove(trait);
            _dbContext.SaveChanges();
        }
    }


    //Validate Traits bir karakterin sahip olduğu traitlere uyup uymadığını kontrol eder
    // 3 personality trait 1 education trait olmazsa false verir
    // ve exclusive olduğu trait olursa false verir
    public bool ValidateTraits(List<Trait> traits, out string errorMessage)
    {
        errorMessage = "";

        var personalityCount = traits.Count(t => t.Type == TraitType.Personality);
        if (personalityCount != 3)
        {
            errorMessage = "A character must have 3 Personality traits.";
            return false;
        }
        var educationCount = traits.Count(t => t.Type == TraitType.Education);
        if (educationCount != 1)
        {
            errorMessage = "A character must have 1 Education traits.";
            return false;
        }
        //exclusive trait kontrolü
        //eğer traitlerin exclusive traiti varsa ve o traitler arasında varsa false verir
        foreach (var trait in traits)
        {
            foreach (var exRelation in trait.ExclusiveWithTraits)
            {
                if (traits.Any(t => t.Id == exRelation.ExclusiveWithTraitId))
                {
                    var ConflictingTrait = traits.First(t => t.Id == exRelation.ExclusiveWithTraitId);
                    errorMessage = $"Trait '{trait.Name}' cannot coexist with '{ConflictingTrait.Name}'.";
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

}
    