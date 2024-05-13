using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance { get; protected set; }
    [HideInInspector]
    public GameObject player;
    public MenuManager menuManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        GameDataManager gd = GameDataManager.Instance;
        
        player = PlayerMovement.Instance.gameObject;
        player.transform.position = gd.GetPlayerPosition();

        StartCoroutine(DebugStartCombat());
    }

    private IEnumerator DebugStartCombat()
    {
        yield return new WaitForSeconds(1);

        // Start combat for debugging
        // Find all enemies in the scene with the script enemyCombat
        EnemyCombat[] enemyScripts = FindObjectsOfType<EnemyCombat>();
        List<GameObject> enemies = new();
        foreach (EnemyCombat enemy in enemyScripts)
        {
            enemies.Add(enemy.gameObject);
        }

        menuManager.StartCombat(new List<GameObject> {player}, enemies);
    }
}