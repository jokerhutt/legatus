using System.Collections.Generic;
using Practice.Scripts.Diplomacy.Events;
using Practice.Scripts.Diplomacy.Model;
using Practice.Scripts.Diplomacy.Views;
using Practice.Scripts.Economy;
using Practice.Scripts.Economy.Events;
using Practice.Scripts.Faction;

namespace Practice.Scripts.Diplomacy.Controller;

public class DiplomacyController
{
    
    private DiplomacyService _diplomacyService;
    private EconomyService _economyService;
    private DiplomacyEvents _diplomacyEvents;
    
    public DiplomacyController(FactionService factionService, EconomyService economyService, DiplomacyEvents diplomacyEvents, List<Treaty> treaties)
    {
        _economyService = economyService;
        _diplomacyService = new DiplomacyService(factionService, treaties);
        _diplomacyEvents = diplomacyEvents;
    }
    
    public void SendGift(string fromFactionId, string toFactionId, int amount)
    {
        _economyService.TransactCoins(fromFactionId, toFactionId, amount);
        _diplomacyService.UpdateOpinion(fromFactionId, toFactionId, 25);
        _diplomacyEvents.EmitOpinionChanged(fromFactionId, toFactionId, _diplomacyService.GetOpinionOf(fromFactionId, toFactionId));
        
    }
    
    public DiplomacyViewDTO GetDiplomacyViewData(string ownFactionId, string otherFactionId)
    {
        return _diplomacyService.GetDiplomacyViewData(ownFactionId, otherFactionId);
    }
    
    
}