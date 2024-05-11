using UnityEngine;

public static class CharacterActionList
{
    public static void AttackCharacter(CharacterCombat user, CharacterCombat target)
    {
        Debug.Log($"{user} attacks {target}!");
    }
}