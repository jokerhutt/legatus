using Practice.Scripts.Faction;
using Practice.Scripts.Map;
using Practice.Scripts.UI.Map;

namespace Practice.Scripts;

using Godot;
using State;
using Province;

public partial class Bootstrap : Node
{
    private GameState _gs;

    private ProvinceService _provinceService;
    private FactionService _factionService;

    public override void _Ready()
    {
        _gs = GetNode<GameState>("/root/GameState");

        // services
        _factionService = new FactionService(_gs.FactionMap);
        _provinceService = new ProvinceService(_gs.ProvinceMap);

        // systems
        var map = GetNode<MapController>("MapController");
        var menu = GetNode<ProvinceMenu>("CanvasLayer/ProvinceMenu");

        // deps
        map.Init(_gs, _provinceService, _factionService);
        menu.Init(_provinceService, _factionService, _gs.TerrainMap, _gs.SelectionState);
    }
}