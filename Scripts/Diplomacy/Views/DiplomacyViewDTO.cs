namespace Practice.Scripts.Diplomacy.Views;

public class DiplomacyViewDTO
{
    public DiplomacyViewFactionDTO Us;
    public DiplomacyViewFactionDTO Them;
    public MutualOpinionDto Opinion;
}

public class DiplomacyViewFactionDTO
{
    public string FactionId;
    public string FactionName;
}