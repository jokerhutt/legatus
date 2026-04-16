using System.Collections.Generic;
using System.Linq;
using Practice.Scripts.Faction.Map;
using Practice.Scripts.Province.Dictionary;
using GDC = Godot.Collections;
namespace Practice.Scripts.Faction;

public class FactionService
{
    
    private FactionMap _factionMap;
    private readonly ProvinceMap _provinceMap;
    
    public FactionService(FactionMap factionMap, ProvinceMap provinceMap)
    {
        _factionMap = factionMap;
        _provinceMap = provinceMap;
    }
    
    public Model.Faction GetFaction(string factionId)
    {
        return _factionMap.Get(factionId);
    }
    
    public List<Province.Entity.Province> GetProvinces(string factionId)
    {
        return _provinceMap.GetAll()
            .Where(p => p.FactionId == factionId)
            .ToList();
    }
    
    public int GetProvinceCount(string factionId)
    {
        return GetProvinces(factionId).Count;
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