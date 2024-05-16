using System.Collections.Generic;
using UnityEngine;
using static ActionResult;

public static class CharacterActionList
{
    private static Dictionary<string, CharacterAction.BattleAction> battleActions = new();

    static CharacterActionList()
    {
        FillActionList();
    }

    public static ActionResult BaseAttack(CharacterCombat user, CharacterCombat target)
    {
        string message;
        int damage = 0;
        bool stun = false;
        ResultType resultType = Random.Range(0, 100) < 90 ? ResultType.Damage : ResultType.Miss;

        if (resultType == ResultType.Damage)
        {
            damage = user.stats.GetAttack() - target.stats.GetDefense();

            if (Random.Range(0, 100) < 10)
            {
                stun = true;
                message = $"{user.name} stuns {target.name}!";
            }
            else
                message = $"{user.name} attacks {target.name}!";
            
        } else
        {
            message = $"{user.name} missed!";
        }

        ActionResult result = new(user, resultType, damage)
        {
            stun = stun,
            message = message,
            target = target
        };

        return result;
    }
    

    public static CharacterAction.BattleAction GetAction(string actionName)
    {
        if (battleActions.ContainsKey(actionName))
        {
            return battleActions[actionName];
        }

        return null;
    }

    private static void FillActionList()
    {
        battleActions.Add("Attack", BaseAttack);
    }
}