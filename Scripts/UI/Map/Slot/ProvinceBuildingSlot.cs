using Godot;

namespace Legatus.Scripts.UI.Map.Slot;

public partial class ProvinceBuildingSlot : PanelContainer
{
    [Export] public Node Container;
    
    public System.Action OnRequestBuild;
    public System.Action<string> OnBuyBuilding;
    public System.Action<string> OnSellBuilding;

    private PackedScene _emptyScene = GD.Load<PackedScene>("res://Scenes/UI/ProvinceMenu/EmptySlot.tscn");
    private PackedScene _buildingScene = GD.Load<PackedScene>("res://Scenes/UI/ProvinceMenu/BuildingSlot.tscn");

    private Node _current;

    public override void _Ready()
    {
        if (Container == null)
        {
            GD.PushError("Container not assigned in ProvinceBuildingSlot");
            return;
        }

        ShowEmpty();
    }

    public void ShowEmpty()
    {
        Clear();
        var ui = _emptyScene.Instantiate<EmptySlot>();
        Container.AddChild(ui);
        ui.OnAddClicked = () => OnRequestBuild?.Invoke();
        _current = ui;
    }

    public void ShowBuilding(string id, int level, Texture2D texture, bool isOwn, bool isMaxLevel, bool canAfford)
    {
        Clear();
        
        GD.Print("Show Building: " + id);

        var ui = _buildingScene.Instantiate<BuildingSlot>();
        Container.AddChild(ui);

        ui.SetData(id, level, texture, isOwn, isMaxLevel, canAfford);
        ui.OnUpgradeClicked = buildingId => OnBuyBuilding?.Invoke(buildingId);
        ui.OnSellClicked = buildingId => OnSellBuilding?.Invoke(buildingId);

        _current = ui;
    }

    private void Clear()
    {
        if (_current != null)
        {
            _current.QueueFree();
            _current = null;
        }
    }
}