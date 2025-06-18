namespace ProjectSolamnia
{
    public class TraitExclusive
    {
        public int TraitId { get; set; }
        public Trait Trait { get; set; }

        public int ExclusiveWithTraitId { get; set; }
        public Trait ExclusiveWithTrait { get; set; }
    }
}
