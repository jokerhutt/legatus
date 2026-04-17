using Godot;
using Practice.Scripts.Diplomacy;
using Practice.Scripts.Diplomacy.Controller;
using Practice.Scripts.State;

namespace Practice.Scripts.UI.Map;

public partial class DiplomacyView : PanelContainer
{
    private SelectionState _selectionState;
    private DiplomacyController _diplomacyController;

    private string PlayerFactionId;
    
    private string TargetFactionId;

    private Label YourOpinionValue;
    private Label TheirOpinonValue;

    private Label TheirName;
    private Label YourName;

    public override void _Ready()
    {
        var gameState = GetNode<GameState>("/root/GameState");
        _selectionState = gameState.SelectionState;
        _selectionState.SelectionChanged += OnSelectionChanged;
        
        var closeBtn = GetNodeOrNull<Button>("%CloseButton");
        if (closeBtn != null)
            closeBtn.Pressed += () => _selectionState.ClearSelection();
        
        TheirName = GetNodeOrNull<Label>("%TheirName");
        YourName = GetNodeOrNull<Label>("%YourName");
        
        YourOpinionValue = GetNodeOrNull<Label>("%YourOpinionValue");
        TheirOpinonValue = GetNodeOrNull<Label>("%TheirOpinionValue");
        
        Hide();
    }

    public void Init(DiplomacyController diplomacyController, string playerFactionId)
    {
        _diplomacyController = diplomacyController;
        PlayerFactionId = playerFactionId;
    }

    private void Refresh()
    {
        var data = _diplomacyController.GetDiplomacyViewData(PlayerFactionId, TargetFactionId);
        if (data == null) Hide();
        else
        {
            if (TheirName != null) TheirName.Text = data.Them.FactionName;
            if (YourName != null) YourName.Text = data.Us.FactionName;
            if (YourOpinionValue != null) YourOpinionValue.Text = data.Opinion.FromAToB.ToString();
            if (TheirOpinonValue != null) TheirOpinonValue.Text = data.Opinion.FromBToA.ToString();
        }
        
        
        
    }

    private void OnSelectionChanged(int type, string id)
    {
        if ((SelectionType)type == SelectionType.Diplomacy)
        {
            TargetFactionId = id;
            Refresh();
            Show();
        }
        else
        {
            Hide();
        }
    }
}

