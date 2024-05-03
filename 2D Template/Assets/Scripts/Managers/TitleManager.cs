using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {
    private GameData gameData;
    public Button newGameButton, loadGameButton, deleteDataButton, quitButton;
    private void Awake() {
        newGameButton.onClick.AddListener(NewGame);
        loadGameButton.onClick.AddListener(LoadGame);
        deleteDataButton.onClick.AddListener(DeleteData);
        quitButton.onClick.AddListener(QuitGame);

        CheckForSavedData();
    }

    private void NewGame() {
        gameData = new GameData().NewGame();
        gameData.worldData.currentScene = 1;
        StartGame();
    }

    private void LoadGame() {
        if (gameData != null) {
            StartGame();
        } else {
            Debug.LogError("No save data found");
        }
    }

    private void StartGame() {
        GameDataManager.Instance.SetGameData(gameData);
        SceneManager.LoadScene(gameData.worldData.currentScene);
    }

    private void DeleteData() {
        SaveSystem.DeleteSaveData();
        CheckForSavedData();
    }

    private void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void CheckForSavedData() {
        gameData = SaveSystem.LoadGameData();
        if (gameData == null) {
            loadGameButton.interactable = false;
            deleteDataButton.interactable = false;
        } else {
            loadGameButton.interactable = true;
            deleteDataButton.interactable = true;
        }
    }
}