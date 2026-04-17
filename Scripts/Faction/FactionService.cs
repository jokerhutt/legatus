using System.Collections.Generic;
using System.Linq;
using Practice.Scripts.Faction.Enum;
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
    
    public IEnumerable<Model.Faction> GetFactions()
    {
        return _factionMap.GetAll();
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
                Color = new Godot.Color(f["color"].ToString()),
                TaxRate = TaxRate.Medium
            };

            _factionMap.Add(faction);
        }
        
        InitializeOpinions(factionList);
    }
    
    private void InitializeOpinions(GDC.Array factionList)
    {
        var allFactions = _factionMap.GetAll().ToList();

        foreach (GDC.Dictionary f in factionList)
        {
            var factionId = f["id"].ToString();
            var faction = _factionMap.Get(factionId);

            GDC.Dictionary jsonOpinions = null;
            if (f.ContainsKey("opinions"))
                jsonOpinions = (GDC.Dictionary)f["opinions"];

            foreach (var other in allFactions)
            {
                if (other.Id == factionId)
                    continue;

                int value = 10;

                if (jsonOpinions != null && jsonOpinions.ContainsKey(other.Id))
                {
                    value = (int)jsonOpinions[other.Id];
                }

                faction.Opinions[other.Id] = value;
            }
        }
    }
    
    
}