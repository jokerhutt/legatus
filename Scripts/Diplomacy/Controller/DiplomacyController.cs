using Practice.Scripts.Diplomacy.Views;
using Practice.Scripts.Faction;

namespace Practice.Scripts.Diplomacy.Controller;

public class DiplomacyController
{
    
    private DiplomacyService _diplomacyService;
    
    public DiplomacyController(FactionService factionService)
    {
        _diplomacyService = new DiplomacyService(factionService);
    }
    
    public DiplomacyViewDTO GetDiplomacyViewData(string ownFactionId, string otherFactionId)
    {
        return _diplomacyService.GetDiplomacyViewData(ownFactionId, otherFactionId);
    }
    
    
}