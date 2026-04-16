using Godot;
using Practice.Scripts.Economy;
using Practice.Scripts.Faction;
using Practice.Scripts.Province;
using Practice.Scripts.State;
using Practice.Scripts.World.Events;

namespace Practice.Scripts.World.Service;

public class TurnService
{
    private WorldEvents WorldEvents;
    private TurnState TurnState;
    private string PlayerFactionId;
    
    private EconomyService EconomyService;
    private ProvinceService ProvinceService;
    private FactionService FactionService;
    
    public TurnService(WorldEvents worldEvents, TurnState turnState, EconomyService economyService, ProvinceService provinceService, FactionService factionService, string playerFactionId)
    {
        TurnState = turnState;
        WorldEvents = worldEvents;
        WorldEvents.TurnEnd += OnTurnEnd;
        EconomyService = economyService;
        ProvinceService = provinceService;
        FactionService = factionService;
        PlayerFactionId = playerFactionId;
    }
    
    
    
    public void OnTurnEnd()
    {
        var factions = FactionService.GetFactions();
        foreach (var faction in factions)
        {
            ProcessFactionTurn(faction.Id);
        }
    }
    
    public void ProcessFactionTurn(string factionId)
    {
        GD.Print("TurnService received TurnEnd event");
        
        var faction = FactionService.GetFaction(factionId);
        var provinces = FactionService.GetProvinces(factionId);
        
        foreach (var province in provinces)
        {
            
            // FOOD & POPULATION & LOYALTY
            ProvinceService.ProcessProvinceTurn(province.Id);
            
            // ECONOMY
            var netIncome = ProvinceService.GetProvinceIncome(province.Id, faction.TaxRate);
            EconomyService.ApplyIncome(factionId, netIncome);

        }

        GD.Print($"Turn {TurnState.TurnNumber} complete");
        TurnState.TurnNumber += 1;
        GD.Print("================================");
        GD.Print($"New Turn: {TurnState.TurnNumber}");
        GD.Print("================================");
        WorldEvents.EmitTurnComplete();
    }
    
}