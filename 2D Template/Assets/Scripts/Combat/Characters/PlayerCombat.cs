using UnityEngine;

public class PlayerCombat : CharacterCombat {
    protected override void Start()
    {
        stats = baseStats = GameDataManager.Instance.GetStats();

        FillActionList();
    }
}