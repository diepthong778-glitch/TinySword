using TMPro;
using UnityEngine;
using System.Collections;

public class CoinUI : MonoBehaviour
{
    private TMP_Text coinText;

    void Awake()
    {
        coinText = GetComponent<TMP_Text>();
        if (SaveManager.Instance == null)
        {
            coinText.text = "Missing SaveManager";
        }
    }

    IEnumerator Start()
    {
        // Wait until SaveManager exists
        while (SaveManager.Instance == null)
            yield return null;

        // Subscribe
        SaveManager.Instance.Data.player.OnCoinsChanged += UpdateUI;

        // Initial update
        UpdateUI(SaveManager.Instance.Data.player.coins);
    }

    void UpdateUI(int coins)
    {
        coinText.text = coins.ToString();
    }

    void OnDestroy()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.Data.player.OnCoinsChanged -= UpdateUI;
    }
}
