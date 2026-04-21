using Godot;

namespace Legatus.Scripts.Util;
using SCG = System.Collections.Generic;

public static class PolygonUtil
{
    public static Vector2[][] GetPolygons(
        Image image,
        string regionColor,
        SCG.Dictionary<string, SCG.List<Vector2>> dict)
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
}