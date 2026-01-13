using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int coins;

    public event Action<int> OnCoinsChanged;

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
    }

    public void SetCoins(int value)
    {
        coins = value;
        OnCoinsChanged?.Invoke(coins);
    }
}
