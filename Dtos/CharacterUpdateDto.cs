namespace ProjectSolamnia;
public class CharacterUpdateDto
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Rank { get; set; }
    public StatusType? Status { get; set; }
    public string? ActiveDuty { get; set; }
    public int? Level { get; set; }

    public void ApplyTo(Character c)
    {
        if (Name != null) c.Name = Name;
        if (Age.HasValue) c.Age = Age.Value;
        if (Rank != null) c.Rank = Rank;
        if (Status.HasValue) c.Status = Status.Value;
        if (ActiveDuty != null) c.ActiveDuty = ActiveDuty;
        if (Level.HasValue) c.Level = Level.Value;
    }
}
