using Godot;
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
    
    [Export]
    public PackedScene ProvinceAreaScene;

    [Export] public Node2D Provinces;
    


    public override void _Ready()
    {
        var scale = MapImage.Scale;
        InitializeProvinceMap(scale);
    }

    private void InitializeProvinceMap(Vector2 mapScale)
    {
        var provinceData = ImportFile(ProvinceDataPath);
        var provinceList = (GDC.Array)provinceData["provinces"];
        var provinceLookup = new GDC.Dictionary<string, GDC.Dictionary>();

        foreach (GDC.Dictionary province in provinceList)
        {
            string name = province["name"].ToString().Trim().ToLower();
            provinceLookup[name] = province;
        }

        Image image = MapImage.Texture.GetImage();
        var pixelColorDict = GetPixelColorDict(image);
        var provincesMetadataDict = ImportFile(ProvinceMetadataPath);

        foreach (string provinceColor in provincesMetadataDict.Keys)
        {
            var province = (ProvinceArea)ProvinceAreaScene.Instantiate();
            string provinceName = provincesMetadataDict[provinceColor].ToString().Trim().ToLower();
            
            province.ProvinceId = provinceName;
            province.Name = provinceColor;

            Color color = new Color("#aaaaaa");
            province.BaseColor = color;
            
            Provinces.AddChild(province);

            var polygons = GetPolygons(image, provinceColor, pixelColorDict);
            
            foreach (var poly in polygons)
            {
                var collision = new CollisionPolygon2D();
                collision.Polygon = poly;

                var polygon = new Polygon2D();
                polygon.Polygon = poly;
                polygon.Color = color;
                
                var line = new Line2D();
                line.Width = 2;
                line.DefaultColor = Colors.Black;

                var closed = new SCG.List<Vector2>(poly);
                closed.Add(poly[0]);

                line.Points = closed.ToArray();

                province.AddChild(line);

                province.AddChild(collision);
                province.AddChild(polygon); 
            }
        }

        Provinces.Scale = mapScale;
        MapImage.QueueFree();

    }
    
    private Vector2[][] GetPolygons(Image image, string regionColor, SCG.Dictionary<string, SCG.List<Vector2>> dict)
    {
        string normalized = "#" + new Color(regionColor).ToHtml(false).ToLower();

        Image target = Image.CreateEmpty(image.GetWidth(), image.GetHeight(), false, Image.Format.Rgba8);

        if (dict.ContainsKey(normalized))
        {
            foreach (var v in dict[normalized])
                target.SetPixel((int)v.X, (int)v.Y, Colors.White);
        }
        else
        {
            for (int y = 0; y < image.GetHeight(); y++)
            {
                for (int x = 0; x < image.GetWidth(); x++)
                {
                    string hex = "#" + image.GetPixel(x, y).ToHtml(false).ToLower();

                    if (hex == normalized)
                        target.SetPixel(x, y, Colors.White);
                }
            }
        }

        var bitmap = new Bitmap();
        bitmap.CreateFromImageAlpha(target);

        var polys = bitmap.OpaqueToPolygons(new Rect2I(Vector2I.Zero, bitmap.GetSize()), 0.01f);

        var result = new Vector2[polys.Count][];
        for (int i = 0; i < polys.Count; i++)
            result[i] = (Vector2[])polys[i];

        return result;
    }
    
    private SCG.Dictionary<string, SCG.List<Vector2>> GetPixelColorDict(Image image)
    {
        var dict = new SCG.Dictionary<string, SCG.List<Vector2>>();

        for (int y = 0; y < image.GetHeight(); y++)
        {
            for (int x = 0; x < image.GetWidth(); x++)
            {
                string hex = "#" + image.GetPixel(x, y).ToHtml(false).ToLower();

                if (!dict.ContainsKey(hex))
                    dict[hex] = new SCG.List<Vector2>();

                dict[hex].Add(new Vector2(x, y));
            }
        }

        return dict;
    }

    private GDC.Dictionary ImportFile(string path)
    {
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);

        if (file != null)
        {
            string text = file.GetAsText().Replace("_", " ");
            return (GDC.Dictionary)Json.ParseString(text);
        }

        GD.PrintErr("Failed to open file: ", path);
        return null;
    }

}