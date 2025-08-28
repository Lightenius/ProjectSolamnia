namespace ProjectSolamnia
{
    // Junction entity to represent mutually exclusive relationships between Traits
    
    public class TraitExclusive
    {
        public int TraitId { get; set; }
        public Trait Trait { get; set; }

        public int ExclusiveWithTraitId { get; set; }
        public Trait ExclusiveWithTrait { get; set; }
    }
}
