using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public PlayerData player;
    public ProgressData progress;
    public List<string> levelsCompleted;

    public SaveData()
    {
        player = new PlayerData();
        progress = new ProgressData();
        levelsCompleted = new List<string>();
    }
}

