using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance { get; private set; }
    private InputActions actions;

    public GameObject pauseMenu, inventoryMenu;
    private List<GameObject> menus;
    public CombatMenuManager combatMenuManager;

    private void Awake() {
        Instance = this;
        actions = new InputActions();

        menus = new List<GameObject> { pauseMenu, inventoryMenu };
    }

    private void Start() {
        Time.timeScale = 1;

        CloseMenus();
        combatMenuManager.gameObject.SetActive(false);

        StartCoroutine(CountPlaytime());
    }

    private void OnEnable() {
        ActivateMenuControls();
    }

    private void OnDisable() {
        DeactivateMenuControls();
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

    private void CloseMenus()
    {
        foreach (GameObject m in menus)
        {
            m.SetActive(false);
        }
    }

    public void StartCombat(List<GameObject> playerObjects, List<GameObject> enemyObjects)
    {
        combatMenuManager.gameObject.SetActive(true);
        CloseMenus();
        DeactivateMenuControls();

        combatMenuManager.Initialize(playerObjects, enemyObjects);
    }

    public void EndCombat()
    {
        combatMenuManager.gameObject.SetActive(false);
        ActivateMenuControls();
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

    private void ActivateMenuControls()
    {
        actions.Menu.Enable();
        actions.Menu.Pause.performed += context => ToggleMenu(pauseMenu);
        actions.Menu.Inventory.performed += context => ToggleMenu(inventoryMenu);
    }

    private void DeactivateMenuControls()
    {
        actions.Menu.Pause.performed -= context => ToggleMenu(pauseMenu);
        actions.Menu.Inventory.performed -= context => ToggleMenu(inventoryMenu);
        actions.Menu.Disable();
    }
}