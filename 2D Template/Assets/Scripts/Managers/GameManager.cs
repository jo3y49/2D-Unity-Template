using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour {
    protected GameData gameData;
    
    public virtual void SetGameData(GameData gameData)
    {
        this.gameData = gameData;
    }

    public virtual void SaveGameData()
    {
        SaveSystem.SaveGameData(gameData);
    }

    public virtual void DeleteSaveData()
    {
        // add a prompt to confirm deletion later
        SaveSystem.DeleteSaveData();
    }

    public virtual void QuitGame()
    {
        // add a prompt to save first later
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}