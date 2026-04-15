using Godot;
using Practice.Scripts.Faction;
using Practice.Scripts.Province;
using Practice.Scripts.State;
using Practice.Scripts.Util;
using GDC = Godot.Collections;
using SCG = System.Collections.Generic;
namespace Practice.Scripts.Map;

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
        InitializeTerrainMap(FSUtil.LoadJson(TerrainDataPath));
        
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
                    Id = JsonUtil.GetString(t, "id"),
                    Name = JsonUtil.GetString(t, "name") ?? "Unknown",
                    IconPath = t["icon"].ToString()
                };
                terrainMap.Add(terrain.Id, terrain);
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
             provinceArea.Name = provinceColor;

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
        }

        Provinces.Scale = mapScale;
        MapImage.QueueFree();

    }

}