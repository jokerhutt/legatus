using Godot;

namespace Practice.Scripts.UI.Map.Army;

public partial class CompactArmyCard : PanelContainer
{
    
    private Label ArmyName;
    private Label UnitCount;

    public override void _Ready()
    {
        ArmyName = GetNode<Label>("%ArmyName");
        UnitCount = GetNode<Label>("%UnitCount");
    }

    public void SetArmy(string name, int units, int soldiers)
    {
        ArmyName.Text = name;
        UnitCount.Text = $"{units} Units | {soldiers} Men";
    }
    
}