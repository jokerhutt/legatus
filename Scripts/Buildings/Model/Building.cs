using System.Collections.Generic;

namespace Practice.Scripts.Buildings.Model;

public class Building
{
    public string Id;
    public string Name;
    public string Description;
    public List<BuildingLevel> Levels; 
    
    public int GetCostForLevel(int level)
    {
        return Levels[level - 1].Cost;
    }
    
    public bool IsMaxLevel(int level)
    {
        return level >= Levels.Count;
    }
    
}