using System.Collections.Generic;

namespace Practice.Scripts.Buildings.Model;

public class Building
{
    public string Id;
    public string Name;
    public string Description;
    public List<BuildingLevel> Levels; 
    
    public int GetFoodYieldForLevel(int level)
    {
        return Levels[level - 1].FoodYield;
    }
    
    public int GetCoinYieldForLevel(int level)
    {
        return Levels[level - 1].GoldYield;
    }
    
    public int GetCostForLevel(int level)
    {
        return Levels[level - 1].Cost;
    }
    
    public int GetMaintenanceCostForLevel(int level)
    {
        return Levels[level - 1].Maintenance;
    }
    
    public bool IsMaxLevel(int level)
    {
        return level >= Levels.Count;
    }
    
}