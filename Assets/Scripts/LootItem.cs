using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject prefab;   // Coin / item prefab
    [Range(0f, 1f)]
    public float dropChance = 1f;

    public int minAmount = 1;
    public int maxAmount = 1;
}
