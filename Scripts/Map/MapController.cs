using System;
using Godot;
using Legatus.Scripts.Buildings.Model;
using Legatus.Scripts.Diplomacy.Model;
using Legatus.Scripts.Diplomacy.Model.Enum;
using Legatus.Scripts.Faction;
using Legatus.Scripts.Province;
using Legatus.Scripts.State;
using Legatus.Scripts.Util;
using GDC = Godot.Collections;
using SCG = System.Collections.Generic;
namespace Legatus.Scripts.Map;

public partial class MapController : Node2D
{

    [Export] public Sprite2D MapImage;
    
    [Export(PropertyHint.File, "*.json")]
    public string ProvinceDataPath;

    [Export(PropertyHint.File, "*.txt")] 
    public string ProvinceMetadataPath;
    
    [Export(PropertyHint.File, "*.json")]
    public string TerrainDataPath;
    
    [Export(PropertyHint.File, "*.json")]
    public string FactionDataPath;
    
    [Export(PropertyHint.File, "*.json")]
    public string BuildingDataPath;
    
    [Export] public Node2D Provinces;

    private GameState GameState;

    public ProvinceService ProvinceService;
    public FactionService FactionService;

    public void Init(GameState gs, ProvinceService ps, FactionService fs)
    {
        GameState = gs;
        ProvinceService = ps;
        FactionService = fs;

        var scale = MapImage.Scale;
        InitializeProvinceMap(scale);
        InitializeBuildingMap(FSUtil.LoadJson(BuildingDataPath));
        InitializeTerrainMap(FSUtil.LoadJson(TerrainDataPath));
        InitializeTreaties(FSUtil.LoadJson(FactionDataPath));

        fs.InitializeFactions(FSUtil.LoadJson(FactionDataPath));
    }
    

    private void InitializeTerrainMap(GDC.Dictionary data)
    {
            var terrainMap = GameState.TerrainMap;
            var terrainList = (GDC.Array)data["terrains"];
            foreach (GDC.Dictionary t in terrainList)
            {
                var terrain = new Terrain
                {
                    Id = (JsonUtil.GetInt(t, "id") ?? 0).ToString(),
                    Name = JsonUtil.GetString(t, "name") ?? "Unknown",
                    IconPath = t["icon"].ToString()
                };
                terrainMap.Add(terrain.Id, terrain);
            }
    }
    
    private void InitializeTreaties(GDC.Dictionary data)
    {
        var treaties = GameState.Treaties;
        var treatyList = (GDC.Array)data["treaties"];
        foreach (GDC.Dictionary t in treatyList)
        {
            var treaty = new Treaty
            {
                A = t["a"].ToString(),
                B = t["b"].ToString(),
                Type = Enum.Parse<TreatyType>(t["type"].ToString()),
                Duration = (int)t["duration"]
            };
            treaties.Add(treaty);
        }
    }

    private void InitializeBuildingMap(GDC.Dictionary data)
    {
        var buildingMap = GameState.BuildingMap;
        var buildingList = (GDC.Array<GDC.Dictionary>)data["buildings"];
        foreach (GDC.Dictionary b in buildingList)
        {
            var building = new Building()
            {
                Id = b["id"].ToString(),
                Name = b["name"].ToString(),
                Description = b.ContainsKey("description") ? b["description"].ToString() : "",
                Levels = new SCG.List<BuildingLevel>()
            };

            var levels = (Godot.Collections.Array)b["levels"];

            foreach (Godot.Collections.Dictionary l in levels)
            {
                var level = new BuildingLevel()
                {
                    Level = (int)l["level"],
                    Cost = (int)l["cost"],
                    Maintenance = (int)l["maintenance"],
                    FoodYield = (int)l["foodYield"],
                    FoodMultiplier = (float)l["foodMultiplier"],
                    GoldYield = (int)l["goldYield"],
                    GoldMultiplier = (float)l["goldMultiplier"],
                    DefenseBonus = (int)l["defenseBonus"],
                    MapTexture = l.ContainsKey("mapTexture") ? GD.Load<Texture2D>((string)l["mapTexture"]) : null,
                    IconTexture = l.ContainsKey("iconTexture") ? GD.Load<Texture2D>((string)l["iconTexture"]) : null
                };

                building.Levels.Add(level);
            }

            buildingMap.Add(building.Id, building); 
        }
            
    }

    private void InitializeProvinceMap(Vector2 mapScale)
    {
        var provinceData = FSUtil.LoadJson(ProvinceDataPath);
        var provinceList = (GDC.Array)provinceData["provinces"];
        var provinceLookup = new GDC.Dictionary<string, GDC.Dictionary>();

        foreach (GDC.Dictionary province in provinceList)
        {
            string name = province["name"].ToString().Trim().ToLower();
            provinceLookup[name] = province;
        }

        Image image = MapImage.Texture.GetImage();
        var pixelColorDict = ImageUtil.GetPixelColorDict(image);
        var provincesMetadataDict = FSUtil.LoadJson(ProvinceMetadataPath);

        foreach (string provinceColor in provincesMetadataDict.Keys)
        {
            var provinceArea = (ProvinceArea)GD.Load<PackedScene>("res://Scenes/ProvinceArea.tscn").Instantiate();
            string provinceName = provincesMetadataDict[provinceColor].ToString().Trim().ToLower();
            
             provinceArea.ProvinceId = provinceName;
             provinceArea.Name = provinceName;

            Color color = new Color("#aaaaaa");
             provinceArea.BaseColor = color;
             
             if (provinceLookup.ContainsKey(provinceName))
             {
                 var data = provinceLookup[provinceName];
                 ProvinceService.InitializeProvinces(provinceName, provinceColor, data);
             }
            
            Provinces.AddChild( provinceArea);

            var polygons = PolygonUtil.GetPolygons(image, provinceColor, pixelColorDict);
             provinceArea.BuildGeometry(polygons, color);
             GD.Print($"{provinceName} polygons: {polygons.Length}");
        }

        Provinces.Scale = mapScale;
        MapImage.QueueFree();

    }

}