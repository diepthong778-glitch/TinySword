using System;

[Serializable]
public class ItemStack
{
    public string itemId;
    public int amount;

    public ItemStack(string id, int amt)
    {
        itemId = id;
        amount = amt;
    }
}