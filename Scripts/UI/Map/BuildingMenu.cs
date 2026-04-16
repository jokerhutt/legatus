using System.Linq;
using Godot;
using Practice.Scripts.Buildings.Dictionary;
using Practice.Scripts.Province;
using Practice.Scripts.UI.Map.Building;

namespace Practice.Scripts.UI.Map.Army;

public partial class BuildingMenu : PanelContainer
{
    
    private ProvinceService _provinceService;
    private BuildingMap _buildingMap;
    

    private Button ShopCloseButton;
    public System.Action<string> OnBuyBuilding;

    public System.Action OnClose;

    [Export] public VBoxContainer BuildingList;
    private PackedScene _buildingCardScene = GD.Load<PackedScene>("res://Scenes/UI/ShopBuildingCard.tscn");

    public override void _Ready()
    {
        ShopCloseButton = GetNode<Button>("%ShopCloseButton");
        ShopCloseButton.Pressed += () => OnClose?.Invoke();
    }

    public void Init(ProvinceService provinceService, BuildingMap buildingMap)
    {
        _provinceService = provinceService;
        _buildingMap = buildingMap;
    }


    public void UpdateBuildingList(string _provinceId)
    {
        
        if (BuildingList == null || _buildingMap == null || string.IsNullOrEmpty(_provinceId))
            return;

        foreach (Node child in BuildingList.GetChildren())
        {
            if (child is ShopBuildingCard)
                child.QueueFree();
        }

        var province = _provinceService.GetProvince(_provinceId);
        if (province == null)
            return;

        foreach (var building in _buildingMap.GetAll())
        {
            var existing = province.Buildings.FirstOrDefault(b => b.Id == building.Id);
            var currentLevel = existing?.Level ?? 0;

            if (building.IsMaxLevel(currentLevel))
                continue;

            int nextLevel = currentLevel + 1;
            int cost = building.GetCostForLevel(nextLevel);

            var card = _buildingCardScene.Instantiate<ShopBuildingCard>();
            BuildingList.AddChild(card);

            var displayName = currentLevel == 0
                ? building.Name
                : $"{building.Name} Lv.{nextLevel}";

            card.SetData(
                displayName,
                building.Description,
                cost,
                building.Id
            );

            card.OnBuy = buildingId => OnBuyBuilding?.Invoke(buildingId);
        }
    }

}