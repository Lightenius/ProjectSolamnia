using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
    public bool UpdateCharacter(Character character, List<int> traidIds, out string errorMessage)
    {
        errorMessage = "NO";

        var traits = _dbContext.Traits
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
            existingCharacter.AssignedHolding = character.AssignedHolding;
            existingCharacter.Activeduty = character.Activeduty;
            existingCharacter.Diplomacy = character.Diplomacy;
            existingCharacter.Martial = character.Martial;
            existingCharacter.Stewardship = character.Stewardship;
            existingCharacter.Intigue = character.Intigue;
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
}