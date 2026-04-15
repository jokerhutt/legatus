using Godot;
using Microsoft.VisualBasic;
using Practice.Scripts.Province.Dictionary;
using Practice.Scripts.Util;
using GDC = Godot.Collections;
namespace Practice.Scripts.Province;
using Entity;

public class ProvinceService
{
    private ProvinceMap _provinceMap;
    
    public ProvinceService(ProvinceMap provinceMap)
    {
        _provinceMap = provinceMap;
    }
    
    public Province GetProvince(string provinceId)
    {
        return _provinceMap.Get(provinceId);
    }
    
    

    public void InitializeProvinces(string regionName, string regionColor, GDC.Dictionary data)
    {
        var p = new Province(id: regionName, color: new Color(regionColor));
        p.FactionId = JsonUtil.GetString(data, "ownerId");
        p.TerrainId = JsonUtil.GetString(data, "terrainId") ?? "0";
        p.Population = JsonUtil.GetInt(data, "population") ?? 1;
        p.FoodSurplus = JsonUtil.GetInt(data, "food_surplus") ?? 0;
        p.SetHappiness(JsonUtil.GetInt(data, "happiness") ?? 50);
        
        var buildings = JsonUtil.GetArray(data, "buildings");
        if (buildings != null)
        {
            foreach(GDC.Dictionary b in buildings)
            {
                var buildingId = JsonUtil.GetString(b, "buildingId");
                var level = JsonUtil.GetInt(b, "level") ?? 1;
                p.Buildings.Add(new ProvinceBuilding(buildingId, level));
            }
        }
        
        _provinceMap.Add(p);
    }
    
}