using System.Collections.Generic;
using UnityEngine;

public class CombatManager {
    private PlayerCombat player;
    private Queue<EnemyCombat> enemies = new();
    
    public CombatManager(GameObject player, Queue<GameObject> enemies)
    {
        player.TryGetComponent(out this.player);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies.Dequeue().TryGetComponent(out EnemyCombat enemy);
            this.enemies.Enqueue(enemy);
        }

        Queue<CharacterCombat> characters = new Queue<CharacterCombat>();

        StartCombat();
    }

    private void StartCombat()
    {
        bool fight = true;

        while (fight && player.stats.GetHealth() > 0 && enemies.Count > 0)
        {
            
        }
    }
}