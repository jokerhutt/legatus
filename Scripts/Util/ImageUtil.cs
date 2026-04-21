using Godot;

namespace Legatus.Scripts.Util;
using SCG = System.Collections.Generic;

public static class ImageUtil
{
    public static SCG.Dictionary<string, SCG.List<Vector2>> GetPixelColorDict(Image image)
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
}