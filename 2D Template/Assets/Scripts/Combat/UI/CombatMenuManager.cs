using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatMenuManager : MonoBehaviour {
    public CombatManager combatManager;
    private Dictionary<string, Stats> playerStats = new();
    private Dictionary<string, Stats> enemyStats = new();
    public GameObject playerNameContainer, enemyNameContainer;
    public GameObject characterInfoPrefab;

    public void Initialize(List<GameObject> playerObjects, List<GameObject> enemyObjects) {
        combatManager.Initialize(playerObjects, enemyObjects);

        foreach (GameObject playerObject in playerObjects) {
            playerObject.TryGetComponent(out PlayerCombat playerCombat);
            playerStats.Add(playerCombat.name, playerCombat.stats);
        }

        foreach (GameObject enemyObject in enemyObjects) {
            enemyObject.TryGetComponent(out EnemyCombat enemyCombat);
            enemyStats.Add(enemyCombat.name, enemyCombat.stats);
        }

        SetHealthMenus();
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
            characterInfo.GetComponentInChildren<TextMeshProUGUI>().text = $"{player.Key}: {player.Value.GetHealth()}";
        }

        foreach (KeyValuePair<string, Stats> enemy in enemyStats) {
            GameObject characterInfo = Instantiate(characterInfoPrefab, enemyNameContainer.transform);
            characterInfo.GetComponentInChildren<TextMeshProUGUI>().text = $"{enemy.Key}: {enemy.Value.GetHealth()}";
        }
    }
}