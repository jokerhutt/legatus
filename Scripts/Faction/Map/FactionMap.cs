using Practice.Scripts.Abstract;

namespace Practice.Scripts.Faction.Map;

public class FactionMap : BaseMap<Model.Faction>
{
    public void Add(Model.Faction f)
    {
        Items[f.Id] = f;
    }
}