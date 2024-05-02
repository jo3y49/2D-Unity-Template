using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : GameManager {
    public void SetHealth(int health)
    {
        gameData.playerData.health = health;
    }

    public void SetLives(int lives)
    {
        gameData.playerData.lives = lives;
    }

    public void SetCoins(int coins)
    {
        gameData.playerData.coins = coins;
    }

    public void SetLevel(int level)
    {
        gameData.playerData.level = level;
    }

    public void SetXP(int xp)
    {
        gameData.playerData.xp = xp;
    }

    public void SetScore(int score)
    {
        gameData.playerData.score = score;
    }

    public void SetInventory(IDictionary<Item, int> inventory)
    {
        gameData.playerData.inventory = inventory;
    }

    public void SetInventoryCap(int inventoryCap)
    {
        gameData.playerData.inventoryCap = inventoryCap;
    }

    public void SetPlaytime(float playtime)
    {
        gameData.statData.playtime = playtime;
    }

    public void SetDeaths(int deaths)
    {
        gameData.statData.deaths = deaths;
    }

    public void SetKills(int kills)
    {
        gameData.statData.kills = kills;
    }

    public void SetDamageDealt(int damageDealt)
    {
        gameData.statData.damageDealt = damageDealt;
    }

    public void SetDamageTaken(int damageTaken)
    {
        gameData.statData.damageTaken = damageTaken;
    }

    public void SetCoinsCollected(int coinsCollected)
    {
        gameData.statData.coinsCollected = coinsCollected;
    }

    public void SetItemsCollected(int itemsCollected)
    {
        gameData.statData.itemsCollected = itemsCollected;
    }

    public void SetCurrentScene(int currentScene)
    {
        gameData.worldData.currentScene = currentScene;
    }

    public void SetPlayerPosition(Vector2 playerPosition)
    {
        gameData.worldData.playerPosition = playerPosition;
    }

    public void SetSettings(SettingsData settingsData)
    {
        gameData.settingsData = settingsData;
    }

    public int GetHealth()
    {
        return gameData.playerData.health;
    }

    public int GetLives()
    {
        return gameData.playerData.lives;
    }

    public int GetCoins()
    {
        return gameData.playerData.coins;
    }

    public int GetLevel()
    {
        return gameData.playerData.level;
    }

    public int GetXP()
    {
        return gameData.playerData.xp;
    }

    public int GetScore()
    {
        return gameData.playerData.score;
    }   

    public IDictionary<Item, int> GetInventory()
    {
        return gameData.playerData.inventory;
    }

    public int GetInventoryCap()
    {
        return gameData.playerData.inventoryCap;
    }

    public float GetPlaytime()
    {
        return gameData.statData.playtime;
    }

    public int GetDeaths()
    {
        return gameData.statData.deaths;
    }

    public int GetKills()
    {
        return gameData.statData.kills;
    }

    public int GetDamageDealt()
    {
        return gameData.statData.damageDealt;
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

    public int GetCurrentScene()
    {
        return gameData.worldData.currentScene;
    }

    public Vector2 GetPlayerPosition()
    {
        return gameData.worldData.playerPosition;
    }
}