using Godot;

namespace Practice.Scripts.UI.Map.Slot;

public partial class EmptySlot : CenterContainer
{

    public Button AddButton;
    public System.Action OnAddClicked;
    
    public override void _Ready()
    {
        AddButton = GetNode<Button>("%AddButton");
        AddButton.Pressed += () => OnAddClicked?.Invoke();
    }

}