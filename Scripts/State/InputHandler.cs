using Legatus.Scripts.State.map;

namespace Legatus.Scripts.State;
using Godot;

public partial class InputHandler : Node
{
    private MapModeState _mapModeState;
    private SelectionState _selectionState;

    public override void _Ready()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        _mapModeState = gameState.MapModeState;
        _selectionState = gameState.SelectionState;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_map_mode"))
            _mapModeState.ToggleMapMode();
        if (@event.IsActionPressed("escape_selection"))
            _selectionState.ClearSelection();
    }
}