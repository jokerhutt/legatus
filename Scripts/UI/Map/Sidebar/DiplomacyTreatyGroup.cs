using Godot;

namespace Legatus.Scripts.UI.Map.Sidebar;

public partial class DiplomacyTreatyGroup : HBoxContainer
{
    private Label Name;
    private Label Duration;
    
    public override void _Ready()
    {
        Name = GetNode<Label>("%Name");
        Duration = GetNode<Label>("%Duration");
    }
    
    public void SetData(string name, int duration)
    {
        Name.Text = name;
        Duration.Text = $"Duration: {duration} turns";
    }

}