using System;
using Godot;

namespace Legatus.Scripts.Util;
using GDC = Godot.Collections;

public static class FSUtil 
{
    public static GDC.Dictionary LoadJson(string path)
    {
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);

        if (file == null)
            throw new Exception($"Failed to open file: {path}");

        string text = file.GetAsText();

        var result = Json.ParseString(text);

        if (result.VariantType != Variant.Type.Dictionary)
            throw new Exception($"Invalid JSON format in: {path}");

        return (GDC.Dictionary)result;
    }
}