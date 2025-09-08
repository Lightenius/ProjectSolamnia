namespace ProjectSolamnia{

// Junction entity to represent the many-to-many relationship between Characters and Traits
    public class CharacterTrait
    {
        public int CharacterId { get; set; }
        public required Character Character { get; set; }

        public int TraitId { get; set; }
        public Trait Trait { get; set; }
    }
}