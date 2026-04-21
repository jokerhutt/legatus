using Godot;
using Legatus.Scripts.State;
using Legatus.Scripts.Diplomacy;

namespace Legatus.Scripts.UI.Map;

public partial class SidePanel : VBoxContainer
{
    private SelectionState _selectionState;

    public override void _Ready()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        _selectionState = gameState.SelectionState;

        var diplomacyTab = GetNode<Button>("DiplomacyTab");
        diplomacyTab.Pressed += () => _selectionState.ToggleDiplomacyView();
    }
    
    
}
