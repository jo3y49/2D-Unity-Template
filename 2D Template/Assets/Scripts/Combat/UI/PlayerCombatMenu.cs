using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCombatMenu : MonoBehaviour {
    public Button buttonPrefab;
    public GameObject attackButtonContainer, itemButtonContainer, enemyButtonContainer;
    private CombatManager combatManager;
    private CombatMenuManager combatMenuManager;
    private Dictionary<PlayerCombat, PlayerCombatOptions> playerOptions = new();
    private Dictionary<EnemyCombat, Button> enemyButtons = new();
    private string selectedAction;
    private PlayerCombat activePlayer;
    private CharacterCombat target;

    public void Initialize(List<PlayerCombat> players, List<EnemyCombat> enemies, CombatManager combatManager, CombatMenuManager combatMenuManager)
    {
        playerOptions.Clear();
        this.combatManager = combatManager;
        this.combatMenuManager = combatMenuManager;
        
        SetPlayerButtons(players);
        SetEnemyButtons(enemies);

        gameObject.SetActive(false);
    }

    private void SelectAction(string action)
    {
        selectedAction = action;
        playerOptions[activePlayer].DeactivateAllButtons();
        enemyButtons.Values.ToList().ForEach(button => button.gameObject.SetActive(true));
    }

    private void SelectTarget(CharacterCombat target)
    {
        this.target = target;
        enemyButtons.Values.ToList().ForEach(button => button.gameObject.SetActive(false));
        SendAction();
    }

    private void SendAction()
    {
        combatManager.ReceiveAction(selectedAction, activePlayer, target);
        gameObject.SetActive(false);
    }

    public void Activate(PlayerCombat player)
    {
        gameObject.SetActive(true);
        activePlayer = player;

        PlayerCombatOptions playerOption = playerOptions[activePlayer];
        playerOption.ActivateButtons(playerOption.attacks.Values.ToList());
    }

    private void SetPlayerButtons(List<PlayerCombat> players)
    {
        foreach (PlayerCombat player in players)
        {
            PlayerCombatOptions options = new(player, buttonPrefab);
                
            foreach (KeyValuePair<string, Button> attack in options.attacks)
            {
                attack.Value.onClick.AddListener(() => SelectAction(attack.Key));
                // attack.Value.OnPointerEnter((eventData) => HoverAction(attack.Key, eventData));

                attack.Value.transform.SetParent(attackButtonContainer.transform);
                attack.Value.gameObject.SetActive(false);
            }

            foreach (KeyValuePair<string, Button> item in options.items)
            {
                item.Value.onClick.AddListener(() => SelectAction(item.Key));
                item.Value.transform.SetParent(itemButtonContainer.transform);
                
                item.Value.gameObject.SetActive(false);
            }

            playerOptions.Add(player, options);
        }
    }

    private void HoverAction(string action, PointerEventData eventData)
    {
        combatMenuManager.ActiveText(action);
    }

    private void SetEnemyButtons(List<EnemyCombat> enemies)
    {
        foreach (EnemyCombat enemy in enemies)
        {
            Button button = Instantiate(buttonPrefab, enemyButtonContainer.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = enemy.name;
            button.onClick.AddListener(() => SelectTarget(enemy));
            enemyButtons.Add(enemy, button);
            button.gameObject.SetActive(false);
        }
    }
}