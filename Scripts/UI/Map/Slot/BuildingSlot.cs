using Godot;

namespace Practice.Scripts.UI.Map.Slot;

public partial class BuildingSlot : VBoxContainer
{

    private TextureRect Icon;
    private Label Name;
    private Label Level;
    
    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("%Icon");
        Name = GetNode<Label>("%Name");
        Level = GetNode<Label>("%Level");
    }
    
    public void SetData(string name, int level)
    {
        Name.Text = name;
        Level.Text = $"Level: {level}";
    }


}