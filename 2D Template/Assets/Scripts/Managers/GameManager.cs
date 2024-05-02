using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    protected GameData gameData;
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetGameData(GameData gameData)
    {
        this.gameData = gameData;
    }

    public void SaveGameData()
    {
        SaveSystem.SaveGameData(gameData);
    }

    public void DeleteSaveData()
    {
        // add a prompt to confirm deletion later
        SaveSystem.DeleteSaveData();
    }

    public void QuitGame()
    {
        // add a prompt to save first later
        SceneManager.LoadScene(0);
    }
}