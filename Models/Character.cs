namespace ProjectSolamnia{

    // character entity to represent individuals in the game world
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Rank { get; set; }
        public StatusType Status { get; set; }

        public int? AssignedHoldingId { get; set; }
        public Holding? AssignedHolding { get; set; }
        public string Mission { get; set; }
        public int Level { get; set; }

        public int BaseDiplomacy { get; set; }
        public int BaseMartial { get; set; }
        public int BaseStewardship { get; set; }
        public int BaseIntrigue { get; set; }
        public int BaseLearning { get; set; }
        public int BaseProwess { get; set; }


        // Wisdom bookkeeping
        public int? WisdomSeed { get; set; }                 // deterministic RNG seed
        public int WisdomPointsApplied { get; set; }         // how many wisdom points allocated so far

        // Frozen baseline used for ranking (base + 3 personality + 1 education at creation time)
        public int WisdomBaselineDip { get; set; }
        public int WisdomBaselineMar { get; set; }
        public int WisdomBaselineSte { get; set; }
        public int WisdomBaselineInt { get; set; }
        public int WisdomBaselineLea { get; set; }
        public int WisdomBaselinePro { get; set; }

        // Persisted wisdom allocations per stat (do NOT change core attributes)
        public int WisDip { get; set; }
        public int WisMar { get; set; }
        public int WisSte { get; set; }
        public int WisInt { get; set; }
        public int WisLea { get; set; }
        public int WisPro { get; set; }


        public ICollection<CharacterTrait> CharacterTraits { get; set; } = new List<CharacterTrait>(); //trait sayfasındaki ile aynı durum
    }
}