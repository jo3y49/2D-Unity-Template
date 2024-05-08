using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Items", order = 0)]
public class Item : ScriptableObject {
    public string itemName = "";
    public Sprite icon;
    public string description = "";
}