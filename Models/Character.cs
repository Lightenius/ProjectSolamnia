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
        public string ActiveDuty { get; set; }
        public int Level { get; set; }

        public int Diplomacy { get; set; }
        public int Martial { get; set; }
        public int Stewardship { get; set; }
        public int Intrigue { get; set; }
        public int Learning { get; set; }
        public int Prowess { get; set; }

        public ICollection<CharacterTrait> CharacterTraits { get; set; } = new List<CharacterTrait>(); //trait sayfasındaki ile aynı durum
    }
}