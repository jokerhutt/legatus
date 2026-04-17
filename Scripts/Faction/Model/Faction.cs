using System.Collections.Generic;
using Practice.Scripts.Faction.Enum;

namespace Practice.Scripts.Faction.Model;
using Godot;
    
public class Faction
{
    public string Id;
    public string Name;
    public Color Color;
    public int Coins;
    public TaxRate TaxRate;
    
    public Dictionary<string, int> Opinions = new();
    
}