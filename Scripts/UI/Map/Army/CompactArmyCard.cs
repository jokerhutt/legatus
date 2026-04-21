using Godot;

namespace Legatus.Scripts.UI.Map.Army;

public partial class CompactArmyCard : PanelContainer
{
    
    private Label ArmyName;
    private Label UnitCount;
    private Label SoldiersCount;
    private TextureButton SelectUnitButton;

    public override void _Ready()
    {
        ArmyName = GetNode<Label>("%ArmyName");
        UnitCount = GetNode<Label>("%UnitCount");
        SoldiersCount = GetNode<Label>("%SoldiersCount");
        SelectUnitButton = GetNode<TextureButton>("%SelectUnitButton");
    }

    public void SetArmy(string name, int units, int soldiers)
    {
        ArmyName.Text = name;
        UnitCount.Text = $"{units} Units";
        SoldiersCount.Text = $"{soldiers}";
    }
    
}