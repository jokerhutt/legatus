using Godot;

namespace Practice.Scripts.State;

public partial class SelectionState : Node
{
    public Selection CurrentSelection = new();
    
    [Signal]
    public delegate void SelectionChangedEventHandler(int type, string id);
    
    public void SelectProvince(string provinceId)
    {
        CurrentSelection.Type = SelectionType.Province;
        CurrentSelection.Id = provinceId;
        EmitSignal(SignalName.SelectionChanged, (int)CurrentSelection.Type, CurrentSelection.Id);
    }
    
    public void SelectArmy(string armyId)
    {
        CurrentSelection.Type = SelectionType.Army;
        CurrentSelection.Id = armyId;
        EmitSignal(SignalName.SelectionChanged, (int)CurrentSelection.Type, CurrentSelection.Id);
    }
    
    public void ClearSelection()
    {
        CurrentSelection.Type = SelectionType.None;
        CurrentSelection.Id = null;
        EmitSignal(SignalName.SelectionChanged, (int)CurrentSelection.Type, CurrentSelection.Id);
    }
    
}