using UnityEngine;

public class Lootable : MonoBehaviour
{
    public LootTable lootTable;

    public void DropLoot()
    {
        if (lootTable == null)
            return;

        lootTable.DropLoot(transform.position);
    }
}
