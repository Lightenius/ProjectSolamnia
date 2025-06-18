
using ProjectSolamnia;

namespace ProjectSolamnia{}
//buraya trait yaratımı kurallarını yazacağım

public class TraitService
{
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
}
    