using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCombat : CharacterCombat {
    public EnemyData enemyData;

    protected override void Start()
    {
        stats = baseStats = enemyData.stats;

        FillActionList();
    }

    public (string, CharacterCombat) DecideAction(List<PlayerCombat> target)
    {
        string action = CharacterAction.battleActions.Keys.ToList()[0];
        CharacterCombat targetCharacter = target[0];

        return (action, targetCharacter);
    }
}