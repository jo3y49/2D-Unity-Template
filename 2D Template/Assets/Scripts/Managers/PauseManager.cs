using System;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public static PauseManager Instance { get; private set; }

    public static event Action<bool> PauseEvent;
    public InputActions actions;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            actions = new InputActions();
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        actions.Menu.Enable();
        actions.Menu.Pause.performed += context => TogglePause();
    }

    private void OnDisable() {
        actions.Menu.Pause.performed -= context => TogglePause();
        actions.Menu.Disable();
    }

    private void TogglePause() {
        bool resume = Time.timeScale == 0;

        if (resume)
        {
            Time.timeScale = 1;
            PauseEvent?.Invoke(false);
        }
        else
        {
            Time.timeScale = 0;
            PauseEvent?.Invoke(true);
        }
    }
}