using System;
using Godot;
using Legatus.Scripts.Buildings.Dictionary;
using Legatus.Scripts.Faction.Enum;
using Legatus.Scripts.Map;
using Legatus.Scripts.Province.Dictionary;
using Legatus.Scripts.Util;
using Microsoft.VisualBasic;
using GDC = Godot.Collections;
namespace Legatus.Scripts.Province;
using Entity;

public class ProvinceService
{
    private ProvinceMap _provinceMap;
    public BuildingMap _buildingMap;
    public TerrainMap _terrainMap;
    
    public ProvinceService(ProvinceMap provinceMap)
    {
        _provinceMap = provinceMap;
    }

    public int GetFoodConsumption(string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return 0;
        return province.Population / 2;
    }
    
    /// <summary>
    /// Call once per turn per province. Applies food yield, consumption, surplus, and pop growth/starvation.
    /// </summary>
    public void ProcessProvinceTurn(string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return;

        var netFood = GetFoodYield(provinceId) - GetFoodConsumption(provinceId);
        province.FoodSurplus += netFood;
        
        var hasStarvation = province.FoodSurplus < 0;
        var hasSurplus = province.FoodSurplus > 0;

        if (hasStarvation)
        {
            province.Population = Math.Max(1, province.Population + province.FoodSurplus);
            province.FoodSurplus = 0;
        }
        else if (hasSurplus)
        {
            province.Population += province.FoodSurplus / 2;
        }
        
        ProcessLoyaltyChange(provinceId, hasStarvation);
        
        
    }
    
    
    public void ProcessLoyaltyChange(string provinceId, bool hasStarved) 
    {
        var province = GetProvince(provinceId);
        if (province == null) return;
        
        // Base Drift
        var target = 50;
        var drift = 0.05f;
        var delta = (int)((target - province.GetHappiness()) * drift);
        province.ChangeHappiness(delta);

        // Did you feed your people
        if (hasStarved)
            province.ChangeHappiness(-10);
        else
            province.ChangeHappiness(+2);
        
        
    }
    
    public int GetBuildingMaintenanceCost(string provinceId)
    {
        int cost = 0;
        var province = GetProvince(provinceId);
        if (province == null) return 0;
        var buildings = province.Buildings;
        foreach (var b in buildings)
        {
            var buildingData = _buildingMap.Get(b.Id);
            if (buildingData == null) continue;
            cost += buildingData.GetMaintenanceCostForLevel(b.Level);
        }
        return cost; 
    }
    
    public int GetProvinceIncome(string provinceId, TaxRate taxRate)
    {
        var populationYield = GetPopulationYield(provinceId, taxRate);
        var buildingYield = GetBuildingCoinYield(provinceId);
        var maintenanceCost = GetBuildingMaintenanceCost(provinceId);
        var net = populationYield + buildingYield - maintenanceCost;
        GD.Print($"{provinceId}:  POP YIELD: {populationYield}, BUILDING YIELD: {buildingYield}, MAINTENANCE COST: {maintenanceCost}, NET INCOME: {net}");
        return net;
    }

    public int GetBuildingCoinYield(string provinceId)
    {
        int yield = 0;
        var province = GetProvince(provinceId);
        if (province == null) return 0;
        var buildings = province.Buildings;
        foreach (var b in buildings)
        {
            var buildingData = _buildingMap.Get(b.Id);
            if (buildingData == null) continue;
            yield += buildingData.GetCoinYieldForLevel(b.Level);
        }
        return yield; 
    }
    
    public int GetPopulationYield(string provinceId, TaxRate taxRate)
    {
        var province = GetProvince(provinceId);
        if (province == null) return 0;

        return province.Population * (int)taxRate;
    }
    
    public int GetPopulationGrowth(string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return 0;
        if (province.FoodSurplus <= 0) return 0;
        return province.FoodSurplus / 2;
    }
    
    public void UpgradeProvinceLevel(string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return;
        if (!province.CanUpgradeLevel()) return;
        province.ProvinceLevel++;
    }
    

    public int GetFoodYield(string provinceId)
    {
        int yield = 0;
        
        var province = GetProvince(provinceId);
        if (province == null) return 0;
        
        // TERRAIN YIELD
        GD.Print($"Calculating food yield for province {provinceId} with terrain {province.TerrainId}");
        var terrain = _terrainMap.Get(province.TerrainId);
        if (terrain == null) return 0;
        yield += terrain.FoodYield;
        
        // BUILDING YIELD
        var buildings = province.Buildings;
        foreach (var b in buildings)
        {
            var buildingData = _buildingMap.Get(b.Id);
            if (buildingData == null) continue;
            yield += buildingData.GetFoodYieldForLevel(b.Level);
        }
        
        return yield;
        
    }

    public void AddBuilding(string BuildingId, string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return;
        if (!HasBuildingSlots(provinceId)) return;
        
        province.Buildings.Add(new ProvinceBuilding(BuildingId, 1));
    }

    public void RemoveBuilding(string buildingId, string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return;
        
        var building = province.Buildings.Find(b => b.Id == buildingId);
        if (building == null) return;
        
        province.Buildings.Remove(building);
    }
    
    public void UpgradeBuilding(string BuildingId, string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return;
        
        var building = province.Buildings.Find(b => b.Id == BuildingId);
        if (building == null) return;
        
        building.Level++;
    }

    public bool HasBuildingSlots(string provinceId)
    {
        var province = GetProvince(provinceId);
        if (province == null) return false;
        return province.Buildings.Count <= province.ProvinceLevel;
    }
    
    public Province GetProvince(string provinceId)
    {
        return _provinceMap.Get(provinceId);
    }
    
    

    public void InitializeProvinces(string regionName, string regionColor, GDC.Dictionary data)
    {
        var p = new Province(id: regionName, color: new Color(regionColor));
        p.FactionId = JsonUtil.GetString(data, "ownerId");
        p.TerrainId = (JsonUtil.GetInt(data, "terrainId") ?? 0).ToString();
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
        
        GD.Print($"Initialized province {p.Id} with faction {p.FactionId}, terrain {p.TerrainId}, population {p.Population}, food surplus {p.FoodSurplus}, happiness {p.GetHappiness()}, and {p.Buildings.Count} buildings");
        
        _provinceMap.Add(p);
    }
    
}