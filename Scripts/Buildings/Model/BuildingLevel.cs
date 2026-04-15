using Godot;

namespace Practice.Scripts.Buildings.Model;

public class BuildingLevel
{
    public int Level;
    public int Cost;
    public int Maintenance;
    public int FoodYield;
    public Texture2D MapTexture;
    public Texture2D IconTexture;
    public float FoodMultiplier;
    public float GoldMultiplier;
    public int GoldYield;
    public int DefenseBonus;
}