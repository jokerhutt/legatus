using System.Collections.Generic;
using Practice.Scripts.Diplomacy.Model;
using Practice.Scripts.Diplomacy.Model.Enum;

namespace Practice.Scripts.Diplomacy.Views;

public class DiplomacyViewDTO
{
    public DiplomacyViewFactionDTO Us;
    public DiplomacyViewFactionDTO Them;
    public MutualOpinionDto Opinion;
    public RelationType Relation;
    public List<Treaty> Treaties;
}

public class DiplomacyViewFactionDTO
{
    public string FactionId;
    public string FactionName;
}
