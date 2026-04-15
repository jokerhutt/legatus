using Godot;
using Practice.Scripts.Faction;
using Practice.Scripts.Map;
using Practice.Scripts.Province;
using Practice.Scripts.State;
using Practice.Scripts.UI.Map.Army;

namespace Practice.Scripts.UI.Map;

public partial class ProvinceMenu : PanelContainer
{
    private string _currentProvinceId;

    private SelectionState _selectionState;
    private ProvinceService _provinceService;
    private FactionService _factionService;
    private TerrainMap _terrainMap;

    private ProvinceMainMenu _mainMenu;
    private BuildingMenu _buildingMenu;

    public override void _Ready()
    {
        _mainMenu = GetNode<ProvinceMainMenu>("MainMenu");
        _buildingMenu = GetNode<BuildingMenu>("BuildingMenu");
        _mainMenu.OnOpenBuildingMenu = SetModeBuilding;
    }

    public void Init(
        ProvinceService provinceService,
        FactionService factionService,
        TerrainMap terrainMap,
        SelectionState selectionState)
    {
        _provinceService = provinceService;
        _factionService = factionService;
        _terrainMap = terrainMap;

        _selectionState = selectionState;
        _selectionState.SelectionChanged += OnSelectionChanged;

        _mainMenu.Init(provinceService, factionService, terrainMap);
        _mainMenu.OnClose = Close;
        
        _buildingMenu.OnClose = SetModeMain;
        

    }

    private void OnSelectionChanged(int type, string id)
    {
        if ((SelectionType)type != SelectionType.Province || id == null)
        {
            Hide();
            return;
        }

        _currentProvinceId = id;

        _mainMenu.SetProvince(id);
        SetModeMain();

        Show();
    }
    
    private void Close()
    {
        _currentProvinceId = null;
        Hide();
    }

    private void SetModeMain()
    {
        _mainMenu.Show();
        _buildingMenu.Hide();
    }

    private void SetModeBuilding()
    {
        _mainMenu.Hide();
        _buildingMenu.Show();
        _buildingMenu.UpdateBuildingList();
    }
}