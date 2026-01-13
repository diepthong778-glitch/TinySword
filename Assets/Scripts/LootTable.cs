using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot/Loot Table")]
public class LootTable : ScriptableObject
{
    public List<LootItem> lootItems = new();

    public void DropLoot(Vector2 position)
    {
        foreach (var item in lootItems)
        {
            if (Random.value > item.dropChance)
                continue;

            int amount = Random.Range(item.minAmount, item.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                Instantiate(
                    item.prefab,
                    position + Random.insideUnitCircle * 0.3f,
                    Quaternion.identity
                );
            }
        }
    }
}
