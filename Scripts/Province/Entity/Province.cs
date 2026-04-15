using System;
using Godot;

namespace Practice.Scripts.Province.Entity;
using Faction.Model;
public class Province
{
    public string Id;
    public string Name;
    public Color Color;

    public int Population;
    public int FoodSurplus;
    private int Happiness;
    public int TaxLevel;
    
    public string FactionId;
    public string TerrainId;
    
    public Province(string id, Color color)
    {
        Id = id;
        Color = color;
        TaxLevel = 1;
    }
    
    public int GetHappiness()
    {
        return Happiness;
    }
    
    public void SetHappiness(int value)
    {
        Happiness = Math.Clamp(value, 0, 100);
    }
    public void ChangeHappiness(int amount)
    {
        // only between 0 and 100
        Happiness = Math.Clamp(Happiness + amount, 0, 100);
    }
    
    
}