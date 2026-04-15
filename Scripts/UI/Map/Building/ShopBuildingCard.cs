using Godot;

namespace Practice.Scripts.UI.Map.Building;

public partial class ShopBuildingCard : PanelContainer
{

    private TextureRect Icon;
    private Label Name;
    private Label Desc;
    private Label Cost;
    public Button BuyButton;
    
    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("%Icon");
        Name = GetNode<Label>("%Name");
        Desc = GetNode<Label>("%Desc");
        Cost = GetNode<Label>("%Cost");
        BuyButton = GetNode<Button>("%BuyButton");
    }

    public void SetData(string name, string desc, int cost)
    {
        Name.Text = name;
        Desc.Text = desc;
        Cost.Text = $"Cost: {cost}";
    }

}