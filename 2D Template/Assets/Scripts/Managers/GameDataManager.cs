using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : GameManager {

    //PlayerData

    public void SetMaxHealth(int maxHealth)
    {
        gameData.playerData.maxHealth = maxHealth;
    }

    public void AddMaxHealth(int maxHealth = 1)
    {
        gameData.playerData.maxHealth += maxHealth;
    }

    public int GetMaxHealth()
    {
        return gameData.playerData.maxHealth;
    }

    public void AddHealth(int health = 1)
    {
        gameData.playerData.health += health;

        gameData.playerData.health = Mathf.Min(health, gameData.playerData.maxHealth);
    }

    public void ResetHealth()
    {
        gameData.playerData.health = gameData.playerData.maxHealth;
    }

    public int GetHealth()
    {
        return gameData.playerData.health;
    }

    public bool AddLives(int lives = 1)
    {
        if (gameData.playerData.lives < 0)
        {
            return RemoveLives(lives);
        }

        gameData.playerData.lives += lives;

        return true;
    }

    public bool RemoveLives(int lives = 1)
    {
        Mathf.Abs(lives);
        if (gameData.playerData.lives - lives <= 0)
        {
            gameData.statData.livesLost += gameData.playerData.lives;
            gameData.playerData.lives = 0;
            return false;
        }

        gameData.playerData.lives -= lives;
        gameData.statData.livesLost += lives;

        return true;
    }

    public int GetLives()
    {
        return gameData.playerData.lives;
    }

    public void AddCoins(int coins = 1)
    {
        if (coins < 0)
        {
            RemoveCoins(coins);
        }

        gameData.playerData.coins += coins;
        gameData.statData.coinsCollected += coins;
    }

    public void RemoveCoins(int coins = 1)
    {
        Mathf.Abs(coins);
        if (gameData.playerData.coins - coins < 0)
        {
            gameData.playerData.coins = 0;
        }

        gameData.playerData.coins -= coins;
    }

    public int GetCoins()
    {
        return gameData.playerData.coins;
    }

    public void SetLevel(int level)
    {
        gameData.playerData.level = level;
    }

    public void AddLevel(int level = 1)
    {
        gameData.playerData.level += level;
    }

    public int GetLevel()
    {
        return gameData.playerData.level;
    }

    public void AddXP(int xp = 1)
    {
        gameData.playerData.xp += xp;
    }

    public int GetXP()
    {
        return gameData.playerData.xp;
    }

    public void SetScore(int score)
    {
        gameData.playerData.score = score;
    }

    public void AddScore(int score = 1)
    {
        if (score < 0)
        {
            RemoveScore(score);
        }

        gameData.playerData.score += score;
        gameData.statData.scoreCollected += score;
    }

    public void RemoveScore(int score = 1)
    {
        Mathf.Abs(score);
        if (gameData.playerData.score - score <= 0)
        {
            gameData.playerData.score = 0;
        }

        gameData.playerData.score -= score;
    }

    public int GetScore()
    {
        return gameData.playerData.score;
    }

    public bool SetInventory(IDictionary<Item, int> inventory)
    {
        if (inventory.Values.Sum() > gameData.playerData.inventoryCap)
        {
            return false;
        }

        gameData.playerData.inventory = inventory;

        return true;
    }

    public bool AddToInventory(Item item, int quantity = 1)
    {
        if (quantity < 0)
        {
            return RemoveFromInventory(item, quantity);
        }

        if (gameData.playerData.inventory.Values.Sum() >= gameData.playerData.inventoryCap)
        {
            return false;
        } else if (gameData.playerData.inventory.Values.Sum() + quantity > gameData.playerData.inventoryCap)
        {
            quantity = gameData.playerData.inventoryCap - gameData.playerData.inventory.Values.Sum();
        }

        if (gameData.playerData.inventory.ContainsKey(item))
        {
            gameData.playerData.inventory[item] += quantity;
        }
        else
        {
            gameData.playerData.inventory.Add(item, quantity);
        }

        gameData.statData.itemsCollected += quantity;

        return true;
    }

    public bool RemoveFromInventory(Item item, int quantity = 1)
    {
        Mathf.Abs(quantity);
        if (gameData.playerData.inventory.ContainsKey(item))
        {
            if (gameData.playerData.inventory[item] - quantity <= 0)
            {
                gameData.playerData.inventory.Remove(item);
            }
            else
            {
                gameData.playerData.inventory[item] -= quantity;
            }

            return true;
        }

        return false;
    }

    public void ClearInventory()
    {
        gameData.playerData.inventory.Clear();
    }

    public IDictionary<Item, int> GetInventory()
    {
        return gameData.playerData.inventory;
    }

    public int GetQuantityFromInventory(Item item)
    {
        if (gameData.playerData.inventory.ContainsKey(item))
        {
            return gameData.playerData.inventory[item];
        }

        return 0;
    }

    public void AddInventoryCap(int inventoryCap = 1)
    {
        if (gameData.playerData.inventory.Values.Sum() > gameData.playerData.inventoryCap + inventoryCap) return;

        gameData.playerData.inventoryCap += inventoryCap;
    }

    public int GetInventoryCap()
    {
        return gameData.playerData.inventoryCap;
    }

    //StatData

    public void SetPlaytime(float playtime)
    {
        gameData.statData.playtime = playtime;
    }

    public void AddPlaytime(float playtime)
    {
        gameData.statData.playtime += playtime;
    }

    public float GetPlaytime()
    {
        return gameData.statData.playtime;
    }

    public void AddDeaths(int deaths = 1)
    {
        gameData.statData.deaths += deaths;
    }

    public int GetDeaths()
    {
        return gameData.statData.deaths;
    }

    public void AddKills(int kills = 1)
    {
        gameData.statData.kills += kills;
    }

    public int GetKills()
    {
        return gameData.statData.kills;
    }

    public void AddDamageDealt(int damageDealt = 1)
    {
        gameData.statData.damageDealt += damageDealt;
    }

    public int GetDamageDealt()
    {
        return gameData.statData.damageDealt;
    }

    public void AddDamageTaken(int damageTaken = 1)
    {
        gameData.statData.damageTaken += damageTaken;
    }

    public int GetDamageTaken()
    {
        return gameData.statData.damageTaken;
    }

    public int GetCoinsCollected()
    {
        return gameData.statData.coinsCollected;
    }

    public int GetItemsCollected()
    {
        return gameData.statData.itemsCollected;
    }

    // worldData

    public void SetCurrentScene(int currentScene)
    {
        gameData.worldData.currentScene = currentScene;
    }

    public int GetCurrentScene()
    {
        return gameData.worldData.currentScene;
    }

    public void SetPlayerPosition(Vector2 playerPosition)
    {
        gameData.worldData.playerPosition = playerPosition;
    }

    public Vector2 GetPlayerPosition()
    {
        return gameData.worldData.playerPosition;
    }

    // settingsData

    public void SetSettings(SettingsData settingsData)
    {
        gameData.settingsData = settingsData;
    }
}