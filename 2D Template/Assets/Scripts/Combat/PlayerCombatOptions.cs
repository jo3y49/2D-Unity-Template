using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatOptions
{
    public Dictionary<string, Button> attacks = new();
    public Dictionary<string, Button> items = new();

    public PlayerCombatOptions(PlayerCombat playerCombat, Button buttonPrefab)
    {
        foreach (string name in playerCombat.CharacterAction.battleActions.Keys)
        {
            attacks.Add(name, GenerateButton(buttonPrefab, name));
        }

        foreach (Item item in GameDataManager.Instance.GetInventory().Keys)
        {
            items.Add(item.itemName, GenerateButton(buttonPrefab, item.itemName));
        }
    }

    public void ActivateButtons(List<Button> buttons)
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void DeactivateAllButtons()
    {
        foreach (Button button in attacks.Values)
        {
            button.gameObject.SetActive(false);
        }

        foreach (Button button in items.Values)
        {
            button.gameObject.SetActive(false);
        }
    }   

    private Button GenerateButton(Button buttonPrefab, string text)
    {
        Button button = Object.Instantiate(buttonPrefab);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        return button;
    }
}