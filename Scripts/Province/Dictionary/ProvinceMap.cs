using SCG = System.Collections.Generic;
using Godot;
using Legatus.Scripts.Abstract;
using GDC = Godot.Collections;
namespace Legatus.Scripts.Province.Dictionary;

public class ProvinceMap : BaseMap<Entity.Province>
{
    public void Add(Entity.Province p)
    {
        Items[p.Id] = p;
    }
}