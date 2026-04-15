using System.Collections.Generic;

namespace Practice.Scripts.Buildings.Model;

public class Building
{
    public string Id;
    public string Name;
    public string Description;
    public List<BuildingLevel> Levels; 
}