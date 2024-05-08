using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {
    public GameObject inventoryContainer, inventorySlotPrefab;
    public Button closeInventoryButton;
    public int inventorySize = 20;
    private InventorySlot[] inventorySlots;

    private void Awake() {
        closeInventoryButton.onClick.AddListener(ToggleInventory);

        foreach (Transform child in inventoryContainer.transform) {
            Destroy(child.gameObject);
        }

        inventorySlots = new InventorySlot[inventorySize];

        foreach (int i in Enumerable.Range(0, inventorySize)) {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer.transform);
            inventorySlots[i] = slot.GetComponent<InventorySlot>();
        }
    }

    private void OnEnable() {
        UpdateInventory();
    }

    public void ToggleInventory() {
        MenuManager.Instance.ToggleMenu(gameObject);
    }

    public void UpdateInventory()
    {
        if (GameDataManager.Instance == null || inventorySlots.Length <= 0) return;

        // IDictionary<Item, int> inventory = GameDataManager.Instance.GetInventory();
        IDictionary<Item, int> inventory = DebugAddItems();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.Count)
            {
                KeyValuePair<Item, int> item = inventory.ElementAt(i);
                inventorySlots[i].gameObject.SetActive(true);
                inventorySlots[i].AddItem(item.Key, item.Value);
            }
            else
            {
                inventorySlots[i].RemoveItem();
                inventorySlots[i].gameObject.SetActive(false);
            }
        }
    }

    private IDictionary<Item, int> DebugAddItems()
    {
        IDictionary<Item, int> inventory = new Dictionary<Item, int>();

        foreach (Item item in ItemManager.GetInstance().GetItems().Values) {
            inventory.Add(item, Random.Range(0, 10));
        }
        return inventory;
    }
}