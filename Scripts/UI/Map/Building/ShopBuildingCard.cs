using Godot;

namespace Practice.Scripts.UI.Map.Building;

public partial class ShopBuildingCard : PanelContainer
{

    private string _buildingId;
    
    private TextureRect Icon;
    private Label Name;
    private Label Desc;
    private Label Cost;
    public Button BuyButton;
    
    
    public System.Action<string> OnBuy;
    
    public override void _Ready()
    {
        Icon = GetNode<TextureRect>("%Icon");
        Name = GetNode<Label>("%Name");
        Desc = GetNode<Label>("%Desc");
        Cost = GetNode<Label>("%Cost");
        BuyButton = GetNode<Button>("%BuyButton");
        
        BuyButton.Pressed += () => OnBuy?.Invoke(_buildingId);

    }

    public void SetData(string name, string desc, int cost, string buildingId)
    {
        _buildingId = buildingId;
        Name.Text = name;
        Desc.Text = desc;
        Cost.Text = $"Cost: {cost}";
    }

}