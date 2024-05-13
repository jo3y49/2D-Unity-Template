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
    private bool endTurn = false;
    
    public void Initialize(List<GameObject> playerObject, List<GameObject> enemyObjects)
    {
        enabled = true;
        players = new();
        enemies = new();
        characterOrder = new();
        fight = true;

        SortedSet<CharacterCombat> sortedCharacters = new(
            
            Comparer<CharacterCombat>.Create((c1, c2) =>
            {
                int speedComparison = c2.stats.GetSpeed().CompareTo(c1.stats.GetSpeed());
                if (speedComparison == 0) 
                {
                    if (c1 is PlayerCombat && c2 is not PlayerCombat) return -1;
                    if (c2 is PlayerCombat && c1 is not PlayerCombat) return 1;

                    int nameComparison = c1.name.CompareTo(c2.name);
                    return nameComparison;
                }

                return speedComparison;
            })
        );

        foreach (GameObject enemy in enemyObjects)
        {
            enemy.TryGetComponent(out EnemyCombat enemyCombat);
            enemies.Add(enemyCombat);
            sortedCharacters.Add(enemyCombat);
        }

        foreach (GameObject player in playerObject)
        {
            player.TryGetComponent(out PlayerCombat playerCombat);
            players.Add(playerCombat);
            sortedCharacters.Add(playerCombat);
        }
        

        foreach (CharacterCombat character in sortedCharacters)
        {
            characterOrder.Enqueue(character);
            character.PrepareBattle();
        }

        playerCombatMenu.Initialize(players, enemies, this);

        StartCoroutine(StartCombat());
    }

    private IEnumerator StartCombat()
    {
        bool fight = true;

        while (fight && characterOrder.Count > 0)
        {
            endTurn = false;
            CharacterCombat currentCharacter = characterOrder.Dequeue();
            if (currentCharacter is PlayerCombat player) combatCoroutine = StartCoroutine(PlayerTurn(player));
    
            else combatCoroutine = StartCoroutine(EnemyTurn((EnemyCombat) currentCharacter));

            yield return combatCoroutine;

            if (currentCharacter.stats.GetHealth() >= 0)
                characterOrder.Enqueue(currentCharacter);
            
        }

        MenuManager.Instance.EndCombat();
    }

    private IEnumerator PlayerTurn(PlayerCombat player)
    {
        Debug.Log("Player turn" + player.name);

        player.StartTurn();
        playerCombatMenu.Activate(player);

        yield return new WaitUntil(() => endTurn);
        // yield return playerCombatMenu.Completed();
    }

    public void ReceiveAction(string action, CharacterCombat user, CharacterCombat target)
    {
        user.DoAction(action, target);
        endTurn = true;
    }

    private IEnumerator EnemyTurn(EnemyCombat enemy)
    {
        Debug.Log("Enemy turn" + enemy.name);

        enemy.StartTurn();

        enemy.DoRandomAction(players[0]);

        yield return new WaitForSeconds(1f);
    }
}