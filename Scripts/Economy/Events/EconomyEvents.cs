using Godot;

namespace Practice.Scripts.Economy.Events;

public partial class EconomyEvents : Node
{
    
    [Signal]
    public delegate void BalanceChangedEventHandler(int newValue, string factionId);
    
    public void EmitBalanceChanged(int newValue, string factionId)
    {
        GD.Print($"Emitting BalanceChanged: {newValue} for faction {factionId}");
        EmitSignal(SignalName.BalanceChanged, newValue, factionId);
    }
    
}