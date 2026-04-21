using System;
using Godot;

namespace Legatus.Scripts.State.map;

public partial class MapModeState : Node
{
    
    
    public MapMode MapMode = MapMode.Default;
    
    [Signal]
    public delegate void MapModeChangedEventHandler(int type);
    
    public void ToggleMapMode()
    {
        GD.Print("Toggle Map Mode");
        int next = ((int)MapMode + 1) % Enum.GetValues(typeof(MapMode)).Length;
        GD.Print("Next Map Mode: " + (MapMode)next);
        SetMapMode((MapMode)next);
    }
    
    public void SetMapMode(MapMode mode)
    {
        MapMode = mode;
        EmitSignal(SignalName.MapModeChanged, (int)MapMode);
    }
    
}