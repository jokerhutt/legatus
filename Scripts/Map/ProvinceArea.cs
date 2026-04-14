using Godot;
using Practice.Scripts.State;
using Practice.Scripts.State.map;

namespace Practice.Scripts.Map;

public partial class ProvinceArea : Area2D
{
    [Export] public string ProvinceId;
    [Export] public Color BaseColor = new Color(1, 1, 1, 0.5f);
    [Export] public Color HoverColor = new Color(1, 1, 1, 0.8f);
    [Export] public Color SelectedColor = new Color(1, 1, 0.5f, 0.8f);

    private SelectionState SelectionState;
    private MapModeState MapModeState;
    
    private bool IsHovered = false;
    private bool IsSelected = false;

    public override void _Ready()
    {
        var _gameState = GetNode<GameState>("/root/GameState");
        SelectionState = _gameState.SelectionState;
        SelectionState.SelectionChanged += OnSelectionChanged;
        MapModeState = _gameState.MapModeState;
        MapModeState.MapModeChanged += OnMapModeChanged;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        InputEvent += OnInputEvent;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left && mb.Pressed)
        {
            GD.Print($"Clicked on province {ProvinceId}");
            SelectionState.SelectProvince(ProvinceId);
        }
    }
    
    private void OnMapModeChanged(int mode)
    {
        
        UpdateVisual();
    }
    
    private void OnSelectionChanged(int type, string id)
    {
        if ((SelectionType)type == SelectionType.Province && id == ProvinceId)
            IsSelected = true;
        else IsSelected = false;
        UpdateVisual();
    }
    
    private void OnMouseEntered()
    {
        GD.Print("ENTER");
        IsHovered = true;
        UpdateVisual();
    }

    private void OnMouseExited()
    {
        IsHovered = false;
        UpdateVisual();
    }

    private void SetProvinceColors(MapMode mapMode)
    {
        switch (mapMode)
        {
            case MapMode.Default:
                BaseColor = new Color(1, 1, 1, 0.5f);
                HoverColor = new Color(1, 1, 1, 0.8f);
                SelectedColor = new Color(1, 1, 0.5f, 0.8f);
                break;
            case MapMode.Faction:
                BaseColor = new Color(0.6f, 0.4f, 0.2f, 0.5f);
                HoverColor = new Color(0.6f, 0.4f, 0.2f, 0.8f);
                SelectedColor = new Color(0.8f, 0.6f, 0.4f, 0.8f);
                break;
            case MapMode.Province:
                BaseColor = new Color(0.4f, 0.6f, 1f, 0.5f);
                HoverColor = new Color(0.4f, 0.6f, 1f, 0.8f);
                SelectedColor = new Color(0.6f, 0.8f, 1f, 0.8f);
                break;
        }
    }
    
    private void UpdateVisual()
    {
        Color color;
        
        if (IsSelected) color = SelectedColor;
        else if (IsHovered) color = HoverColor;
        else color = BaseColor;

        foreach (Node node in GetChildren())
        {
            if (node is Polygon2D poly)
            {
                poly.Color = color;
            }
        }
    }
    
}