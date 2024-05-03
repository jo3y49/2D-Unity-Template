using System;
using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public static PauseManager Instance { get; private set; }

    public static event Action<bool> PauseEvent;
    public InputActions actions;

    private void Awake() {
        Instance = this;
        actions = new InputActions();
    }

    private void Start() {
        Time.timeScale = 1;

        StartCoroutine(CountPlaytime());
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

    private IEnumerator CountPlaytime()
    {
        float previousTime = Time.time;

        while (true)
        {
            if (Time.timeScale > 0)
            {
                float deltaTime = Time.time - previousTime;
                GameDataManager.Instance.AddPlaytime(deltaTime);
            }

            previousTime = Time.time;

            yield return null;
        }
    }
}