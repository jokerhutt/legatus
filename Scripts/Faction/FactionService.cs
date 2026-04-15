using Practice.Scripts.Faction.Map;
using GDC = Godot.Collections;
namespace Practice.Scripts.Faction;

public class FactionService
{
    
    private FactionMap _factionMap;
    
    public FactionService(FactionMap factionMap)
    {
        _factionMap = factionMap;
    }
    
    public Model.Faction GetFaction(string factionId)
    {
        return _factionMap.Get(factionId);
    }

    public void InitializeFactions(GDC.Dictionary data)
    {
        var factionList = (GDC.Array)data["factions"];
        foreach (GDC.Dictionary f in factionList)
        {
            var faction = new Model.Faction
            {
                Id = f["id"].ToString(),
                Name = f["name"].ToString(),
                Coins = (int)f["coins"],
                Color = new Godot.Color(f["color"].ToString())
            };

            _factionMap.Add(faction);
        }
    }
    
    
}