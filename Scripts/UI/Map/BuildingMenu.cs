using Godot;
using Practice.Scripts.UI.Map.Building;

namespace Practice.Scripts.UI.Map.Army;

public partial class BuildingMenu : PanelContainer
{

    private Button ShopCloseButton;

    public System.Action OnClose;

    [Export] public VBoxContainer BuildingList;
    private PackedScene _buildingCardScene = GD.Load<PackedScene>("res://Scenes/UI/ShopBuildingCard.tscn");

    public override void _Ready()
    {
        ShopCloseButton = GetNode<Button>("%ShopCloseButton");
        ShopCloseButton.Pressed += () => OnClose?.Invoke();
    }

    public void UpdateBuildingList()
    {
        if (BuildingList == null)
            return;
        
        foreach (Node child in BuildingList.GetChildren())
        {
            if (child is ShopBuildingCard)
                child.QueueFree();
        }
        
        var card = _buildingCardScene.Instantiate<ShopBuildingCard>();
        BuildingList.AddChild(card);
        card.SetData("Blacksmith", "Unlocks new weapons and armor for your armies.", 100);

    }

}