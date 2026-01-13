using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveData Data { get; private set; }
    string Path => Application.persistentDataPath + "/save.json";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void ApplyLevelResult(LevelResult result)
    {
        if (result == null)
        {
            Debug.LogError("LevelResult is NULL");
            return;
        }

        if (Data == null)
        {
            Debug.LogError("Save Data is NULL");
            return;
        }

        Data.player.coins += result.coinsGained;

        if (!Data.levelsCompleted.Contains(result.levelId))
            Data.levelsCompleted.Add(result.levelId);

        Save();
    }


    public void NewGame()
    {
        Data = new SaveData();
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(Path, json);
    }

    public void Load()
    {
        if (File.Exists(Path))
        {
            string json = File.ReadAllText(Path);
            Data = JsonUtility.FromJson<SaveData>(json);
        }

        if (Data == null)
        {
            Debug.Log("No save found, creating new save");
            Data = new SaveData();
            Save();
        }
    }

}
