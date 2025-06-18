namespace ProjectSolamnia {

    //traitleri define eden sayfaı 

    public class Trait
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public TraitType Type { get; set; }
        public string? Description { get; set; }  //anladığım kadarıyla string? yazmazsan boş değer veremiyorsun
        public string? ImageUrl { get; set; }


        public int BonusDiplomacy { get; set; }
        public int BonusMartial { get; set; }
        public int BonusStewardship { get; set; }
        public int BonusIntrigue { get; set; }
        public int BonusLearning { get; set; }
        public int BonusProwess { get; set; }


        public ICollection<CharacterTrait> CharacterTraits { get; set; } = new List<CharacterTrait>();  // burada ne yapacağımı bilemedim karakterin birden fazla traite sahip olması gerekiyo sahip olduğu traitleri de trait1 trait2 şeklinde yapmak pek mümkün olmadığından ötürü chatgptye sordum bu kodu verdi ama hala tam anlamadım.
        public ICollection<TraitExclusive> ExclusiveWithTraits { get; set; } = new List<TraitExclusive>();
        

    }
}