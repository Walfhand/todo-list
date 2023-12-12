using TodoList.Common.Core.Model;

namespace TodoList.API.Building.Todos.Domains;

public class Item : Entity
{
    public string Name { get; private set; } = null!;
    public ItemStatus Status { get; private set; }
    private Item() { }
    private Item(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        Status = ItemStatus.Todo;
    }

    public static Item Create(string name)
    {
        return new Item(name);
    }

    public void UpdateStatus(ItemStatus status)
    {
        Status = status;
    }
}
