using Godot;
using Practice.Scripts.Economy.Events;
using Practice.Scripts.Faction;
using Practice.Scripts.State;

namespace Practice.Scripts.UI.Map.TopBar;

public partial class TopBar : PanelContainer
{
    
    private TurnState _turnState;

    [Export] public Texture2D CoinsTexture;
    [Export] public Texture2D ProvincesTexture;
    [Export] public Texture2D YearTexture;

    private HBoxContainer CoinsGroup;
    private HBoxContainer ProvincesGroup;
    private HBoxContainer YearGroup;
    
    private TextureButton SettingsButton;
    private TextureButton InfoButton;
    
    private FactionService _factionService;
    private string PlayerFactionId;

    private EconomyEvents EconomyEvents;
    
    public override void _Ready()
    {
        CoinsGroup = GetNode<HBoxContainer>("%CoinsGroup");
        ProvincesGroup = GetNode<HBoxContainer>("%ProvincesGroup");
        YearGroup = GetNode<HBoxContainer>("%YearGroup");
        
        SettingsButton = GetNode<TextureButton>("%SettingsButton");
        InfoButton = GetNode<TextureButton>("%InfoButton");
        
        InfoButton.Pressed += OnInfoButtonPressed;
        SettingsButton.Pressed += OnSettingsButtonPressed;
        
        EconomyEvents = GetNode<EconomyEvents>("/root/EconomyEvents");
        
        EconomyEvents.BalanceChanged += OnBalanceChanged;
    }
    
    private void OnBalanceChanged(int newBalance, string factionId)
    {
        GD.Print($"TopBar received BalanceChanged: {newBalance} for faction {factionId}");
        Refresh();
    }
    
    public void Init(FactionService factionService, string playerFactionId)
    {
        _factionService = factionService;
        PlayerFactionId = playerFactionId;
        
        CoinsGroup.GetNode<TextureRect>("%Icon").Texture = CoinsTexture;
        ProvincesGroup.GetNode<TextureRect>("%Icon").Texture = ProvincesTexture;
        YearGroup.GetNode<TextureRect>("%Icon").Texture = YearTexture;
        
        Refresh();
    }
    
    public void Refresh()
    {
        var faction = _factionService.GetFaction(PlayerFactionId);
        CoinsGroup.GetNode<Label>("%Label").Text = faction.Coins.ToString();
        ProvincesGroup.GetNode<Label>("%Label").Text = _factionService.GetProvinceCount(PlayerFactionId).ToString();
    }
    
    public void OnSettingsButtonPressed()
    {
        GD.Print("Settings button pressed");
    }
    
    public void OnInfoButtonPressed()
    {
        GD.Print("Info button pressed");
    }
    
    


}