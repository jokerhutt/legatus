using Godot;
using Practice.Scripts.Buildings.Dictionary;
using Practice.Scripts.Faction;
using Practice.Scripts.Map;
using Practice.Scripts.Province;
using Practice.Scripts.UI.Map.Army;
using Practice.Scripts.UI.Map.Slot;

namespace Practice.Scripts.UI.Map;

public partial class ProvinceMainMenu : MarginContainer
{
    private ProvinceService _provinceService;
    private FactionService _factionService;
    private TerrainMap _terrainMap;
    private BuildingMap _buildingMap;
    public System.Action OnOpenBuildingMenu;
    private string _provinceId;

    // == HEADER ==
    private Label ProvinceName;
    private Label OwnerName;
    private Label TerrainName;
    private TextureRect TerrainIcon;
    private Button CloseButton;

    // == ARMY ==
    [Export] public VBoxContainer ArmyColumn;
    private PackedScene _armyCardScene = GD.Load<PackedScene>("res://Scenes/UI/CompactArmyCard.tscn");
    
    // == BUILDINGS ==
    [Export] public HBoxContainer BuildingSlots;
    private PackedScene _buildingSlotsScene = GD.Load<PackedScene>("res://Scenes/UI/ProvinceMenu/ProvinceBuildingSlot.tscn");

    public System.Action OnClose;

    public override void _Ready()
    {
        ProvinceName = GetNode<Label>("%ProvinceName");
        OwnerName = GetNode<Label>("%OwnerName");
        TerrainName = GetNode<Label>("%TerrainName");
        TerrainIcon = GetNode<TextureRect>("%TerrainIcon");

        CloseButton = GetNode<Button>("%CloseButton");
        CloseButton.Pressed += () => OnClose?.Invoke();
    }

    public void Init(
        ProvinceService provinceService,
        FactionService factionService,
        TerrainMap terrainMap)
    {
        _provinceService = provinceService;
        _factionService = factionService;
        _terrainMap = terrainMap;
    }

    public void SetProvince(string provinceId)
    {
        _provinceId = provinceId;
        Redraw();
    }

    private void Redraw()
    {
        if (_provinceId == null)
            return;

        var p = _provinceService.GetProvince(_provinceId);
        var f = _factionService.GetFaction(p.FactionId);
        var t = _terrainMap.Get(p.TerrainId);

        ProvinceName.Text = p.Id;
        OwnerName.Text = f.Name;
        TerrainName.Text = t.Name;
        TerrainIcon.Texture = GD.Load<Texture2D>(t.IconPath);

        UpdateArmies();
        UpdateBuildings();
    }

    private void UpdateArmies()
    {
        if (ArmyColumn == null)
            return;

        foreach (Node child in ArmyColumn.GetChildren())
        {
            if (child is CompactArmyCard)
                child.QueueFree();
        }

        var card = _armyCardScene.Instantiate<CompactArmyCard>();
        ArmyColumn.AddChild(card);
        card.SetArmy("Legio I", 10, 1000);
    }

    private void UpdateBuildings()
    {
        if (BuildingSlots == null)
            return;

        foreach (Node child in BuildingSlots.GetChildren())
        {
            if (child is ProvinceBuildingSlot)
                child.QueueFree();
        }
        
        var slot = _buildingSlotsScene.Instantiate<ProvinceBuildingSlot>();
        BuildingSlots.AddChild(slot);
        slot.ShowBuilding();
        
        var emptySlot = _buildingSlotsScene.Instantiate<ProvinceBuildingSlot>();
        BuildingSlots.AddChild(emptySlot);
        emptySlot.OnRequestBuild = () => OnOpenBuildingMenu?.Invoke();
        emptySlot.ShowEmpty();
        
    }
    
}