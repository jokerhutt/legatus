using Practice.Scripts.Diplomacy.Views;
using Practice.Scripts.Faction;

namespace Practice.Scripts.Diplomacy;

public class DiplomacyService
{
    
    private FactionService _factionService;
    
    public DiplomacyService(FactionService factionService)
    {
        _factionService = factionService;
    }
    
    public int GetOpinionOf(string ownFactionId, string otherFactionId)
    {
        var ownFaction = _factionService.GetFaction(ownFactionId);
        if (ownFaction == null) return 0;
        if (!ownFaction.Opinions.ContainsKey(otherFactionId)) return 0;
        return ownFaction.Opinions[otherFactionId];
    }
    
    public DiplomacyViewDTO GetDiplomacyViewData (string ownFactionId, string otherFactionId)
    {
        var ownFaction = _factionService.GetFaction(ownFactionId);
        var otherFaction = _factionService.GetFaction(otherFactionId);
        if (ownFaction == null || otherFaction == null) return null;
        var mutualOpinion = GetMutualOpinion(ownFactionId, otherFactionId);
        
        var ownFactionDTO = new DiplomacyViewFactionDTO
        {
            FactionId = ownFaction.Id,
            FactionName = ownFaction.Name,
        };
        var otherFactionDTO = new DiplomacyViewFactionDTO
        {
            FactionId = otherFaction.Id,
            FactionName = otherFaction.Name,
        };
        
        return new DiplomacyViewDTO
        {
            Us = ownFactionDTO,
            Them = otherFactionDTO,
            Opinion = mutualOpinion
        };
        
        

    }
    
    public MutualOpinionDto GetMutualOpinion(string a, string b)
    {
        return new MutualOpinionDto
        {
            FromAToB = GetOpinionOf(a, b),
            FromBToA = GetOpinionOf(b, a)
        };
    }
    
}

public class MutualOpinionDto
{
    public int FromAToB;
    public int FromBToA;
}