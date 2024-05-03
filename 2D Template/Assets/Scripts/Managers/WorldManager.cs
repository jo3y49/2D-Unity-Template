using UnityEngine;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance { get; protected set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        GameDataManager gd = GameDataManager.Instance;
        
        PlayerMovement.Instance.transform.position = gd.GetPlayerPosition();
    }
}