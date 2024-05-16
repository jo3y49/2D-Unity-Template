using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatMenuManager : MonoBehaviour {
    private Dictionary<string, Stats> playerStats = new();
    private Dictionary<string, Stats> enemyStats = new();
    private List<string> battleLog = new();
    public TextMeshProUGUI battleText;
    public GameObject playerNameContainer, enemyNameContainer;
    public GameObject characterInfoPrefab;
    private Dictionary<string, TextMeshProUGUI> characterInfoList = new();

    public void Initialize(List<PlayerCombat> playerObjects, List<EnemyCombat> enemyObjects) {
        foreach (PlayerCombat player in playerObjects) {
            playerStats.Add(player.name, player.stats);
        }

        foreach (EnemyCombat enemy in enemyObjects) {
            enemyStats.Add(enemy.name, enemy.stats);
        }

        SetHealthMenus();
        battleText.text = "";
        battleLog.Clear();
    }

    private void SetHealthMenus()
    {
        for (int i = playerNameContainer.transform.childCount - 1; i >= 0; i--) {
            Destroy(playerNameContainer.transform.GetChild(i).gameObject);
        }
        for (int i = enemyNameContainer.transform.childCount - 1; i >= 0; i--) {
            Destroy(enemyNameContainer.transform.GetChild(i).gameObject);
        }
        foreach (KeyValuePair<string, Stats> player in playerStats) {
            GameObject characterInfo = Instantiate(characterInfoPrefab, playerNameContainer.transform);
            TextMeshProUGUI text = characterInfo.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"{player.Key}: {player.Value.GetHealth()}";
            characterInfoList.Add(player.Key, text);
        }

        foreach (KeyValuePair<string, Stats> enemy in enemyStats) {
            GameObject characterInfo = Instantiate(characterInfoPrefab, enemyNameContainer.transform);
            TextMeshProUGUI text = characterInfo.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"{enemy.Key}: {enemy.Value.GetHealth()}";
            characterInfoList.Add(enemy.Key, text);
        }
    }

    public void UpdateUI() {
        UpdateHealth();
    }

    private void UpdateHealth() {
        foreach (KeyValuePair<string, Stats> player in playerStats) {
            if (player.Value.GetHealth() <= 0) {
                characterInfoList[player.Key].text = $"{player.Key}: is Defeated";
            } else {
                characterInfoList[player.Key].text = $"{player.Key}: {player.Value.GetHealth()}";
            }
        }

        foreach (KeyValuePair<string, Stats> enemy in enemyStats) {
            if (enemy.Value.GetHealth() <= 0) {
                characterInfoList[enemy.Key].text = $"{enemy.Key}: is Defeated";
            } else {
                characterInfoList[enemy.Key].text = $"{enemy.Key}: {enemy.Value.GetHealth()}";
            }
        }
    }

    public void ActiveText(string text) {
        battleText.text = text;
        AddToBattleLog(text);
        Debug.Log(text);
    }

    private void AddToBattleLog(string text) {
        battleLog.Add(text);
    }
}