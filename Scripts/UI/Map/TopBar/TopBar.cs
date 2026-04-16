using Godot;
using Practice.Scripts.Faction;

namespace Practice.Scripts.UI.Map.TopBar;

public partial class TopBar : PanelContainer
{

    [Export] public Texture2D CoinsTexture;
    [Export] public Texture2D ProvincesTexture;

    private HBoxContainer CoinsGroup;
    private HBoxContainer ProvincesGroup;
    
    private TextureButton SettingsButton;
    private TextureButton InfoButton;
    
    private FactionService _factionService;
    private string PlayerFactionId;
    
    public override void _Ready()
    {
        CoinsGroup = GetNode<HBoxContainer>("%CoinsGroup");
        ProvincesGroup = GetNode<HBoxContainer>("%ProvincesGroup");
        
        SettingsButton = GetNode<TextureButton>("%SettingsButton");
        InfoButton = GetNode<TextureButton>("%InfoButton");
        
        InfoButton.Pressed += OnInfoButtonPressed;
        SettingsButton.Pressed += OnSettingsButtonPressed;
    }
    
    public void Init(FactionService factionService, string playerFactionId)
    {
        _factionService = factionService;
        PlayerFactionId = playerFactionId;
        
        CoinsGroup.GetNode<TextureRect>("%Icon").Texture = CoinsTexture;
        ProvincesGroup.GetNode<TextureRect>("%Icon").Texture = ProvincesTexture;
        
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