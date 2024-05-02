using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int lives;
    public int health;
    public int coins;
    public int level;
    public int xp;
    public int score;
    public IDictionary<Item, int> inventory = new Dictionary<Item, int>();
    public int inventoryCap;
    
}
[System.Serializable]
public class StatData
{
    public float playtime;
    public int deaths;
    public int kills;
    public int damageDealt;
    public int damageTaken;
    public int coinsCollected;
    public int itemsCollected;
}
[System.Serializable]
public class WorldData
{
    public int currentScene = 1;
    public Vector2 playerPosition = new();
}

[System.Serializable]
public class SettingsData
{
    
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData = new();
    public StatData statData = new();
    public WorldData worldData = new();
    public SettingsData settingsData = new();

    public GameData NewGame()
    {
        playerData = new();
        statData = new();
        worldData = new();

        return this;
    }
}