using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    public List<PlayerCombat> players { get; private set;}
    public List<EnemyCombat> enemies { get; private set;}
    private Queue<CharacterCombat> characterOrder = new();

    [SerializeField] private PlayerCombatMenu playerCombatMenu;

    public bool fight = true;
    private bool menuCommandGiven = false;
    public int turnCount = 0;
    public bool endUserTurn = true;

    public float placeholderAnimationWaitTime = 1f;
    
    public void Initialize(List<GameObject> playerObject, List<GameObject> enemyObjects)
    {
        enabled = true;

        ResetCountingVariables();
        SortCharactersIntoQueue(playerObject, enemyObjects);

        playerCombatMenu.Initialize(players, enemies, this);

        StartCoroutine(StartCombat());
    }

    private IEnumerator StartCombat()
    {
        while (fight)
        {
            CharacterCombat currentCharacter = characterOrder.Peek();
            Coroutine characterAction = CharacterAction(currentCharacter);

            yield return characterAction;

            fight = NextTurnLogic(currentCharacter);
        }

        MenuManager.Instance.EndCombat();
    }

    private IEnumerator PlayerTurn(PlayerCombat player)
    {
        Debug.Log("Player turn" + player.name);
        menuCommandGiven = false;

        player.StartTurn();
        playerCombatMenu.Activate(player);

        yield return new WaitUntil(() => menuCommandGiven);

        yield return new WaitForSeconds(placeholderAnimationWaitTime);
        
    }

    private IEnumerator EnemyTurn(EnemyCombat enemy)
    {
        Debug.Log("Enemy turn" + enemy.name);

        enemy.StartTurn();
        EnemyAiAction(enemy);

        yield return new WaitForSeconds(placeholderAnimationWaitTime);
    }

    public void ReceiveAction(string action, CharacterCombat user, CharacterCombat target)
    {
        ActionResult actionResult = user.DoAction(action, target);
        
        HandleActionResult(actionResult);

        menuCommandGiven = true;
    }

    private void HandleActionResult(ActionResult actionResult)
    {
        if (actionResult.message != null) Debug.Log(actionResult.message);

        switch (actionResult.resultType)
        {
            case ActionResult.ResultType.Damage:
                actionResult.target.TakeDamage(actionResult.value);
                break;
            case ActionResult.ResultType.Heal:
                actionResult.target.Heal(actionResult.value);
                break;
            // case ActionResult.ResultType.Buff:
            //     actionResult.target.StatBuff(actionResult.statEnum, actionResult.value, actionResult.buffLength);
            //     break;
            // case ActionResult.ResultType.Debuff:
            //     actionResult.target.StatDebuff(actionResult.statEnum, actionResult.value, actionResult.buffLength);
            //     break;
            case ActionResult.ResultType.Miss:
                
                break;
        }

        endUserTurn = !actionResult.stun;
    }

    private void EnemyAiAction(EnemyCombat enemy)
    {
        (string, CharacterCombat) action = enemy.DecideAction(players);

        ReceiveAction(action.Item1, enemy, action.Item2);
    }

    private bool NextTurnLogic(CharacterCombat currentCharacter)
    {
        // CheckForDeaths();

        if (currentCharacter.stats.GetHealth() >= 0 && endUserTurn)
        {
            characterOrder.Dequeue();
            characterOrder.Enqueue(currentCharacter);
        }

        bool fight = characterOrder.Count > 1;

        return fight;
    }

    private void CheckForDeaths()
    {
        foreach (CharacterCombat character in characterOrder)
        {
            if (character.stats.GetHealth() <= 0)
            {
                
                character.EndBattle();
            }
        }
    }

    private Coroutine CharacterAction(CharacterCombat currentCharacter)
    {
        Coroutine combatCoroutine;

        if (currentCharacter is PlayerCombat player) combatCoroutine = StartCoroutine(PlayerTurn(player));

        else combatCoroutine = StartCoroutine(EnemyTurn((EnemyCombat)currentCharacter));

        return combatCoroutine;
    }

    private void ResetCountingVariables()
    {
        players = new();
        enemies = new();
        characterOrder = new();
        fight = true;
        turnCount = 0;
    }

    private void SortCharactersIntoQueue(List<GameObject> playerObject, List<GameObject> enemyObjects)
    {
        // consider changing to ffx system where attacks have a speed value
        SortedSet<CharacterCombat> sortedCharacters = InitiativeOrder();

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
    }

    private SortedSet<CharacterCombat> InitiativeOrder()
    {
        return new(

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
    }
}