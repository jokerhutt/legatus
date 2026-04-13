using Godot;
using System.Collections.Generic;
namespace Practice.Scripts.Map;

public partial class ProvinceArea : Area2D
{
    [Export] public string ProvinceId;
    [Export] public Color BaseColor = new Color(1, 1, 1, 0.5f);
    [Export] public Color HoverColor = new Color(1, 1, 1, 0.8f);
    [Export] public Color SelectedColor = new Color(1, 1, 0.5f, 0.8f);

    public override void _Ready()
    {
        InputEvent += OnInputEvent;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left && mb.Pressed)
            GD.Print($"Clicked on province {ProvinceId}");
    }
    
}