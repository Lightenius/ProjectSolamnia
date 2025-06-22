using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectSolamnia;

namespace ProjectSolamnia {}

//buraya karakter yaratımı kurallarını yazacağım
// burada new character ve update character yazmaktadır

public class CharacterService
{
    private readonly ProjectSolamniaDbContext _dbContext;
    private readonly TraitService _traitService;

    public CharacterService(ProjectSolamniaDbContext dbContext, TraitService traitService)
    {
        _dbContext = dbContext;
        _traitService = traitService;
    }
    
    public List<Character> GetAllCharacters()
    {
        return _dbContext.Characters
            .Include(c => c.CharacterTraits)
            .ThenInclude(ct => ct.Trait)
            .Include(c => c.AssignedHolding)
            .ToList();
    }
    public Character? GetCharacterById(int id)
    {
        return _dbContext.Characters
            .Include(c => c.CharacterTraits)
            .ThenInclude(ct => ct.Trait)
            .Include(c => c.AssignedHolding)
            .FirstOrDefault(c => c.Id == id);
    }
    
    public bool UpdateCharacter(Character character, List<int> traitIds, out string errorMessage)
    {
        errorMessage = "";
        
        try 
        {
            // Validate traits first
            var traits = _dbContext.Traits
                .Include(t => t.ExclusiveWithTraits)
                .ThenInclude(t => t.ExclusiveWithTrait)
                .Where(t => traitIds.Contains(t.Id))
                .ToList();

            Console.WriteLine($"Found traits: {string.Join(", ", traits.Select(t => $"{t.Id}:{t.Name}({t.Type})"))}");

            var personalityCount = traits.Count(t => t.Type == TraitType.Personality);
            var educationCount = traits.Count(t => t.Type == TraitType.Education);

            if (personalityCount != 3)
            {
                errorMessage = $"A character must have 3 Personality traits (you have {personalityCount}).";
                return false;
            }

            if (educationCount != 1)
            {
                errorMessage = $"A character must have 1 Education trait (you have {educationCount}).";
                return false;
            }

            Character? existingCharacter = null;

            if (character.Id != 0)
            {
                existingCharacter = _dbContext.Characters
                    .Include(c => c.CharacterTraits)
                    .FirstOrDefault(c => c.Id == character.Id);

                if (existingCharacter == null)
                {
                    errorMessage = $"Character with ID {character.Id} not found.";
                    return false;
                }
            }

            if (existingCharacter == null)
            {
                // Handle new character
                character.CharacterTraits = new List<CharacterTrait>();
                foreach (var t in traits)
                {
                    character.CharacterTraits.Add(new CharacterTrait
                    {
                        TraitId = t.Id,
                        Trait = t,
                        Character = character
                    });
                }
                _dbContext.Characters.Add(character);
            }
            else
            {
                // Handle existing character
                existingCharacter.Name = character.Name;
                existingCharacter.Age = character.Age;
                existingCharacter.Rank = character.Rank;
                existingCharacter.Status = character.Status;
                existingCharacter.AssignedHoldingId = character.AssignedHoldingId;
                existingCharacter.ActiveDuty = character.ActiveDuty;
                existingCharacter.Level = character.Level;
                existingCharacter.Diplomacy = character.Diplomacy;
                existingCharacter.Martial = character.Martial;
                existingCharacter.Stewardship = character.Stewardship;
                existingCharacter.Intrigue = character.Intrigue;
                existingCharacter.Learning = character.Learning;
                existingCharacter.Prowess = character.Prowess;

                // Clear existing traits and add new ones
                existingCharacter.CharacterTraits.Clear();
                foreach (var t in traits)
                {
                    existingCharacter.CharacterTraits.Add(new CharacterTrait
                    {
                        CharacterId = existingCharacter.Id,
                        TraitId = t.Id,
                        Character = existingCharacter,
                        Trait = t
                    });
                }
            }

            _dbContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
            return false;
        }
    }
    public bool DeleteCharacter(int characterId, out string errorMessage)
    {
        errorMessage = "";

        var character = _dbContext.Characters
            .Include(c => c.CharacterTraits)  // karakterin traitlerini de dahil eder
            .FirstOrDefault(c => c.Id == characterId);

        if (character == null)
        {
            errorMessage = "Character not found.";
            return false;
        }

        // bağlı traitleri temizler
        _dbContext.CharacterTraits.RemoveRange(character.CharacterTraits);

        // karakteri siler
        _dbContext.Characters.Remove(character);

        _dbContext.SaveChanges();
        return true;
    }

}

//Efektif karakter attributelarını belirleyen komut
//base statlarla trait bonuslarını birleştirir
public class EffectiveAttributeCalculator
{
    private static int GetWisdomBonus(Character character)
    {
        return (int)Math.Floor(2 * Math.Log(character.Age - 14));
    }

    private static int GetProwessAgeModifier(Character character)
    {
        if (character.Age <= 25) 
            return 1;  // Young bonus
        if (character.Age > 35) 
            return -(int)Math.Floor((character.Age - 35) / 8.0);  // Gradual decline
        return 0;
    }

    public static int EffectiveDiplomacy(Character character)
    {
        return character.Diplomacy 
               + character.CharacterTraits.Sum(ct => ct.Trait.BonusDiplomacy)
               + GetWisdomBonus(character);
    }

    public static int EffectiveMartial(Character character)
    {
        return character.Martial 
               + character.CharacterTraits.Sum(ct => ct.Trait.BonusMartial)
               + GetWisdomBonus(character);
    }

    public static int EffectiveStewardship(Character character)
    {
        return character.Stewardship 
               + character.CharacterTraits.Sum(ct => ct.Trait.BonusStewardship)
               + GetWisdomBonus(character);
    }

    public static int EffectiveIntrigue(Character character)
    {
        return character.Intrigue 
               + character.CharacterTraits.Sum(ct => ct.Trait.BonusIntrigue)
               + GetWisdomBonus(character);
    }

    public static int EffectiveLearning(Character character)
    {
        return character.Learning 
               + character.CharacterTraits.Sum(ct => ct.Trait.BonusLearning)
               + GetWisdomBonus(character);
    }

    public static int EffectiveProwess(Character character)
    {
        return character.Prowess 
               + character.CharacterTraits.Sum(ct => ct.Trait.BonusProwess)
               + character.Level 
               + GetProwessAgeModifier(character);
    }
}
    