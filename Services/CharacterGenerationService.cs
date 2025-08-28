using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSolamnia
{
    /// <summary>
    /// Character generation logic based on defined probabilities and trait assignments.
    /// This service creates a character with randomized attributes, age, name, and traits.
    /// </summary>
    public class CharacterGenerationService
    {
        private readonly TraitService _traitService;
        private readonly Random _rng = new Random();

        public CharacterGenerationService(TraitService traitService)
        {
            _traitService = traitService;
        }

        // Main method to generate a character
        public (Character character, List<int> traitIds, Trait? educationTrait, List<Trait> personalityTraits) Generate()
        {
            // Attribute roll according to defined probabilities
            int RollBaseAttribute()
            {
                int roll = _rng.Next(1, 10001);
                return roll switch
                {
                    <= 277  => 2,  // 2.77%
                    <= 832  => 3,  // 5.55%
                    <= 1665 => 4,  // 8.33%
                    <= 2776 => 5,  // 11.11%
                    <= 4164 => 6,  // 13.88%
                    <= 5830 => 7,  // 16.66%
                    <= 7218 => 8,  // 13.88%
                    <= 8329 => 9,  // 11.11%
                    <= 9162 => 10, // 8.33%
                    <= 9717 => 11, // 5.55%
                    _       => 12  // 2.83%
                };
            }

            // Age roll based on defined probabilities
            int age = _rng.Next(100) switch
            {
                < 15 => _rng.Next(16, 26), // 15% genç
                < 70 => _rng.Next(26, 46), // 55% prime
                < 90 => _rng.Next(46, 61), // 20% orta yaş
                _    => _rng.Next(61, 71)  // 10% yaşlı
            };

            // Character creation with rolled attributes
            var character = new Character
            {
                Name        = GetRandomName(),
                Age         = age,
                Rank        = "",
                Status      = StatusType.AD,
                ActiveDuty  = "",
                Diplomacy   = RollBaseAttribute(),
                Martial     = RollBaseAttribute(),
                Stewardship = RollBaseAttribute(),
                Intrigue    = RollBaseAttribute(),
                Learning    = RollBaseAttribute(),
                Prowess     = RollBaseAttribute(),
                Level       = _rng.Next(1, 6)
            };

            // Determine education type and select traits
            string educationType = _rng.Next(6) switch
            {
                0 => "Diplomacy",
                1 => "Martial",
                2 => "Stewardship",
                3 => "Intrigue",
                4 => "Learning",
                _ => "Prowess"
            };

            // Education trait selection based on category and tier probabilities
            var allTraits = _traitService.GetAllTraits();
            var educationTrait = allTraits
                .Where(t => t.Type == TraitType.Education)
                .Where(t => IsInEducationCategory(t, educationType))
                .Where(t => GetTierPredicate(educationType) (GetTier(t, educationType)))
                .OrderBy(_ => _rng.Next())
                .FirstOrDefault();

            // Select 3 random personality traits
            var personalityTraits = allTraits
                .Where(t => t.Type == TraitType.Personality)
                .OrderBy(_ => _rng.Next())
                .Take(3)
                .ToList();

            // Combine trait IDs
            var traitIds = new List<int>();
            if (educationTrait != null) traitIds.Add(educationTrait.Id);
            traitIds.AddRange(personalityTraits.Select(t => t.Id));

            return (character, traitIds, educationTrait, personalityTraits);

            // ---------- local helpers ----------
            bool IsInEducationCategory(Trait trait, string category) => category switch
            {
                "Diplomacy"   => trait.BonusDiplomacy  > 0 && trait.BonusMartial == 0,
                "Martial"     => trait.BonusMartial    > 0 && trait.BonusStewardship == 0,
                "Stewardship" => trait.BonusStewardship> 0 && trait.BonusIntrigue == 0,
                "Intrigue"    => trait.BonusIntrigue   > 0 && trait.BonusLearning == 0,
                "Learning"    => trait.BonusLearning   > 0 && trait.BonusProwess == 0,
                "Prowess"     => trait.BonusProwess    > 0,
                _ => false
            };

            // Get trait tier based on category
            int GetTier(Trait trait, string category) => category switch
            {
                "Diplomacy"   => trait.BonusDiplomacy   / 2,
                "Martial"     => trait.BonusMartial     / 2,
                "Stewardship" => trait.BonusStewardship / 2,
                "Intrigue"    => trait.BonusIntrigue    / 2,
                "Learning"    => trait.BonusLearning    / 2,
                "Prowess"     => trait.BonusProwess, // 
                _ => 1
            };

            // Get tier predicate based on defined probabilities
            Func<int, bool> GetTierPredicate(string cat)
            {
                int roll = _rng.Next(100);
                if (roll < 35) return t => t == 1;
                if (roll < 65) return t => t == 2;
                if (roll < 85) return t => t == 3;
                if (roll < 95) return t => t == 4;
                return t => t == 5;
            }
        }

        // Random name generation
        private string GetRandomName()
        {
            var isMale = _rng.Next(2) == 0;
            string firstName = isMale
                ? MaleNames[_rng.Next(MaleNames.Length)]
                : FemaleNames[_rng.Next(FemaleNames.Length)];
            string title = isMale ? "Sir" : "Dame";
            string home  = HomeLands.Length > 0 ? HomeLands[_rng.Next(HomeLands.Length)] : "Homeland";
            return $"{title} {firstName} {home}";
        }

        // --- DATA ---
        private static readonly string[] MaleNames = new[]
        {
            "Aaron","Abbo","Abel","Abraham","Absalom","Achard","Achilles","Acledulf","Aclefrid","Aclehard",
            "Acleman","Aclemund","Actard","Actwin","Adalald","Adalbald","Adalbod","Adalfrid","Adalgrim",
            "Adalhar","Adalhelm","Adalmar","Adalmund","Adalrad","Adalwald","Adam","Adelard","Ademar","Adolf",
            "Adrian","Adrulf","Ağda","Aicard","Ailbert","Ailhard","Ainard","Alain","Alaric","Alban","Alberic",
            "Albert","Albo","Aldebrand","Aldemar","Aldrich","Aldwin","Alexander","Alfgar","Alfhelm","Alfred",
            "Alfwin","Alphonse","Alric","Alvaro","Alwin","Amadeus","Ambrose","Amis","Ancel","Andrew","Anselm",
            "Ansgar","Anzo","Apollonius","Archibald","Aristotle","Arnold","Arnulf","Artald","Arthur","Athelstan",
            "Aubrey","Audoen","August","Aurelian","Aurelius","Austin","Averroes","Avo","Aylmer","Baldwin",
            "Balthasar","Barnabas","Bartholomew","Basil","Bastian","Benedict","Benjamin","Bernard","Berengar",
            "Bertram","Bjorn","Blaise","Bodo","Boguslav","Boleslav","Boniface","Boso","Brand","Brian","Brice",
            "Bruno","Cadell","Cadwallon","Caesar","Caius","Casimir","Cassian","Charles","Christian","Christopher",
            "Claudian","Conrad","Constantine","Corbinian","Crispin","Cumcision","Cuthbert","Cyprian","Cyril","Dagobert",
            "Damian","Daniel","David","Denis","Dietrich","Dominic","Donald","Drogo","Dunstan","Edgar","Edmund",
            "Edward","Edwin","Elias","Eliezer","Emery","Engelbert","Ephraim","Erik","Ernest","Eugene","Eustace",
            "Everard","Favian","Felix","Ferdinand","Fulk","Gabriel","Gawain","Geoffrey","George","Gerard",
            "Gervase","Gilbert","Giles","Godfrey","Godric","Godwin","Gregory","Grimbald","Gualter","Gunnar",
            "Guy","Harold","Hector","Henry","Herbert","Hildebrand","Hincmar","Hugh","Humbert","Humphrey",
            "Ivo","Jasper","Jerome","John","Jolan","Joseph","Joshua","Julian","Julius","Justus","Kenelm",
            "Lambert","Laurence","Leif","Leonard","Leopold","Lothar","Louis","Lucian","Ludovic","Magnus",
            "Malcolm","Marcus","Martin","Matthew","Maurice","Michael","Nicholas","Odo","Oliver","Orson",
            "Oswald","Otho","Otto","Pascal","Patrick","Paul","Percival","Peter","Philip","Raimond","Ralph",
            "Raymond","Reginald","Reinbald","Richard","Robert","Roderick","Roger","Roland","Rolf","Rupert",
            "Samson","Sebastian","Siegfried","Sigismund","Simon","Stephen","Tancred","Theobald","Theodore",
            "Theodoric","Thomas","Thurstan","Tiberius","Timothy","Tobias","Torsten","Tristan","Ulrich",
            "Ulysses","Valentin","Victor","Vincent","Virgil","Vitalis","Vivian","Waleran","Walter","Warin",
            "Wenceslas","Wilfred","William","Wulfric","Xavier","Yves","Zachary"
        };

        private static readonly string[] FemaleNames = new[]
        {
            "Adelaide","Adelina","Agatha","Agnes","Alba","Aldith","Alexandra","Alice","Amabel","Amalia",
            "Amice","Anastasia","Andrea","Angela","Anna","Anne","Avelina","Beatrice","Berenice","Brigid",
            "Cecilia","Clarimond","Constance","Drusilla","Eleanor","Elizabeth","Emmeline","Eugenia",
            "Euphemia","Felicia","Florence","Genevieve","Gisela","Gratiana","Helena","Hildegard","Idony",
            "Isabel","Joan","Juliana","Katherine","Leah","Lucia","Margaret","Maria","Matilda","Mirabel",
            "Olivia","Philippa","Rosamund","Sabina","Sophia","Theodora","Ursula","Valentina","Winifred","Ysabel"
        };

        private static readonly string[] HomeLands = new[]
        {
            "uth Duskhollow","uth Ironreach","uth Wolfshearth","uth Bleakmarsh","uth Rivenrock","uth Thornwold",
            "uth Shadowcrest","uth Stonemark","uth Kjeldur","uth Ulfdale","uth Hargoth’s Stand","uth Vexmire",
            "uth Grimspire","uth Duskrend","uth Frostbite","uth Raven’s Maw","uth Briarstoke","uth Witchmelt",
            "uth Ashthroat","uth Deadspan","uth Hearthscar","uth Blightstoke","uth Moorgrave","uth Blackmire",
            "uth Stoneharrow","uth Wyrmfen","uth Frostsink","uth Grimbreach","uth Vaelmoor","uth Stormcrag",
            "uth Caergoth","uth Edgerton","uth Harrying","uth Hamilton","uth Lockhart","uth Starport",
            "uth Stimpton","uth Restglen","uth Rening","uth O'Call","uth Di Estra","uth Firstward","uth Gorbie",
            "uth Wtdel","uth Ironrock","uth Portsmith","uth Deepdel","uth Gwyntarr","uth Lytburg","uth Thelgaard",
            "uth Brasdel","uth Luinstat","uth Sage","uth Kyre","uth Vex","uth Ravenscar","uth Cairngorn",
            "uth di Caela","uth Solanthus","uth Arnal","uth Patina","uth Tresvka","uth Hartford","uth Jansburg",
            "uth Auchunan","uth Egaard","uth Valoria","uth Forestedge","uth Delgaard","uth Relgoth","uth Ryn",
            "uth Arngrim","uth Brightblade","uth Southford","uth Naergoth","uth Tearford","uth Starmont",
            "uth Gaarlus","uth Navarre","uth Viranesh","uth Ligett","uth Vogler","uth Kalaman","uth Witdell",
            "uth Manydell","uth Gander","uth Hargoth","uth Winterholm","uth Potter's Mill","uth Korval",
            "uth Godnest","uth Palanthas","uth Dawnfort","uth Highrule","uth Varus","di Calea","de Montrefeltrp",
            "Boyle","Ashworth","Winslow","Pathwarden","Donner"
        };
    }
}
