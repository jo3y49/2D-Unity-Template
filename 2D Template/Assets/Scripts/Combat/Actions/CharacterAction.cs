using System;
using System.Collections.Generic;
using System.Linq;

public class CharacterAction {
    public delegate ActionResult BattleAction(CharacterCombat user, CharacterCombat target = null);

    // List to store battle actions
    public Dictionary<string, BattleAction> battleActions = new();

    // Method to add a battle action to the list
    public void AddToActionList(string actionName) {
        battleActions.Add(actionName, CharacterActionList.GetAction(actionName));
    }

    // Method to perform a random battle action from the list with parameters
    public ActionResult PerformRandomBattleAction(CharacterCombat user, CharacterCombat target) {
        if (battleActions.Count > 0) {
            Random rand = new Random();
            int index = rand.Next(0, battleActions.Count);
            BattleAction action = battleActions.ElementAt(index).Value;
            return action(user, target); // Invoke the selected battle action with the parameters
        } else {
            return null;
        }
    }
}