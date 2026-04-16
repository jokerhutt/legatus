using Practice.Scripts.Economy;
using Practice.Scripts.Faction;
using Practice.Scripts.Map;
using Practice.Scripts.UI.Map;
using Practice.Scripts.UI.Map.TopBar;

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

    public override void _Ready()
    {
        _gs = GetNode<GameState>("/root/GameState");
        _gs.PlayerFactionId = "ROM";

        // services
        _factionService = new FactionService(_gs.FactionMap, _gs.ProvinceMap);
        _provinceService = new ProvinceService(_gs.ProvinceMap);
        _economyService = new EconomyService(_gs.FactionMap, _provinceService, _gs.BuildingMap);

        // systems
        var map = GetNode<MapController>("MapController");
        var menu = GetNode<ProvinceMenu>("CanvasLayer/ProvinceMenu");
        var topBar = GetNode<TopBar>("CanvasLayer/TopBar");

        // deps
        map.Init(_gs, _provinceService, _factionService);
        
        // UI
        menu.Init(_provinceService, _factionService, _gs.TerrainMap, _gs.SelectionState, _economyService, _gs.BuildingMap, _gs.PlayerFactionId);
        topBar.Init(_factionService, _gs.PlayerFactionId);


    }
}