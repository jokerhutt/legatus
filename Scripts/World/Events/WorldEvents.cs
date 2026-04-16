using Godot;

namespace Practice.Scripts.World.Events;

public partial class WorldEvents : Node
{
    
    [Signal]
    public delegate void TurnEndEventHandler();
    
    [Signal]
    public delegate void TurnCompleteEventHandler();
    
    public void EmitTurnEnd()
    {
        GD.Print("Emitting TurnEnd event");
        EmitSignal(SignalName.TurnEnd);
    }
    
    public void EmitTurnComplete()
    {
        EmitSignal(SignalName.TurnComplete);
    }
    
}