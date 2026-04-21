using Godot;

namespace Legatus.Scripts.Diplomacy.Events;

public partial class DiplomacyEvents : Node
{
    [Signal]
    public delegate void OpinionChangedEventHandler(string sourceFactionId, string targetFactionId, int newOpinionValue);
    
    [Signal]
    public delegate void TreatyChangedEventHandler(string factionA, string factionB);
    
    public void EmitOpinionChanged(string sourceFactionId, string targetFactionId, int newOpinionValue)
    {
        GD.Print($"Emitting OpinionChanged: {sourceFactionId} -> {targetFactionId} = {newOpinionValue}");
        EmitSignal(SignalName.OpinionChanged, sourceFactionId, targetFactionId, newOpinionValue);
    }
    
    public void EmitTreatyChanged(string factionA, string factionB)
    {
        GD.Print($"Emitting TreatyChanged: {factionA} <-> {factionB}");
        EmitSignal(SignalName.TreatyChanged, factionA, factionB);
    }
}