using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    public Button resumeButton, saveButton, quitButton;

    private void Awake() {
        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(Save);
        quitButton.onClick.AddListener(Quit);
    }
    private void Start() {
        PauseManager.PauseEvent += TogglePauseMenu;
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        PauseManager.PauseEvent -= TogglePauseMenu;
    }

    private void TogglePauseMenu(bool paused) {
        gameObject.SetActive(!paused);
    }

    public void Resume() {
        PauseManager.Instance.TogglePause();
    }

    public void Save() {
        GameManager.Instance.SaveGameData();
    }

    public void Quit() {
        GameManager.Instance.QuitGame();
    }
}