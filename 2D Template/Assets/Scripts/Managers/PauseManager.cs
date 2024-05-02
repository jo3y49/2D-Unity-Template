using System;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public static PauseManager Instance { get; private set; }

    public static event Action<bool> PauseEvent;
    public InputActions actions;

    private void Awake() {
        Instance = this;
        actions = new InputActions();
    }

    private void OnEnable() {
        actions.Menu.Enable();
        actions.Menu.Pause.performed += context => TogglePause();
    }

    private void OnDisable() {
        actions.Menu.Pause.performed -= context => TogglePause();
        actions.Menu.Disable();
    }

    public void TogglePause() {
        bool paused = Time.timeScale == 0;
        
        Time.timeScale = paused ? 1 : 0;
        PauseEvent?.Invoke(paused);
        PlayerMovement.Instance.TogglePause(!paused);
    }
}