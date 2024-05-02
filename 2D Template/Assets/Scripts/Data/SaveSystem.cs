using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/saveData.json";
    
    #pragma warning disable 0414
    private static readonly string webGLKey = "GameData";
    #pragma warning restore 0414
    

    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);

    public static void SaveGameData(GameData gameData)
    {
        string gameDataString = JsonUtility.ToJson(gameData);

        #if UNITY_WEBGL && !UNITY_EDITOR
            SaveToLocalStorage(webGLKey, gameDataString);
        #else
            File.WriteAllText(path, gameDataString);
        #endif
    }

    public static GameData LoadGameData()
    {
        string gameDataString;

        #if UNITY_WEBGL && !UNITY_EDITOR
            gameDataString = LoadFromLocalStorage(webGLKey);
        #else
            if (File.Exists(path))
            {
                gameDataString = File.ReadAllText(path);
            }
            else
            {
                Debug.Log("Save file not found in " + path);
                return null;
            }
        #endif

        return JsonUtility.FromJson<GameData>(gameDataString);
    }

    public static void DeleteSaveData()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            RemoveFromLocalStorage(webGLKey);
        #else
            if(File.Exists(path))
                File.Delete(path);
        #endif
            
    }
}