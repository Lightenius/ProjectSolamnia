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
    
    public bool UpdateCharacter(Character character, List<int> traidIds, out string errorMessage)
    {
        errorMessage = "";

        var traits = _dbContext.Traits
        .Include(t => t.ExclusiveWithTraits)
        .Where(t => traidIds.Contains(t.Id))       //traitleri DB'den çekme
        .ToList();


        //girilen traitlerin kurala uyup uymadığını belirler
        if (!_traitService.ValidateTraits(traits, out errorMessage))
        {
            return false;
        }
        var existingCharacter = _dbContext.Characters
            .Include(c => c.CharacterTraits)
            .FirstOrDefault(c => c.Id == character.Id);


        // girilen karakterin var olup olmadığını bakar
        //varsa bilgilerini günceller
        //yoksa yeni karakter yaratır 
        // ya da öyle bişey yapar şu anda emin değilim hatalı olabilir
        if (existingCharacter == null)
        {
            _dbContext.Characters.Add(character);
            existingCharacter = character;
        }
        else
        {
            existingCharacter.Name = character.Name;
            existingCharacter.Age = character.Age;
            existingCharacter.Rank = character.Rank;
            existingCharacter.Status = character.Status;
            existingCharacter.AssignedHoldingId = character.AssignedHoldingId;
            existingCharacter.Activeduty = character.Activeduty;

            //Sadece base statları değiştiriyor
            existingCharacter.Diplomacy = character.Diplomacy;
            existingCharacter.Martial = character.Martial;
            existingCharacter.Stewardship = character.Stewardship;
            existingCharacter.Intrigue = character.Intrigue;
            existingCharacter.Learning = character.Learning;
            existingCharacter.Prowess = character.Prowess;
        }

        // Girdi traitler yukarıdaki kontrolden geçtiyse
        // Eski traitleri siler ve yerine yenilerini koyar
        // ama yani yanlış girilirse karakterin halihazırdaki traitlerini de silebilir
        //öyle bi durumda if kullanarak çözebiliriz sanırım
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

        //kayıt
        _dbContext.SaveChanges();
        return true;
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
    public static int EffectiveDiplomacy(Character character)
    {
        int baseValue = character.Diplomacy;
        int bonus = character.CharacterTraits
            .Select(ct => ct.Trait.BonusDiplomacy)
            .Sum();

        return baseValue + bonus;
    }
    public static int EffectiveMartial(Character character)
    {
        int baseValue = character.Martial;
        int bonus = character.CharacterTraits
            .Select(ct => ct.Trait.BonusMartial)
            .Sum();

        return baseValue + bonus;
    }
    public static int EffectiveStewardship(Character character)
    {
        int baseValue = character.Stewardship;
        int bonus = character.CharacterTraits
            .Select(ct => ct.Trait.BonusStewardship)
            .Sum();

        return baseValue + bonus;
    }
    public static int EffectiveIntrigue(Character character)
    {
        int baseValue = character.Intrigue;
        int bonus = character.CharacterTraits
            .Select(ct => ct.Trait.BonusIntrigue)
            .Sum();

        return baseValue + bonus;
    }
    public static int EffectiveLearning(Character character)
    {
        int baseValue = character.Learning;
        int bonus = character.CharacterTraits
            .Select(ct => ct.Trait.BonusLearning)
            .Sum();

        return baseValue + bonus;
    }
    public static int EffectiveProwess(Character character)
    {
        int baseValue = character.Prowess;
        int bonus = character.CharacterTraits
            .Select(ct => ct.Trait.BonusProwess)
            .Sum();

        return baseValue + bonus;
    }
    
}