using SCG = System.Collections.Generic;
using Godot;
using GDC = Godot.Collections;
namespace Practice.Scripts.Province.Dictionary;

public class ProvinceMap
{
    public SCG.Dictionary<string, Entity.Province> Provinces = new();

    public void Initialize(GDC.Array list)
    {
        foreach (GDC.Dictionary p in list)
        {
            var province = new Entity.Province
            {
                Id = p["id"].ToString(),
                Name = p["name"].ToString()
            };
            
            Provinces[province.Id] = province;
        }
    }
    
    public bool TryGet(string id, out Entity.Province province)
    {
        return Provinces.TryGetValue(id, out province);
    }
    
    public Entity.Province Get(string id)
    {
        return Provinces[id];
    }
    
    
}