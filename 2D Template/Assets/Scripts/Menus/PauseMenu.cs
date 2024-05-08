using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public Button resumeButton, saveButton, quitButton;

    private void Awake() {
        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(Save);
        quitButton.onClick.AddListener(Quit);
    }

    public void Resume() {
        MenuManager.Instance.ToggleMenu(gameObject);
    }

    public void Save() {
        GameDataManager gd = GameDataManager.Instance;
        gd.SetCurrentScene(SceneManager.GetActiveScene().buildIndex);
        gd.SetPlayerPosition(PlayerMovement.Instance.transform.position);

        gd.SaveGameData();
    }

    public void Quit() {
        GameDataManager.Instance.QuitGame();
    }
}