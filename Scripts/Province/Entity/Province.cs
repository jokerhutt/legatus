using System;
using System.Collections.Generic;
using Godot;

namespace Legatus.Scripts.Province.Entity;
using Faction.Model;
public class Province
{
    public string Id;
    public string Name;
    public Color Color;

    public int Population;
    public int FoodSurplus;
    public int ProvinceLevel;
    private int Happiness;
    public int TaxLevel;

    public int MaxLevel = 6;

    public List<ProvinceBuilding> Buildings = new();
    
    public string FactionId;
    public string TerrainId;
    
    public Province(string id, Color color)
    {
        Id = id;
        Color = color;
        TaxLevel = 1;
        ProvinceLevel = 4;
    }
    
    public int GetHappiness()
    {
        return Happiness;
    }

    public int GetFoodCostForNextLevel()
    {
        if (!CanUpgradeLevel())
            return int.MaxValue;
        return ProvinceLevel * 50;
    }
    
    public int GetCoinCostForNextLevel()
    {
        if (!CanUpgradeLevel())
            return int.MaxValue;
        return ProvinceLevel * 100;
    }
    
    public bool CanUpgradeLevel()
    {
        return ProvinceLevel < MaxLevel;
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