using System.Collections.Generic;
using Practice.Scripts.Diplomacy.Model;
using Practice.Scripts.Diplomacy.Model.Enum;
using Practice.Scripts.Diplomacy.Views;
using Practice.Scripts.Faction;

namespace Practice.Scripts.Diplomacy;

public class DiplomacyService
{
    
    private FactionService _factionService;
    private List<Treaty> _treaties;
    
    public DiplomacyService(FactionService factionService, List<Treaty> treaties)
    {
        _factionService = factionService;
        _treaties = treaties;
    }
    
    public int GetOpinionOf(string ownFactionId, string otherFactionId)
    {
        var ownFaction = _factionService.GetFaction(ownFactionId);
        if (ownFaction == null) return 0;
        if (!ownFaction.Opinions.ContainsKey(otherFactionId)) return 0;
        return ownFaction.Opinions[otherFactionId];
    }
    
    public void CreateTreaty(string factionAId, string factionBId, TreatyType type)
    {
        var existingTreaty = _treaties.Find(t =>
            (t.A == factionAId && t.B == factionBId) ||
            (t.A == factionBId && t.B == factionAId));
        if (existingTreaty != null)
        {
            existingTreaty.Type = type;
        }
        else
        {
            _treaties.Add(new Treaty
            {
                A = factionAId,
                B = factionBId,
                Type = type
            });
        }
    }
    
    public void CancelTreaty(string factionAId, string factionBId, TreatyType type)
    {
        _treaties.RemoveAll(t =>
            ((t.A == factionAId && t.B == factionBId) ||
             (t.A == factionBId && t.B == factionAId)) &&
            t.Type == type);
    }
    
    public List<Treaty> GetTreatiesBetween(string factionAId, string factionBId)
    {
        return _treaties.FindAll(t =>
            (t.A == factionAId && t.B == factionBId) ||
            (t.A == factionBId && t.B == factionAId));
    }
    
    public List<Treaty> GetTreatiesOf(string factionId)
    {
        return _treaties.FindAll(t => t.A == factionId || t.B == factionId);
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
        
        var treaties = GetTreatiesBetween(ownFactionId, otherFactionId);
        
        return new DiplomacyViewDTO
        {
            Us = ownFactionDTO,
            Them = otherFactionDTO,
            Opinion = mutualOpinion,
            Relation = GetRelationType(treaties),
            Treaties = treaties
        };
    }
    
    public void MakePeace(string factionAId, string factionBId)
    {
        CancelTreaty(factionAId, factionBId, TreatyType.War);
    }
    
    public void DeclareWar(string factionAId, string factionBId)
    {
        CreateTreaty(factionAId, factionBId, TreatyType.War);
    }
    
    public void FormAlliance(string factionAId, string factionBId)
    {
        CreateTreaty(factionAId, factionBId, TreatyType.Alliance);
    }
    
    public void UpdateOpinion(string fromFactionId, string toFactionId, int amount)
    {
        var fromFaction = _factionService.GetFaction(fromFactionId);
        if (fromFaction == null) return;
        
        if (!fromFaction.Opinions.ContainsKey(toFactionId))
            fromFaction.Opinions[toFactionId] = 0;
        
        if (amount < -100)
            amount = -100;
        if (amount > 100)
            amount = 100;
        
        fromFaction.Opinions[toFactionId] += amount;
        
    }
    
    
    
    public RelationType GetRelationType(List<Treaty> treaties)
    {
        var allianceExists = treaties.Exists(t => t.Type == TreatyType.Alliance);
        var warExists = treaties.Exists(t => t.Type == TreatyType.War);
        
        if (allianceExists)
            return RelationType.Ally;
        if (warExists)
            return RelationType.Enemy;
        else
            return RelationType.Neutral;
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