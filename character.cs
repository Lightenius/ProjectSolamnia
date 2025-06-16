namespace ProjectSolamnia{

    //karakterleri define eden sayfa
    public class Character
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Rank { get; set; }
        public required string Status { get; set; }

        public ICollection<CharacterTrait> CharacterTraits { get; set; } = new List<CharacterTrait>(); //trait sayfasındaki ile aynı durum
    }
}