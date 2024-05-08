using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour {
    private Item item;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI itemName;
    private int quantity;

    private void Awake() {
        RemoveItem();
    }
    
    public void AddItem(Item item, int quantity = 1) {
        this.item = item;
        icon.sprite = item.icon;
        this.quantity = quantity;
        if (itemName != null) itemName.text = item.itemName + $" ({quantity})";
        icon.color = Color.white;
    }

    public void RemoveItem() {
        item = null;
        quantity = 0;
        if (itemName != null) itemName.text = string.Empty;
        icon.color = Color.clear;
    }

    public void Select() {
        if (item == null) return;
        
        Debug.Log($"Selected {item.itemName}");
    }
}