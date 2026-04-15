using Godot;
using Practice.Scripts.Province.Dictionary;
using Practice.Scripts.State.map;

namespace Practice.Scripts.State;

public partial class GameState : Node
{
     
     public ProvinceMap ProvinceMap = new ProvinceMap();
    
     public SelectionState SelectionState = new SelectionState();
     public MapModeState MapModeState = new MapModeState();


}