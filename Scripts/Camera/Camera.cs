using Godot;
using System;

public partial class Camera : Camera2D
{
    [Export] public float Speed = 500f;

    // * ZOOM PROPERTIES * //
    private float ZoomLevel = 2f;
    [Export] public float ZoomStep = 0.1f;
    [Export] public float MaxZoom = 6f;
    [Export] public float MinZoom = 1f;
    
    // * PAN PROPERTIES * //
    private bool isPanning = false;
    private Vector2 panDelta = Vector2.Zero;
    
    // * INIT MAP THING OFFSETS * //
    [Export] public Sprite2D MapImage;

    public override void _Ready()
    {
        var mapSize = MapImage.Texture.GetSize() * MapImage.Scale;

        LimitLeft = 0;
        LimitTop = 0;
        LimitRight = (int)mapSize.X;
        LimitBottom = (int)mapSize.Y;

        var viewportSize = GetViewportRect().Size;

        float halfW = (viewportSize.X * 0.5f) / ZoomLevel;
        float halfH = (viewportSize.Y * 0.5f) / ZoomLevel;

        Position = new Vector2(
            Mathf.Clamp(mapSize.X / 2f, halfW, mapSize.X - halfW),
            Mathf.Clamp(mapSize.Y / 2f, halfH, mapSize.Y - halfH)
        );
    }

    public override void _Input(InputEvent @event)
    {
        HandleZoom(@event);
        HandlePan(@event);
    }

    private void HandleZoom(InputEvent @event)
    {
        if (@event.IsActionPressed("zoom_in"))
        {
            ZoomLevel = Math.Max(MinZoom, ZoomLevel - ZoomStep);
            Zoom = new Vector2(ZoomLevel, ZoomLevel);
        }

        if (@event.IsActionPressed("zoom_out"))
        {
            ZoomLevel = Math.Min(MaxZoom, ZoomLevel + ZoomStep);
            Zoom = new Vector2(ZoomLevel, ZoomLevel);
        }
    }


    private void HandlePan(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == MouseButton.Middle)
                isPanning = mb.Pressed;
        }

        if (@event is InputEventMouseMotion motion && isPanning)
        {
            panDelta += motion.Relative;
        }
        
    }

    public override void _Process(double delta)
    {
        Vector2 direction = Vector2.Zero;

        if (Input.IsActionPressed("ui_right")) direction.X += 1;
        if (Input.IsActionPressed("ui_left"))  direction.X -= 1;
        if (Input.IsActionPressed("ui_down"))  direction.Y += 1;
        if (Input.IsActionPressed("ui_up"))    direction.Y -= 1;

        direction = direction.Normalized();

        var newPos = Position;

        // WASD
        newPos += direction * Speed * (float)delta;

        // PAN
        newPos -= panDelta * (1f / ZoomLevel);
        panDelta = Vector2.Zero;

        // CLAMP
        var viewportSize = GetViewportRect().Size;
        float halfW = (viewportSize.X * 0.5f) / ZoomLevel;
        float halfH = (viewportSize.Y * 0.5f) / ZoomLevel;

        float minX = LimitLeft + halfW;
        float maxX = LimitRight - halfW;
        float minY = LimitTop + halfH;
        float maxY = LimitBottom - halfH;

        newPos.X = Mathf.Clamp(newPos.X, minX, maxX);
        newPos.Y = Mathf.Clamp(newPos.Y, minY, maxY);

        // APPLY POS
        Position = newPos;
        
    }
}