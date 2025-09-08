using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProjectSolamnia
{
    /// <summary>
    /// Character generation logic based on defined probabilities and trait assignments.
    /// This service creates a character with randomized attributes, age, name, and traits.
    /// </summary>
    public class CharacterGenerationService
    {
        private readonly struct Stats
        {
            public readonly int Dip, Mar, Ste, Int, Lea, Pro;
            public Stats(int dip, int mar, int ste, int intr, int lea, int pro)
                => (Dip, Mar, Ste, Int, Lea, Pro) = (dip, mar, ste, intr, lea, pro);

            public static Stats operator +(Stats a, Stats b)
                => new(a.Dip + b.Dip, a.Mar + b.Mar, a.Ste + b.Ste, a.Int + b.Int, a.Lea + b.Lea, a.Pro + b.Pro);
        }

        private readonly TraitService _traitService;
        private readonly Random _rng;

        public CharacterGenerationService(TraitService traitService)
        {
            _traitService = traitService;
            _rng = new Random();
        }

        // Main method to generate a character
        public (Character character, List<int> traitIds, Trait? educationTrait, List<Trait> personalityTraits) Generate()
        {
            var character = new Character
            {
                Name        = GetRandomName(),
                Age         = Age(),
                Rank        = "",
                Status      = StatusType.AD,
                Mission     = "",
                BaseDiplomacy   = RollBaseAttribute(),
                BaseMartial     = RollBaseAttribute(),
                BaseStewardship = RollBaseAttribute(),
                BaseIntrigue    = RollBaseAttribute(),
                BaseLearning    = RollBaseAttribute(),
                BaseProwess     = RollBaseAttribute(),
                Level       = _rng.Next(1, 6)
            };

            var allTraits = _traitService.GetAllTraits();

            var pickedPersonality = Pick3ExclusivePersonalityTraits();
            var baseSnap = new Stats(
                character.BaseDiplomacy,
                character.BaseMartial,
                character.BaseStewardship,
                character.BaseIntrigue,
                character.BaseLearning,
                character.BaseProwess
            );

            var perssonalitySnap =baseSnap + SumBonuses(pickedPersonality);

            // choose education category once, then pick a tier once
            var category = EducationCategoryBiased(perssonalitySnap);
            var tierPredicate = GetTierPredicate(category);

            // Build once
            var allEdu = allTraits.Where(t => t.Type == TraitType.Education).ToList();

            // Pool for the chosen category (by PRIMARY stat)
            var eduPool = allEdu.Where(t => IsInEducationCategory(t, category)).ToList();

            // Try: category + tier
            var educationTrait = eduPool
                .Where(t => tierPredicate(GetTier(t, category)))
                .OrderBy(_ => _rng.Next())
                .FirstOrDefault();

            // Fallback 1: category any tier
            if (educationTrait == null && eduPool.Count > 0)
                educationTrait = eduPool.OrderBy(_ => _rng.Next()).FirstOrDefault();

            // Fallback 2: any education (DB may be incomplete)
            if (educationTrait == null)
                educationTrait = allEdu.OrderBy(_ => _rng.Next()).FirstOrDefault();

            if (educationTrait == null)
                throw new InvalidOperationException("No Education traits exist in DB. Add at least one.");


            var traitIds = new List<int>();
            if (educationTrait != null) traitIds.Add(educationTrait.Id);
            traitIds.AddRange(pickedPersonality.Select(t => t.Id));

            return (character, traitIds, educationTrait, pickedPersonality);
        }


        // ---------HELPERS---------

        // Roll 2d6 for base attribute
        private int RollBaseAttribute()
        {

            int rollA = _rng.Next(1, 7);
            int rollB = _rng.Next(1, 7);

            int v = rollA + rollB;
            if (v < 2 || v > 12) throw
                new Exception("Invalid roll");
            return v;
        }

        // Determine age based on defined probabilities 
        private int Age()
        {
            int roll = _rng.Next(100);
            if (roll < 15)
                return _rng.Next(16, 26); // 15% young adult
            else if (roll < 70)
                return _rng.Next(26, 46); // 55% adult
            else if (roll < 90)
                return _rng.Next(46, 61); // 20% middle-aged
            else if (roll < 98)
                return _rng.Next(61, 71); // 8% old
            else
                return _rng.Next(71, 86); // 2% venerable
        }

        //Pick 3 non-exclusive personality traits
        private List<Trait> Pick3ExclusivePersonalityTraits()
        {
            var pool = _traitService.GetAllTraits()
                .Where(t => t.Type == TraitType.Personality)
                .ToList();

            while (true)
            {
                var pick = pool
                    .OrderBy(_ => _rng.Next())
                    .Take(3)
                    .ToList();

                if (_traitService.ValidateTraits(pick, out _))
                    return pick;
            }
            throw new Exception("Failed to pick 3 exclusive personality traits");
        }
        
        private (string top1, string top2) GetTopAttributesFromSnapshot(Stats s)
        {
            var pairs = new List<(string key, int val, double tie)>
            {
                ("Diplomacy", s.Dip, _rng.NextDouble()),
                ("Martial",   s.Mar, _rng.NextDouble()),
                ("Stewardship", s.Ste, _rng.NextDouble()),
                ("Intrigue",  s.Int, _rng.NextDouble()),
                ("Learning",  s.Lea, _rng.NextDouble()),
                ("Prowess",   s.Pro, _rng.NextDouble())
            };
            var sorted = pairs.OrderByDescending(p => p.val).ThenByDescending(p => p.tie).ToList();
            return (sorted[0].key, sorted[1].key);
        }


        private static Stats SumBonuses(IEnumerable<Trait> traits)
        {
            int d = 0, m = 0, s = 0, i = 0, l = 0, p = 0;
            foreach (var t in traits)
            {
                d += t.BonusDiplomacy;
                m += t.BonusMartial;
                s += t.BonusStewardship;
                i += t.BonusIntrigue;
                l += t.BonusLearning;
                p += t.BonusProwess;
            }
            return new Stats(d, m, s, i, l, p);
        }


        // Find the strongest attributes of the character
    //   private (string top1, string top2) GetTopAttributes(Character character)
    //   {
    //       var pairs = new List<(string key, int val, double tie)>
    //       {
    //           ("Diplomacy", character.Diplomacy, _rng.NextDouble()),
    //           ("Martial", character.Martial, _rng.NextDouble()),
    //           ("Stewardship", character.Stewardship, _rng.NextDouble()),
    //           ("Intrigue", character.Intrigue, _rng.NextDouble()),
    //           ("Learning", character.Learning, _rng.NextDouble()),
    //           ("Prowess", character.Prowess, _rng.NextDouble())
    //       };
    //       var sorted = pairs.OrderByDescending(p => p.val).ThenByDescending(p => p.tie).ToList();
    //       return (sorted[0].key, sorted[1].key);
    //   }

        // Bias education category towards top attributes
        // 60% top1, 25% top2, 15% random other
        private string EducationCategoryBiased(Stats snap)
        {
            var (top1, top2) = GetTopAttributesFromSnapshot(snap);
            int roll = _rng.Next(100);
            if (roll < 60) return top1;
            if (roll < 85) return top2;

            var others = new List<string> { "Diplomacy", "Martial", "Stewardship", "Intrigue", "Learning", "Prowess" };
            others.Remove(top1);
            others.Remove(top2);
            return others[_rng.Next(others.Count)];
        }
        
        // Get tier multiplier function based on category
        private Func<int, bool> GetTierPredicate(string category)
        {
            if (category == "Prowess")
            {
                int roll = _rng.Next(100);
                if (roll < 35) return t => t == 1;
                if (roll < 65) return t => t == 2;
                if (roll < 85) return t => t == 3;
                return t => t == 4;
            }
            else
            {
                int roll = _rng.Next(100);
                if (roll < 35) return t => t == 1;
                if (roll < 65) return t => t == 2;
                if (roll < 85) return t => t == 3;
                if (roll < 95) return t => t == 4;
                return t => t == 5;
            }
        }
        
        // Return the primary category for this trait (strictly largest positive bonus). Null if none or tie.
        private string? PrimaryCategoryOf(Trait t)
        {
            var pairs = new (string key, int val)[]
            {
                ("Diplomacy",   t.BonusDiplomacy),
                ("Martial",     t.BonusMartial),
                ("Stewardship", t.BonusStewardship),
                ("Intrigue",    t.BonusIntrigue),
                ("Learning",    t.BonusLearning),
                ("Prowess",     t.BonusProwess),
            };

            var max = pairs.Max(p => p.val);
            if (max <= 0) return null;

            var tops = pairs.Where(p => p.val == max).ToList();
            return tops.Count == 1 ? tops[0].key : null; // exclude ties
        }

        // Education trait is in category iff its PRIMARY bonus matches that category
        private bool IsInEducationCategory(Trait t, string category)
        {
            if (t.Type != TraitType.Education) return false;
            var primary = PrimaryCategoryOf(t);
            return primary != null && primary == category;
        }

        
        // Map primary bonus to tier: non-prowess +2/+4/+6/+8/+10 → 1..5 ; Prowess 1..4 directly
        private int GetTier(Trait trait, string category)
        {
            int score = category switch
            {
                "Diplomacy"   => trait.BonusDiplomacy,
                "Martial"     => trait.BonusMartial,
                "Stewardship" => trait.BonusStewardship,
                "Intrigue"    => trait.BonusIntrigue,
                "Learning"    => trait.BonusLearning,
                "Prowess"     => trait.BonusProwess,
                _ => 0
            };

            if (category == "Prowess")
                return Math.Clamp(score, 1, 4);

            // Non-prowess education bonuses are even steps; map to 1..5
            int tier = (int)Math.Round(score / 2.0); // 2→1, 4→2, 6→3, 8→4, 10→5
            return Math.Clamp(tier, 1, 5);
        }


        // Random name generation
        private string GetRandomName()
        {
            var isMale = _rng.Next(2) == 0;
            string firstName = isMale
                ? MaleNames[_rng.Next(MaleNames.Length)]
                : FemaleNames[_rng.Next(FemaleNames.Length)];
            string title = isMale ? "Sir" : "Dame";
            string home = HomeLands.Length > 0 ? HomeLands[_rng.Next(HomeLands.Length)] : "Homeland";
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
