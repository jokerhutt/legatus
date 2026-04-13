using Godot;
using System;

public partial class Camera : Camera2D
{
    [Export] public float Speed = 500f;

    private float ZoomLevel = 2f;
    private const float ZoomStep = 0.1f;
    private const float MaxZoom = 6f;
    private const float MinZoom = 1f;
    
    
    public override void _Ready()
    {
        GetViewport().SizeChanged += OnResize;
    }
    
    private void OnResize()
    {
        var size = GetViewportRect().Size;
        float zoom = 1920f / size.X;
        Zoom = new Vector2(zoom, zoom);
    }

    public override void _Input(InputEvent @event)
    {
        OnZoom();
    }

    private void OnZoom()
    {
        if (Input.IsActionPressed("zoom_in"))
            ZoomLevel = Math.Max(MinZoom, ZoomLevel - ZoomStep);
        if (Input.IsActionPressed("zoom_out"))
            ZoomLevel = Math.Min(MaxZoom, ZoomLevel + ZoomStep);

        Zoom = new Vector2(ZoomLevel, ZoomLevel);
        
    }

    public override void _Process(double delta)
    {
        Vector2 direction = Vector2.Zero;

        if (Input.IsActionPressed("ui_right")) direction.X += 1;
        if (Input.IsActionPressed("ui_left"))  direction.X -= 1;
        if (Input.IsActionPressed("ui_down"))  direction.Y += 1;
        if (Input.IsActionPressed("ui_up"))    direction.Y -= 1;

        direction = direction.Normalized();

        Position += direction * Speed * (float)delta;
    }
}