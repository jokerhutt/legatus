using System.Linq;
using Godot;
using Practice.Scripts.Buildings.Dictionary;
using Practice.Scripts.Faction.Map;
using Practice.Scripts.Province;

namespace Practice.Scripts.Economy;

public class EconomyService
{
    private readonly FactionMap _factionMap;
    private readonly ProvinceService _provinceService;
    private readonly BuildingMap _buildingMap;

    public EconomyService(FactionMap factionMap, ProvinceService provinceService, BuildingMap buildingMap)
    {
        _factionMap = factionMap;
        _provinceService = provinceService;
        _buildingMap = buildingMap;
    }
    
    public bool CanAffordBuilding(string factionId, string buildingId, int level)
    {
        var faction = _factionMap.Get(factionId);
        var building = _buildingMap.Get(buildingId);

        GD.Print($"CHECKING AFFORDABILITY OF BUILIDNG {buildingId} for faction {factionId} at level {level}");
        GD.Print("Checking if faction can afford building:");
        if (faction == null || building == null)
            return false;
        

        var cost = building.GetCostForLevel(level);
        GD.Print($"Faction coins: {faction.Coins}, Building cost for level {level}: {cost}");
        return faction.Coins >= cost;
    }
    
    public bool CanAffordProvinceUpgrade(string factionId, string provinceId)
    {
        var faction = _factionMap.Get(factionId);
        var province = _provinceService.GetProvince(provinceId);
        if (faction == null || province == null)
            return false;

        var foodCost = province.GetFoodCostForNextLevel();
        var coinCost = province.GetCoinCostForNextLevel();

        var coins = faction.Coins;
        
        return ((coins >= coinCost) && (province.FoodSurplus >= foodCost));
    }
    
    public void UpgradeProvinceLevel(string factionId, string provinceId)
    {
        var faction = _factionMap.Get(factionId);
        var province = _provinceService.GetProvince(provinceId);
        if (faction == null || province == null)
            return;

        if (!CanAffordProvinceUpgrade(factionId, provinceId))
            return;
        
        var foodCost = province.GetFoodCostForNextLevel();
        var coinCost = province.GetCoinCostForNextLevel();
        
        faction.Coins -= coinCost;
        province.FoodSurplus -= foodCost;
        
        _provinceService.UpgradeProvinceLevel(provinceId);
    }
    
    public void ApplyIncome(string factionId, int amount)
    {
        var faction = _factionMap.Get(factionId);
        if (faction == null)
            return;
        faction.Coins += amount;
    }
    
    public bool IsBuildingMaxLevel(string buildingId, int currentLevel)
    {
        var building = _buildingMap.Get(buildingId);
        if (building == null)
            return true;

        return building.IsMaxLevel(currentLevel);
    }
    
    public bool CanBuyBuilding(string factionId, string provinceId, string buildingId)
    {
        var faction = _factionMap.Get(factionId);
        var province = _provinceService.GetProvince(provinceId);
        var building = _buildingMap.Get(buildingId);

        GD.Print($"Checking if faction {factionId} can buy building {buildingId} in province {provinceId}");
        if (faction == null || province == null || building == null)
            return false;
        
        GD.Print($"Faction coins: {faction.Coins}");

        var existing = province.Buildings.FirstOrDefault(b => b.Id == buildingId);
        var currentLevel = existing?.Level ?? 0;
        
        GD.Print($"Current level of building {buildingId} in province {provinceId}: {currentLevel}");

        if (building.IsMaxLevel(currentLevel))
        {
            GD.Print($"Building {buildingId} is already at max level in province {provinceId}");
            return false;
        }

            
        var nextLevel = currentLevel + 1;
        var buildingCost = building.GetCostForLevel(nextLevel);

        return buildingCost <= faction.Coins;
    }
    
    public bool SellBuilding(string factionId, string provinceId, string buildingId)
    {
        var faction = _factionMap.Get(factionId);
        var province = _provinceService.GetProvince(provinceId);
        var building = _buildingMap.Get(buildingId);

        if (faction == null || province == null || building == null)
            return false;

        var existing = province.Buildings.FirstOrDefault(b => b.Id == buildingId);
        if (existing == null)
            return false;

        var currentLevel = existing.Level;
        var sellPrice = building.GetCostForLevel(currentLevel) / 10;

        faction.Coins += sellPrice;
        _provinceService.RemoveBuilding(buildingId, provinceId);

        return true;
    }

    public bool BuyBuilding(string factionId, string provinceId, string buildingId)
    {
        if (!CanBuyBuilding(factionId, provinceId, buildingId))
            return false;

        var faction = _factionMap.Get(factionId);
        var province = _provinceService.GetProvince(provinceId);
        var building = _buildingMap.Get(buildingId);

        var existing = province.Buildings.FirstOrDefault(b => b.Id == buildingId);
        var currentLevel = existing?.Level ?? 0;
        var nextLevel = currentLevel + 1;
        var buildingCost = building.GetCostForLevel(nextLevel);

        faction.Coins -= buildingCost;
        
        if (existing == null)
            _provinceService.AddBuilding(buildingId, provinceId);
        else
            _provinceService.UpgradeBuilding(buildingId, provinceId);

        return true;
    }
}