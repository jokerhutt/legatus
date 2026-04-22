using System.Collections.Generic;
using Legatus.Scripts.Diplomacy.Model;
using Legatus.Scripts.Diplomacy.Model.Enum;
using Practice.Scripts.Diplomacy;

namespace Legatus.Scripts.Diplomacy.Views;

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
