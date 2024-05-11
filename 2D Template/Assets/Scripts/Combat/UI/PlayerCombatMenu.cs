using UnityEngine;

public class PlayerCombatMenu : MonoBehaviour {
    private bool active = false;
    public void Activate(PlayerCombat player)
    {
        active = true;
    }

    public bool Completed()
    {
        return !active;
    }
}