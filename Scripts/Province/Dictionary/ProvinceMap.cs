using SCG = System.Collections.Generic;
using Godot;
using Practice.Scripts.Abstract;
using GDC = Godot.Collections;
namespace Practice.Scripts.Province.Dictionary;

public class ProvinceMap : BaseMap<Entity.Province>
{
    public void Add(Entity.Province p)
    {
        Items[p.Id] = p;
    }
}