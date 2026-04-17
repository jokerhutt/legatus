using System.Collections.Generic;
using Godot;
using Practice.Scripts.Province.Dictionary;
using Practice.Scripts.State;
using Practice.Scripts.State.map;

namespace Practice.Scripts.Map;

public partial class ProvinceArea : Area2D
{
    [Export] public string ProvinceId;
    [Export] public Color BaseColor = new Color(1, 1, 1, 0.5f);
    [Export] public Color HoverColor = new Color(1, 1, 1, 0.8f);
    [Export] public Color SelectedColor = new Color(1, 1, 0.5f, 0.8f);
    
    private ProvinceMap ProvinceMap;

    private SelectionState SelectionState;
    private MapModeState MapModeState;
    
    private bool IsHovered = false;
    private bool IsSelected = false;

    public override void _Ready()
    {
        var _gameState = GetNode<GameState>("/root/GameState");
        SelectionState = _gameState.SelectionState;
        ProvinceMap = _gameState.ProvinceMap;
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
            return;
        }

        if (@event is InputEventMouseButton mb2 && mb2.ButtonIndex == MouseButton.Right && mb2.Pressed)
        {
            GD.Print($"Right-clicked on province {ProvinceId}");
            var province = ProvinceMap.Get(ProvinceId);
            GD.Print($"Province {ProvinceId} belongs to faction {province.FactionId}");
            if (province.FactionId == null) return;
            GD.Print($"Selecting faction {province.FactionId} from province {ProvinceId}");
            SelectionState.SelectFaction(province.FactionId);
            return;
        }
    }

    private void OnMapModeChanged(int mode)
    {
        SetProvinceColors((MapMode)mode);
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
        var Province = ProvinceMap.Get(ProvinceId);
        
        switch (mapMode)
        {
            case MapMode.Default:
                BaseColor = new Color(1, 1, 1, 0.5f);
                HoverColor = new Color(1, 1, 1, 0.8f);
                SelectedColor = new Color(1, 1, 0.5f, 0.8f);
                break;
            case MapMode.Faction:
                BaseColor = new Color(0.4f, 0.6f, 0.2f, 0.5f);
                HoverColor = new Color(0.6f, 0.4f, 0.2f, 0.8f);
                SelectedColor = new Color(0.8f, 0.6f, 0.4f, 0.8f);
                break;
            case MapMode.Province:
                BaseColor = new Color(0.4f, 0.6f, 1f, 0.5f);
                HoverColor = new Color(0.4f, 0.6f, 1f, 0.8f);
                SelectedColor = new Color(0.6f, 0.8f, 1f, 0.8f);
                break;
            case MapMode.Food:
                SetFoodMapModeColors(Province);
                break;
        }
    }
    
    private void SetFoodMapModeColors(Province.Entity.Province p)
    {
        float food = p.FoodSurplus;

        float max = 10f;
        float t = Mathf.Clamp(food / max, 0f, 1f);

        Color color = new Color(
            1f - t,
            t,
            0f
        );

        BaseColor = new Color(color.R, color.G, color.B, 0.5f);
        HoverColor = new Color(color.R, color.G, color.B, 0.8f);
        SelectedColor = new Color(color.R, color.G, 0.3f, 0.9f);
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
    
    public void BuildGeometry(Vector2[][] polygons, Color color)
    {
        foreach (var poly in polygons)
        {
            var collision = new CollisionPolygon2D { Polygon = poly };

            var polygon = new Polygon2D
            {
                Polygon = poly,
                Color = color
            };

            var line = new Line2D
            {
                Width = 2,
                DefaultColor = Colors.Black
            };

            var closed = new List<Vector2>(poly);
            closed.Add(poly[0]);
            line.Points = closed.ToArray();

            AddChild(line);
            AddChild(collision);
            AddChild(polygon);
        }
    }
    
}