using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour {
    public Stats stats { get; protected set;}
    protected Stats baseStats;
    protected int turnCount = 0;
    protected List<(Func<int>, int, int)> buffList = new();
    protected CharacterAction characterAction = new();

    protected virtual void Start() {
        baseStats = stats;

        characterAction.AddBattleAction("Attack", CharacterActionList.AttackCharacter);
    }

    public void PrepareBattle()
    {

    }

    public void StartTurn()
    {
        turnCount++;

        BuffCheck();
    }

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
    }
    
    public void StatBuff(StatEnum.StatType statEnum, int value, int buffLength = 1)
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

    public void StatDebuff(StatEnum.StatType statEnum, int value, int buffLength) => StatBuff(statEnum, -value, buffLength);

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

    public void EndBattle()
    {
        stats = baseStats;
        buffList.Clear();
    }
}