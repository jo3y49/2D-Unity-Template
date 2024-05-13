using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour {
    public Stats stats { get; protected set;}
    protected Stats baseStats;
    protected int turnCount = 0;
    protected List<(Func<int>, int, int)> buffList = new();
    public CharacterAction CharacterAction {get; protected set;} = new();

    protected virtual void Start() {
        baseStats = stats;

        
    }

    public virtual void PrepareBattle()
    {

    }

    public virtual void DoRandomAction(CharacterCombat target)
    {
        CharacterAction.PerformRandomBattleAction(this, target);
    }

    public virtual void DoAction(string actionName, CharacterCombat target)
    {
        if (CharacterAction.battleActions.ContainsKey(actionName))
        {
            CharacterAction.battleActions[actionName](this, target);
        }
    }

    public virtual void StartTurn()
    {
        turnCount++;

        BuffCheck();
    }

    public virtual void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
    }
    
    public virtual void StatBuff(StatEnum.StatType statEnum, int value, int buffLength = 1)
    {
        Func<int> targetStat = null;

        switch (statEnum)
        {
            case StatEnum.StatType.Health:
                targetStat = () => stats.GetMaxHealth();
                break;
            case StatEnum.StatType.Attack:
                targetStat = () => stats.GetAttack();
                break;
            case StatEnum.StatType.Defense:
                targetStat = () => stats.GetDefense();
                break;
            case StatEnum.StatType.Speed:
                targetStat = () => stats.GetSpeed();
                break;
        }

        if (targetStat != null)
        {
            if (targetStat() + value < 0)
            {
                value = -targetStat();
            }

            // Apply the buff
            targetStat += () => value;

            buffList.Add((targetStat, value, buffLength));
        }
    }

    public virtual void StatDebuff(StatEnum.StatType statEnum, int value, int buffLength) => StatBuff(statEnum, -value, buffLength);

    private void BuffCheck()
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            var buff = buffList[i];

            // Decrement the remaining turns for the buff
            buff = (buff.Item1, buff.Item2, buff.Item3 - 1);

            if (buff.Item3 <= 0)
            {
                // Revert the buff if the remaining turns reach 0 or less
                RevertBuff(buff);
                i--;
            }
            else
            {
                buffList[i] = buff; // Update the buff in the list
            }
        }
    }

    private void RevertBuff((Func<int>, int, int) buff)
    {
        buff.Item1 -= () => buff.Item2;
        buffList.Remove(buff);
    }

    public virtual void EndBattle()
    {
        stats = baseStats;
        buffList.Clear();
    }
}