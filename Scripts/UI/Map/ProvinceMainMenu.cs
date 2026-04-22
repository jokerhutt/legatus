using Godot;
using Legatus.Scripts.Buildings.Dictionary;
using Legatus.Scripts.Economy;
using Legatus.Scripts.Faction;
using Legatus.Scripts.Map;
using Legatus.Scripts.Province;
using Legatus.Scripts.UI.Map.Army;
using Legatus.Scripts.UI.Map.Slot;

namespace Legatus.Scripts.UI.Map;

public partial class ProvinceMainMenu : MarginContainer
{
    private ProvinceService _provinceService;
    private FactionService _factionService;
    private TerrainMap _terrainMap;
    private BuildingMap _buildingMap;
    private EconomyService _economyService;
    public System.Action OnOpenBuildingMenu;
    public System.Action<string> OnBuyBuilding;
    public System.Action<string> OnSellBuilding;
    
    private string _provinceId;
    private string PlayerFactionId;

    // == HEADER ==
    private Label ProvinceName;
    private Label OwnerName;
    private Label TerrainName;
    private TextureRect TerrainIcon;
    private Button CloseButton;
    private Button OwnerButton;

    // == ARMY ==
    [Export] public VBoxContainer ArmyColumn;
    private PackedScene _armyCardScene = GD.Load<PackedScene>("res://Scenes/UI/CompactArmyCard.tscn");
    
    // == BUILDINGS ==
    [Export] public HBoxContainer BuildingSlots;
    private PackedScene _buildingSlotsScene = GD.Load<PackedScene>("res://Scenes/UI/ProvinceMenu/ProvinceBuildingSlot.tscn");

    public System.Action OnClose;
    public System.Action<string> OnOwnerButtonPressed;

    public override void _Ready()
    {
        ProvinceName = GetNode<Label>("%ProvinceName");
        OwnerName = GetNode<Label>("%OwnerName");
        TerrainName = GetNode<Label>("%TerrainName");
        TerrainIcon = GetNode<TextureRect>("%TerrainIcon");
        OwnerButton = GetNode<Button>("%OwnerButton");

        CloseButton = GetNode<Button>("%CloseButton");
        CloseButton.Pressed += () => OnClose?.Invoke();
        OwnerButton.Pressed += () => OnSelectFaction();
    }
    
    private void OnSelectFaction()
    {
        if (_provinceId == null)
            return;

        var province = _provinceService.GetProvince(_provinceId);
        if (province == null)
            return;

        OnOwnerButtonPressed?.Invoke(province.FactionId);
    }

    public void Init(
        ProvinceService provinceService,
        FactionService factionService,
        TerrainMap terrainMap,
        BuildingMap buildingMap,
        string playerFactionId,
        EconomyService economyService)
        
    {
        _provinceService = provinceService;
        _factionService = factionService;
        _terrainMap = terrainMap;
        _buildingMap = buildingMap;
        PlayerFactionId = playerFactionId;
        _economyService = economyService;
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
        UpdateBuildings(p);
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

    private void UpdateBuildings(Province.Entity.Province province)
    {
        if (BuildingSlots == null)
            return;

        foreach (Node child in BuildingSlots.GetChildren())
        {
            if (child is ProvinceBuildingSlot)
                child.QueueFree();
        }
        
        GD.Print($"Updating buildings for province {province.Id}, owned by faction {province.FactionId}, and the player faction is {PlayerFactionId}");
        var isOwn = province.FactionId == PlayerFactionId;
        
        GD.Print($"Province has the following {province.Buildings.Count} buildings:");

        foreach (var pb in province.Buildings)
        {
            
            GD.Print($"Processing building {pb.Id} at level {pb.Level} in province {province.Id} with levels count of {_buildingMap.Get(pb.Id)?.Levels.Count ?? 0}");
            
            var isMaxLevel = _economyService.IsBuildingMaxLevel(pb.Id, pb.Level);
            var canAfford = !isMaxLevel && isOwn && _economyService.CanAffordBuilding(province.FactionId, pb.Id, pb.Level + 1);
            
            GD.Print($"CAN AFFORD: {canAfford}, IS MAX LEVEL: {isMaxLevel}, IS OWN: {isOwn}");
            
            var def = _buildingMap.Get(pb.Id);
            GD.Print("Got building definition for building id " + pb.Id + ": " + (def != null ? def.Name : "null"));
            if (def == null)
                continue;

            if (pb.Level <= 0 || pb.Level > def.Levels.Count)
                continue;

            var levelData = def.Levels[pb.Level - 1];

            var slot = _buildingSlotsScene.Instantiate<ProvinceBuildingSlot>();
            BuildingSlots.AddChild(slot);
            
            
            slot.ShowBuilding(def.Id, pb.Level, levelData.IconTexture, isOwn, isMaxLevel, canAfford);
            slot.OnBuyBuilding = buildingId => OnBuyBuilding?.Invoke(buildingId);
            slot.OnSellBuilding = buildingId => OnSellBuilding?.Invoke(buildingId);
        }

        int emptyCount = province.ProvinceLevel - province.Buildings.Count;

        if (isOwn)
        {
            for (int i = 0; i < emptyCount; i++)
            {
                var emptySlot = _buildingSlotsScene.Instantiate<ProvinceBuildingSlot>();
                BuildingSlots.AddChild(emptySlot);

                emptySlot.OnRequestBuild = () => OnOpenBuildingMenu?.Invoke();
                emptySlot.ShowEmpty();
            }
        }

    }
    
}