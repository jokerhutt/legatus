using Godot;

namespace Legatus.Scripts.UI.Map.Slot;

public partial class BuildingSlot : VBoxContainer
{

    private string buildingId;
    private TextureButton UpgradeButton;
    private TextureButton DeleteButton;
    
    private TextureRect Icon;
    private Label Name;
    private Label Level;
    
    public System.Action<string> OnUpgradeClicked;
    public System.Action<string> OnSellClicked;
    
    
    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("%Icon");
        Name = GetNode<Label>("%Name");
        Level = GetNode<Label>("%Level");
        UpgradeButton = GetNode<TextureButton>("%UpgradeButton");
        DeleteButton = GetNode<TextureButton>("%DeleteButton");
        
        UpgradeButton.Pressed += () => OnUpgradeClicked?.Invoke(buildingId);
        DeleteButton.Pressed += () => OnSellClicked?.Invoke(buildingId);
    }
    
    public void SetData(string name, int level, Texture2D iconTexture, bool isOwn, bool isMaxLevel, bool canAfford)
    {
        buildingId = name;
        Name.Text = name;
        Level.Text = $"Level: {level}";
        Icon.Texture = iconTexture;
        
        UpgradeButton.Disabled = !canAfford;
        
        UpgradeButton.Visible = isOwn && !isMaxLevel;
        DeleteButton.Visible = isOwn;
    }


}