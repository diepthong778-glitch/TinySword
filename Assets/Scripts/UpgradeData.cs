using System;

[Serializable]
public class UpgradeData
{
    public string upgradeId;
    public int level;

    public UpgradeData(string id, int lvl)
    {
        upgradeId = id;
        level = lvl;
    }
}