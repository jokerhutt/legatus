using Practice.Scripts.Economy;
using Practice.Scripts.Faction;
using Practice.Scripts.Map;
using Practice.Scripts.UI.Map;
using Practice.Scripts.UI.Map.TopBar;
using Practice.Scripts.UI.Map.WorldActions;
using Practice.Scripts.World.Events;
using Practice.Scripts.World.Service;

namespace Practice.Scripts;

using Godot;
using State;
using Province;

public partial class Bootstrap : Node
{
    private GameState _gs;

    private ProvinceService _provinceService;
    private FactionService _factionService;
    private EconomyService _economyService;
    private TurnService _turnService;

    public override void _Ready()
    {
        _gs = GetNode<GameState>("/root/GameState");
        _gs.PlayerFactionId = "ROM";
        
        var worldEvents = GetNode<WorldEvents>("/root/WorldEvents");

        // services
        _factionService = new FactionService(_gs.FactionMap, _gs.ProvinceMap);
        _provinceService = new ProvinceService(_gs.ProvinceMap);
        _provinceService._buildingMap = _gs.BuildingMap;
        _provinceService._terrainMap = _gs.TerrainMap;
        _economyService = new EconomyService(_gs.FactionMap, _provinceService, _gs.BuildingMap);
        _turnService = new TurnService(worldEvents, _gs.TurnState, _economyService, _provinceService, _factionService, _gs.PlayerFactionId);

        // systems
        var map = GetNode<MapController>("MapController");
        var menu = GetNode<ProvinceMenu>("CanvasLayer/ProvinceMenu");
        var topBar = GetNode<TopBar>("CanvasLayer/TopBar");

        // deps
        map.Init(_gs, _provinceService, _factionService);
        
        // UI
        menu.Init(_provinceService, _factionService, _gs.TerrainMap, _gs.SelectionState, _economyService, _gs.BuildingMap, _gs.PlayerFactionId);
        topBar.Init(_factionService, _gs.PlayerFactionId, _gs.TurnState);


    }
}