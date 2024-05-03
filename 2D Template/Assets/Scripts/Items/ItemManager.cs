using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager {
    private static ItemManager Instance;
    private readonly IDictionary<string, Item> items = new Dictionary<string, Item>();
    public SortEnum.SortType sortType = SortEnum.SortType.Name;
    public SortEnum.SortOrder sortOrder = SortEnum.SortOrder.Ascending;

    public ItemManager()
    {
        Item[] itemList = Resources.LoadAll<Item>("Items");

        foreach (Item item in itemList)
        {
            items.Add(item.itemName, item);
        }

        items = SortEnum.Sort(items, sortType, sortOrder);
    }

    public static ItemManager GetInstance()
    {
        Instance ??= new ItemManager();

        return Instance;
    }

    public Item GetItem(string itemName)
    {
        if (items.ContainsKey(itemName)) return items[itemName];

        else return null;
    }

    public Item GetItem(int index)
    {
        if (index >= 0 || index < items.Count) return items.ElementAt(index).Value;

        else return null;
    }

    public Item GetRandomItem()
    {
        return items.ElementAt(Random.Range(0, items.Count)).Value;
    }

    public IDictionary<string, Item> GetItems() { return items; }

    public static void UseItem(Item item)
    {
        // add item use logic here

        GameDataManager.Instance.RemoveFromInventory(item);
    }
}