using Godot;
using Practice.Scripts.Faction.Map;
using Practice.Scripts.Map;
using Practice.Scripts.Province.Dictionary;
using Practice.Scripts.State.map;

namespace Practice.Scripts.State;

public partial class GameState : Node
{
     
     public ProvinceMap ProvinceMap = new ProvinceMap();
     public FactionMap FactionMap = new FactionMap();
     public TerrainMap TerrainMap = new TerrainMap();
    
     public SelectionState SelectionState = new SelectionState();
     public MapModeState MapModeState = new MapModeState();


}