using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    public List<PlayerCombat> players { get; private set;}
    public List<EnemyCombat> enemies { get; private set;}
    private Queue<CharacterCombat> characterOrder = new();
    private Coroutine combatCoroutine = null;
    [SerializeField] private PlayerCombatMenu playerCombatMenu;
    public bool fight = true;
    
    public void Initialize(List<GameObject> playerObject, List<GameObject> enemyObjects)
    {
        enabled = true;
        players = new();
        enemies = new();
        characterOrder = new();
        fight = true;

        SortedSet<CharacterCombat> sortedCharacters = new(
            Comparer<CharacterCombat>.Create((c1, c2) => c2.stats.GetSpeed().CompareTo(c1.stats.GetSpeed()))
        );

        foreach (GameObject player in playerObject)
        {
            player.TryGetComponent(out PlayerCombat playerCombat);
            players.Add(playerCombat);
            sortedCharacters.Add(playerCombat);
        }
        foreach (GameObject enemyObject in enemyObjects)
        {
            enemyObject.TryGetComponent(out EnemyCombat enemy);
            enemies.Add(enemy);
            sortedCharacters.Add(enemy);
        }

        foreach (CharacterCombat character in sortedCharacters)
        {
            characterOrder.Enqueue(character);
            character.PrepareBattle();
        }

        StartCoroutine(StartCombat());
    }

    private IEnumerator StartCombat()
    {
        bool fight = true;

        while (fight && characterOrder.Count > 0)
        {
            CharacterCombat currentCharacter = characterOrder.Dequeue();
            
            if (currentCharacter.GetType() == typeof(PlayerCombat)) combatCoroutine = StartCoroutine(PlayerTurn((PlayerCombat) currentCharacter));
            
            else combatCoroutine = StartCoroutine(EnemyTurn((EnemyCombat) currentCharacter));

            yield return combatCoroutine;

            if (currentCharacter.stats.GetHealth() > 0)
                characterOrder.Enqueue(currentCharacter);
            
        }
    }

    private IEnumerator PlayerTurn(PlayerCombat player)
    {
        player.StartTurn();
        playerCombatMenu.Activate(player);

        yield return playerCombatMenu.Completed();
    }

    private IEnumerator EnemyTurn(EnemyCombat enemy)
    {
        enemy.StartTurn();
        yield return new WaitForSeconds(1f);
    }
}