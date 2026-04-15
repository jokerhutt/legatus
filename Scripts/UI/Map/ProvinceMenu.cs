using Godot;
using Practice.Scripts.Faction;
using Practice.Scripts.Map;
using Practice.Scripts.Province;
using Practice.Scripts.State;

namespace Practice.Scripts.UI.Map;

public partial class ProvinceMenu : PanelContainer
{

    private string CurrentProvinceId;
    
    private SelectionState _selectionState;
    private ProvinceService _provinceService;
    private FactionService _factionService;
    private TerrainMap _terrainMap;

    // == HEADER == //
    
    private Label ProvinceName;
    private Label OwnerName;
    private Label TerrainName;
    private TextureRect TerrainIcon;
    
    // == PROVINCE == //
    private Label PopulationCountLabel;
    private Label HappinessLevelLabel;
    private Label FoodSurplusLabel;
    private Label TaxLevelLabel;
    
    
    public void Init(ProvinceService provinceService, FactionService factionService, TerrainMap terrainMap, SelectionState selectionState)
    {
        _provinceService = provinceService;
        _factionService = factionService;
        _terrainMap = terrainMap;
        
        _selectionState = selectionState;
        _selectionState.SelectionChanged += OnSelectionChanged;
        
        ProvinceName = GetNode<Label>("%ProvinceName");
        OwnerName = GetNode<Label>("%OwnerName");
        TerrainName = GetNode<Label>("%TerrainName");
        TerrainIcon = GetNode<TextureRect>("%TerrainIcon");
        
        PopulationCountLabel = GetNode<Label>("%PopulationCountLabel");
        HappinessLevelLabel = GetNode<Label>("%HappinessLevelLabel");
        FoodSurplusLabel = GetNode<Label>("%FoodSurplusLabel");
        TaxLevelLabel = GetNode<Label>("%TaxLevelLabel");
        
    }
    
    private void OnSelectionChanged(int type, string id)
    {
        if ((SelectionType)type != SelectionType.Province || id == null)
        {
            CurrentProvinceId = null;
            Hide();
            return;
        }

        CurrentProvinceId = id;
        Redraw();
        Show();
    }

    public void Redraw()
    {
        if (CurrentProvinceId == null)
            return;

        var p = _provinceService.GetProvince(CurrentProvinceId);
        var f = _factionService.GetFaction(p.FactionId);
        var t = _terrainMap.Get(p.TerrainId);

        // Header
        ProvinceName.Text = p.Id;
        OwnerName.Text = f.Name;
        TerrainName.Text = t.Name;
        
        // Province Overview
        PopulationCountLabel.Text = $"Population: {p.Population}";
        HappinessLevelLabel.Text = $"Happiness: {p.GetHappiness()}";
        FoodSurplusLabel.Text = $"Food Surplus: {p.FoodSurplus}";
        TaxLevelLabel.Text = $"Tax Level: {p.TaxLevel}";

        TerrainIcon.Texture = GD.Load<Texture2D>(t.IconPath);
    }
    
    
    
    

}