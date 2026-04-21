using System;
using Godot;
using GDC = Godot.Collections;
namespace Legatus.Scripts.Util;

public static class JsonUtil
{
    public static string GetString(Godot.Collections.Dictionary d, string key)
    {
        return d.ContainsKey(key) && d[key].VariantType != Variant.Type.Nil
            ? d[key].ToString()
            : null;
    }
    
    public static int RequireInt(GDC.Dictionary d, string key)
    {
        if (!d.ContainsKey(key) || d[key].VariantType == Variant.Type.Nil)
            throw new Exception($"Missing required key: {key}");

        return (int)d[key];
    }

    public static int? GetInt(Godot.Collections.Dictionary d, string key)
    {
        return d.ContainsKey(key) && d[key].VariantType != Variant.Type.Nil
            ? (int)d[key]
            : null;
    }

    public static Godot.Collections.Array GetArray(Godot.Collections.Dictionary d, string key)
    {
        return d.ContainsKey(key) && d[key].VariantType != Variant.Type.Nil
            ? (Godot.Collections.Array)d[key]
            : null;
    }
}