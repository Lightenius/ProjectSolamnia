namespace ProjectSolamnia{

// bu sayfa karakterlerle idleri birleştirmeye yarıyor
    public class CharacterTrait
    {
        public int CharacterId { get; set; }
        public required Character Character { get; set; }

        public int TraitId { get; set; }
        public Trait Trait { get; set; }
    }
}