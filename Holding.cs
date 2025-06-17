namespace ProjectSolamnia
{
    // Holdingler için referans entitysi

    public class Holding
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HoldingType Type { get; set; }
        public string? Region { get; set; }
        public string? Description { get; set; }
        public string? Supplies { get; set; }
        public int? TroopsCount { get; set; }

        public ICollection<Character> AssignedCharacters { get; set; } = new List<Character>();
    }
}
