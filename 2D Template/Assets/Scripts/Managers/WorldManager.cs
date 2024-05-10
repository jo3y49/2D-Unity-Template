using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance { get; protected set; }
    public GameObject player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        GameDataManager gd = GameDataManager.Instance;
        
        player = PlayerMovement.Instance.gameObject;
        player.transform.position = gd.GetPlayerPosition();

        DebugStartCombat();
    }

    private void DebugStartCombat()
    {
        // Start combat for debugging
        // Find all enemies in the scene with the script enemyCombat
        EnemyCombat[] enemyScripts = FindObjectsOfType<EnemyCombat>();
        Queue<GameObject> enemies = new();
        foreach (EnemyCombat enemy in enemyScripts)
        {
            enemies.Enqueue(enemy.gameObject);
        }
        
        CombatManager combatManager = new CombatManager(player, enemies);
    }
}