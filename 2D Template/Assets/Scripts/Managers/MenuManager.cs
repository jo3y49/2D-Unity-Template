using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance { get; private set; }
    private InputActions actions;

    public GameObject pauseMenu, inventoryMenu;
    private List<GameObject> menus;

    private void Awake() {
        Instance = this;
        actions = new InputActions();

        menus = new List<GameObject> { pauseMenu, inventoryMenu };
    }

    private void Start() {
        Time.timeScale = 1;

        foreach (GameObject m in menus)
        {
            m.SetActive(false);
        }

        StartCoroutine(CountPlaytime());
    }

    private void OnEnable() {
        actions.Menu.Enable();
        actions.Menu.Pause.performed += context => ToggleMenu(pauseMenu);
        actions.Menu.Inventory.performed += context => ToggleMenu(inventoryMenu);
    }

    private void OnDisable() {
        actions.Menu.Pause.performed -= context => ToggleMenu(pauseMenu);
        actions.Menu.Inventory.performed -= context => ToggleMenu(inventoryMenu);
        actions.Menu.Disable();
    }
    public void ToggleMenu(GameObject menu)
    {
        bool active = menu.activeSelf;
        foreach (GameObject m in menus)
        {
            if (m != menu)
            {
                m.SetActive(false);
            }
        }

        Time.timeScale = active ? 1 : 0;
        PlayerMovement.Instance.ToggleActive(active);
        menu.SetActive(!active);
    }

    private IEnumerator CountPlaytime()
    {
        float previousTime = Time.time;

        while (true)
        {
            if (pauseMenu.activeSelf == false)
            {
                float deltaTime = Time.time - previousTime;
                GameDataManager.Instance.AddPlaytime(deltaTime);
            }

            previousTime = Time.time;

            yield return null;
        }
    }
}