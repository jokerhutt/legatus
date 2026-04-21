using System.Collections.Generic;
using Godot;
using Legatus.Scripts.Buildings.Dictionary;
using Legatus.Scripts.Diplomacy.Model;
using Legatus.Scripts.Faction.Map;
using Legatus.Scripts.Map;
using Legatus.Scripts.Province.Dictionary;
using Legatus.Scripts.State.map;

namespace Legatus.Scripts.State;

public partial class GameState : Node
{
     public ProvinceMap ProvinceMap = new ProvinceMap();
     public FactionMap FactionMap = new FactionMap();
     public TerrainMap TerrainMap = new TerrainMap();
     public BuildingMap BuildingMap = new BuildingMap();
     
     public List<Treaty> Treaties = new List<Treaty>();
     
     public TurnState TurnState = new TurnState();
     
     public SelectionState SelectionState = new SelectionState();
     public MapModeState MapModeState = new MapModeState();
     public string PlayerFactionId;


}