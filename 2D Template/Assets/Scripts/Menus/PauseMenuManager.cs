using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    public Button resumeButton, inventoryButton, saveButton, quitButton;
    public GameObject inventoryMenu;

    private void Awake() {
        resumeButton.onClick.AddListener(Resume);
        inventoryButton.onClick.AddListener(Inventory);
        saveButton.onClick.AddListener(Save);
        quitButton.onClick.AddListener(Quit);
    }
    private void Start() {
        PauseManager.PauseEvent += TogglePauseMenu;

        gameObject.SetActive(false);
    }

    private void OnEnable() {
        inventoryMenu.SetActive(false);
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

    public void Inventory() {
       inventoryMenu.SetActive(true);
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