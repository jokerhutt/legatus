using System.Linq;
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

    public bool CanBuyBuilding(string factionId, string provinceId, string buildingId)
    {
        var faction = _factionMap.Get(factionId);
        var province = _provinceService.GetProvince(provinceId);
        var building = _buildingMap.Get(buildingId);

        if (faction == null || province == null || building == null)
            return false;

        var existing = province.Buildings.FirstOrDefault(b => b.Id == buildingId);
        var currentLevel = existing?.Level ?? 0;

        if (building.IsMaxLevel(currentLevel))
            return false;

        var nextLevel = currentLevel + 1;
        var buildingCost = building.GetCostForLevel(nextLevel);

        return buildingCost <= faction.Coins;
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
        _provinceService.AddBuilding(buildingId, provinceId);

        return true;
    }
}