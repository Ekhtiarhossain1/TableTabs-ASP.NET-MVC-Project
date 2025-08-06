namespace TableTabsApp.Models;

public class Order
{
    public int Id { get; set; }
    public string TableName { get; set; }
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }
    public int Quantity { get; set; }
    public bool IsReady { get; set; } // false = not ready, true = ready
}
