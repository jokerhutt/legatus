using System.Collections.Generic;
using Godot;
using Legatus.Scripts.Diplomacy.Controller;
using Legatus.Scripts.Diplomacy.Events;
using Legatus.Scripts.Diplomacy.Model;
using Legatus.Scripts.Diplomacy.Model.Enum;
using Legatus.Scripts.Economy.Events;
using Legatus.Scripts.State;
using Legatus.Scripts.UI.Map.Sidebar;
using Legatus.Scripts.Diplomacy;

namespace Legatus.Scripts.UI.Map;

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
    
    private TextureRect _yourCrest;
    private TextureRect _theirCrest;

    // Action buttons
    private Button _declareWarBtn;
    private Button _makePeaceBtn;
    private Button _nonAggressionBtn;
    private Button _allianceBtn;
    private Button _sendGiftBtn;

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
        
        _yourCrest = GetNodeOrNull<TextureRect>("%YourCrest");
        _theirCrest = GetNodeOrNull<TextureRect>("%TheirCrest");

        // Action buttons
        _declareWarBtn = GetNodeOrNull<Button>("%DeclareWarBtn");
        _makePeaceBtn = GetNodeOrNull<Button>("%MakePeaceBtn");
        _nonAggressionBtn = GetNodeOrNull<Button>("%NonAggressionBtn");
        _allianceBtn = GetNodeOrNull<Button>("%AllianceBtn");
        _sendGiftBtn = GetNodeOrNull<Button>("%SendGiftBtn");

        _declareWarBtn?.Connect("pressed", Callable.From(OnDeclareWar));
        _makePeaceBtn?.Connect("pressed", Callable.From(OnMakePeace));
        _nonAggressionBtn?.Connect("pressed", Callable.From(OnNonAggression));
        _allianceBtn?.Connect("pressed", Callable.From(OnAlliance));
        _sendGiftBtn?.Connect("pressed", Callable.From(OnSendGift));

        // Events
        var economyEvents = GetNode<EconomyEvents>("/root/EconomyEvents");
        var diplomacyEvents = GetNode<DiplomacyEvents>("/root/DiplomacyEvents");
        economyEvents.BalanceChanged += OnBalanceChanged;
        diplomacyEvents.OpinionChanged += OnOpinionChanged;
        diplomacyEvents.TreatyChanged += OnTreatyChanged;

        Hide();
    }

    public void Init(DiplomacyController diplomacyController, string playerFactionId)
    {
        _diplomacyController = diplomacyController;
        PlayerFactionId = playerFactionId;
    }

    // -- Event handlers --

    private void OnBalanceChanged(int newBalance, string factionId)
    {
        if (factionId == PlayerFactionId || factionId == TargetFactionId) Refresh();
    }

    private void OnOpinionChanged(string a, string b, int val)
    {
        if ((a == PlayerFactionId || b == PlayerFactionId) && (a == TargetFactionId || b == TargetFactionId)) Refresh();
    }

    private void OnTreatyChanged(string a, string b)
    {
        if ((a == PlayerFactionId || b == PlayerFactionId) && (a == TargetFactionId || b == TargetFactionId)) Refresh();
    }

    // -- Button callbacks --

    private void OnDeclareWar()
    {
        if (TargetFactionId == null) return;
        _diplomacyController.DeclareWar(PlayerFactionId, TargetFactionId);
    }

    private void OnMakePeace()
    {
        if (TargetFactionId == null) return;
        _diplomacyController.MakePeace(PlayerFactionId, TargetFactionId);
    }

    private void OnNonAggression()
    {
        if (TargetFactionId == null) return;
        if (_diplomacyController.HasNonAggression(PlayerFactionId, TargetFactionId))
            _diplomacyController.CancelNonAggression(PlayerFactionId, TargetFactionId);
        else
            _diplomacyController.SignNonAggression(PlayerFactionId, TargetFactionId);
    }

    private void OnAlliance()
    {
        if (TargetFactionId == null) return;
        if (_diplomacyController.HasAlliance(PlayerFactionId, TargetFactionId))
            _diplomacyController.CancelAlliance(PlayerFactionId, TargetFactionId);
        else
            _diplomacyController.FormAlliance(PlayerFactionId, TargetFactionId);
    }

    private void OnSendGift()
    {
        if (TargetFactionId == null) return;
        _diplomacyController.SendGift(PlayerFactionId, TargetFactionId);
    }

    // -- Refresh --

    private void Refresh()
    {
        if (TargetFactionId == null || _diplomacyController == null) return;
        var data = _diplomacyController.GetDiplomacyViewData(PlayerFactionId, TargetFactionId);
        if (data == null) { Hide(); return; }

        if (TheirName != null) TheirName.Text = data.Them.FactionName;
        if (YourName != null) YourName.Text = data.Us.FactionName;
        if (_yourCrest != null) _yourCrest.Texture = _diplomacyController.GetFactionCrest(PlayerFactionId);
        if (_theirCrest != null) _theirCrest.Texture = _diplomacyController.GetFactionCrest(TargetFactionId);
        if (YourOpinionValue != null) YourOpinionValue.Text = FormatOpinion(data.Opinion.FromAToB);
        if (TheirOpinonValue != null) TheirOpinonValue.Text = FormatOpinion(data.Opinion.FromBToA);
        UpdateStatus(data.Relation);
        UpdateTreaties(data.Treaties);
        UpdateButtons();
    }

    private string FormatOpinion(int val) => val >= 0 ? $"+{val}" : val.ToString();

    private void UpdateButtons()
    {
        bool atWar = _diplomacyController.IsAtWar(PlayerFactionId, TargetFactionId);
        bool hasNap = _diplomacyController.HasNonAggression(PlayerFactionId, TargetFactionId);
        bool hasAlliance = _diplomacyController.HasAlliance(PlayerFactionId, TargetFactionId);
        bool canDeclare = _diplomacyController.CanDeclareWar(PlayerFactionId, TargetFactionId);
        bool canGift = _diplomacyController.CanAffordGift(PlayerFactionId);

        if (_declareWarBtn != null) { _declareWarBtn.Visible = !atWar; _declareWarBtn.Disabled = !canDeclare; }
        if (_makePeaceBtn != null) { _makePeaceBtn.Visible = atWar; }

        if (_nonAggressionBtn != null)
        {
            _nonAggressionBtn.Text = hasNap ? "Cancel Non-Aggression" : "Sign Non-Aggression";
            _nonAggressionBtn.Disabled = atWar;
        }

        if (_allianceBtn != null)
        {
            _allianceBtn.Text = hasAlliance ? "Break Alliance" : "Form Alliance";
            _allianceBtn.Disabled = atWar;
        }

        if (_sendGiftBtn != null)
        {
            _sendGiftBtn.Text = $"Send Gift ({_diplomacyController.GetGiftCost()} coins)";
            _sendGiftBtn.Disabled = atWar || !canGift;
        }
    }

    private void UpdateStatus(RelationType relationType)
    {
        switch (relationType)
        {
            case RelationType.Ally:
                StatusLabel.Text = "Ally"; StatusIcon.Text = "🛡️"; break;
            case RelationType.Enemy:
                StatusLabel.Text = "War"; StatusIcon.Text = "⚔️"; break;
            case RelationType.Neutral:
                StatusLabel.Text = "Peace"; StatusIcon.Text = "🕊️"; break;
        }
    }

    private void UpdateTreaties(List<Treaty> treaties)
    {
        foreach (Node child in TreatiesList.GetChildren())
        {
            if (child is DiplomacyTreatyGroup) child.QueueFree();
        }
        foreach (var treaty in treaties)
        {
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


