using UnityEngine;

public class EnemyCombat : CharacterCombat {
    public EnemyData enemyData;

    protected override void Start()
    {
        stats = baseStats = enemyData.stats;
    }
}