using Godot;
using Practice.Scripts.State;

namespace Practice.Scripts.UI.Map;

public partial class DiplomacyView : PanelContainer
{
    private SelectionState _selectionState;

    public override void _Ready()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        _selectionState = gameState.SelectionState;
        _selectionState.SelectionChanged += OnSelectionChanged;
        
        var closeBtn = GetNodeOrNull<Button>("%CloseButton");
        if (closeBtn != null)
            closeBtn.Pressed += () => _selectionState.ClearSelection();
        
        Hide();
    }

    private void OnSelectionChanged(int type, string id)
    {
        if ((SelectionType)type == SelectionType.Diplomacy)
        {
            // Update faction name if we have one
            var theirName = GetNodeOrNull<Label>("%TheirName");
            if (theirName != null && !string.IsNullOrEmpty(id))
                theirName.Text = id;
            
            Show();
        }
        else
        {
            Hide();
        }
    }
}

