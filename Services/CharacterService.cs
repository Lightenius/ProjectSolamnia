using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace ProjectSolamnia
{

    // Service to manage Character entities and their traits

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

        // Update using DTO
        public bool UpdateCharacter(int characterId, CharacterUpdateDto dto, List<int> traitIds, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                // Validate traits first
                var traits = LoadAndValidateTraits(traitIds, out errorMessage);
                if (traits == null) return false;


                var existingCharacter = _dbContext.Characters
                    .Include(c => c.CharacterTraits)
                    .FirstOrDefault(c => c.Id == characterId);

                if (existingCharacter == null)
                {
                    errorMessage = $"Character with ID {characterId} not found.";
                    return false;
                }

                dto.ApplyTo(existingCharacter);

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
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
                return false;
            }
        }

        // Update using full Character object (for both new and existing characters)
        public bool UpdateCharacter(Character character, List<int> traitIds, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                var traits = LoadAndValidateTraits(traitIds, out errorMessage);
                if (traits == null) return false;

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
                    //handle new character
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
                    existingCharacter.Mission = character.Mission;
                    existingCharacter.Level = character.Level;
                    existingCharacter.BaseDiplomacy = character.BaseDiplomacy;
                    existingCharacter.BaseMartial = character.BaseMartial;
                    existingCharacter.BaseStewardship = character.BaseStewardship;
                    existingCharacter.BaseIntrigue = character.BaseIntrigue;
                    existingCharacter.BaseLearning = character.BaseLearning;
                    existingCharacter.BaseProwess = character.BaseProwess;

                    // Update traits
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

        // Create new character        
        public bool CreateCharacter(Character character, List<int> traitIds, out string errorMessage)
        {
            errorMessage = "";
            character.Id = 0; // Ensure new character
            return UpdateCharacter(character, traitIds, out errorMessage);
        }

        // Delete character and associated traits
        public bool DeleteCharacter(int characterId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var local = _dbContext.Characters.Local.FirstOrDefault(c => c.Id == characterId);
                if (local != null) _dbContext.Entry(local).State = EntityState.Detached;

                var affected = _dbContext.Characters
                    .Where(c => c.Id == characterId)
                    .ExecuteDelete();

                if (affected == 0)
                {
                    errorMessage = "Character not found.";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
                return false;
            }
        }


        // Load and validate traits based on IDs
        private List<Trait>? LoadAndValidateTraits(List<int> traitIds, out string errorMessage)
        {
            errorMessage = "";

            var traits = _dbContext.Traits
                .Include(t => t.ExclusiveWithTraits)
                .ThenInclude(et => et.ExclusiveWithTrait)
                .Where(t => traitIds.Contains(t.Id))
                .ToList();

            var personalityCount = traits.Count(t => t.Type == TraitType.Personality);
            var educationCount = traits.Count(t => t.Type == TraitType.Education);

            if (personalityCount != 3 || educationCount != 1)
            {
                errorMessage = $"Invalid selection. Need exactly 3 Personality + 1 Education traits (you entered {personalityCount}+{educationCount}).";
                return null;
            }

            if (!_traitService.ValidateTraits(traits, out var exclusivityError))
            {
                errorMessage = exclusivityError;
                return null;
            }


            return traits;
        }
        public List<CharacterBriefDto> GetAllCharactersBrief()
        {
            return _dbContext.Characters
                .AsNoTracking()
                .Select(c => new CharacterBriefDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Level = c.Level,
                    Status = c.Status
                })
                .OrderBy(c => c.Id)
                .ToList();
        }


    }
}



