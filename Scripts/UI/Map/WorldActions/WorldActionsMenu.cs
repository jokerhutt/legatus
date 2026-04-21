using Godot;
using Legatus.Scripts.World.Events;

namespace Legatus.Scripts.UI.Map.WorldActions;

public partial class WorldActionsMenu : PanelContainer
{

    public Button TurnButton;
    public bool Active = true;
    private WorldEvents WorldEvents;
    
    public override void _Ready()
    {
        WorldEvents = GetNode<WorldEvents>("/root/WorldEvents");
        TurnButton = GetNode<Button>("%TurnButton");
        TurnButton.Pressed += OnTurnButtonPressed;
        
        // Subscribe
        WorldEvents.TurnComplete += OnTurnComplete;
    }
    
    

    public void OnTurnButtonPressed()
    {
            if (!Active) return;
            Active = false;
            Refresh(Active);
            GD.Print("Turn button pressed, emitting TurnEnd event");
            WorldEvents.EmitTurnEnd();
    }

    private void OnTurnComplete()
    {
        Active = true;
        Refresh(Active);
    }

    public void Refresh(bool active)
    {
        // Button disablled = !active
    }


}