using System;
using System.Collections.Generic;
using System.Linq;

public class CharacterAction {
    public delegate void BattleAction(CharacterCombat user, CharacterCombat target = null);

    // List to store battle actions
    public Dictionary<string, BattleAction> battleActions = new();

    // Method to add a battle action to the list
    public void AddBattleAction(string actionName, BattleAction action) {
        battleActions.Add(actionName, action);
    }

    // Method to perform a random battle action from the list with parameters
    public void PerformRandomBattleAction(CharacterCombat user, CharacterCombat target) {
        if (battleActions.Count > 0) {
            Random rand = new Random();
            int index = rand.Next(0, battleActions.Count);
            BattleAction action = battleActions.ElementAt(index).Value;
            action(user, target); // Invoke the selected battle action with the parameters
        }
    }
}