using Godot;

namespace Practice.Scripts.Diplomacy.Events;

public partial class DiplomacyEvents : Node
{
    [Signal]
    public delegate void OpinionChangedEventHandler(string sourceFactionId, string targetFactionId, int newOpinionValue);
    
    public void EmitOpinionChanged(string sourceFactionId, string targetFactionId, int newOpinionValue)
    {
        GD.Print($"Emitting OpinionChanged: {sourceFactionId} -> {targetFactionId} = {newOpinionValue}");
        EmitSignal(SignalName.OpinionChanged, sourceFactionId, targetFactionId, newOpinionValue);
    }
    
}