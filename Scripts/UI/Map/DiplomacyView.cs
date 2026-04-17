using System.Collections.Generic;
using Godot;
using Practice.Scripts.Diplomacy;
using Practice.Scripts.Diplomacy.Controller;
using Practice.Scripts.Diplomacy.Events;
using Practice.Scripts.Diplomacy.Model;
using Practice.Scripts.Diplomacy.Model.Enum;
using Practice.Scripts.Economy.Events;
using Practice.Scripts.State;
using Practice.Scripts.UI.Map.Sidebar;

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

    private Label StatusLabel;
    private Label StatusIcon;

    [Export] public VBoxContainer TreatiesList;
    private PackedScene TreatyCardScene = GD.Load<PackedScene>("res://Scenes/UI/DiplomacyTreatyGroup.tscn");

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
        
        StatusLabel = GetNodeOrNull<Label>("%StatusLabel");
        StatusIcon = GetNodeOrNull<Label>("%StatusIcon");
        
        var EconomyEvents = GetNode<EconomyEvents>("/root/EconomyEvents");
        var DiplomacyEvents = GetNode<DiplomacyEvents>("/root/DiplomacyEvents");
        EconomyEvents.BalanceChanged += OnBalanceChanged;
        DiplomacyEvents.OpinionChanged += OnOpinionChanged;

        
        Hide();
    }

    public void Init(DiplomacyController diplomacyController, string playerFactionId)
    {
        _diplomacyController = diplomacyController;
        PlayerFactionId = playerFactionId;
    }
    
    public void OnBalanceChanged(int newBalance, string factionId)
    {
        if (factionId == PlayerFactionId || factionId == TargetFactionId)
        {
            Refresh();
        }
    }
    
    public void OnOpinionChanged(string factionA, string factionB, int newValue)
    {
        if ((factionA == PlayerFactionId || factionB == PlayerFactionId) && (factionA == TargetFactionId || factionB == TargetFactionId))
        {
            Refresh();
        }
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
            UpdateStatus(data.Relation);
            UpdateTreaties(data.Treaties);
        }
        
        
        
    }

    private void UpdateStatus(RelationType relationType)
    {
        switch (relationType)
        {
            case RelationType.Ally:
                StatusLabel.Text = "Ally";
                StatusIcon.Text = "🛡️";
                break;

            case RelationType.Enemy:
                StatusLabel.Text = "War";
                StatusIcon.Text = "⚔️";
                break;

            case RelationType.Neutral:
                StatusLabel.Text = "Peace";
                StatusIcon.Text = "🕊️";
                break;
        }
    }

    private void UpdateTreaties(List<Treaty> treaties)
    {

        foreach (Node child in TreatiesList.GetChildren())
        {
            if (child is DiplomacyTreatyGroup)
                child.QueueFree();
        }
        
        
        foreach (var treaty in treaties)
        {
            GD.Print("Adding treaty card for " + treaty.Type);
            var card = TreatyCardScene.Instantiate<DiplomacyTreatyGroup>();
            TreatiesList.AddChild(card);
            card.SetData(treaty.Type.ToString(), treaty.Duration);
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

