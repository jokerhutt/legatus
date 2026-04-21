using System.Collections.Generic;
using Legatus.Scripts.Faction.Enum;

namespace Legatus.Scripts.Faction.Model;
using Godot;
    
public class Faction
{
    public string Id;
    public string Name;
    public Color Color;
    public Texture2D Crest;
    public int Coins;
    public TaxRate TaxRate;
    
    public Dictionary<string, int> Opinions = new();
    
}