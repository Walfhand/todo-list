using TodoList.Common.Core.Model;

namespace TodoList.API.Building.Todos.Domains;

public class Todo : Entity
{
    private readonly List<Item> _items = [];
    public string Name { get; private set; } = null!;
    public IReadOnlyList<Item> Items => _items.AsReadOnly();
    public bool HasItemsToProcess { get; private set; }

    private Todo() { }
    private Todo(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
        HasItemsToProcess = false;
    }
    public static Todo Create(string name)
    {
        return new Todo(name)
        {
            Id = Guid.NewGuid()
        };
    }

    public Item AddItem(string itemName)
    {
        if (_items.FirstOrDefault(i => i.Name == itemName) is not null)
        {
            //throw business errors
        }
        var item = Item.Create(itemName);
        _items.Add(item);
        UpdateHasItemsToProcess();
        return item;
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item is null)
        {
            //throw business errors
        }
        _items.RemoveAll(x => x.Id == itemId);
        UpdateHasItemsToProcess();
    }

    public void UpdateItemStatus(Guid itemId, ItemStatus status)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item is null)
        {
            //throw business errors
        }

        item.UpdateStatus(status);
        UpdateHasItemsToProcess();
    }

    private void UpdateHasItemsToProcess()
    {
        HasItemsToProcess = !_items.All(x => x.Status == ItemStatus.Done);
    }
}