using TMPro;
using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text coinText;
    public TMP_Text enemyText;

    void Start()
    {
        LevelResult result = LevelResultContext.LastResult;

        if (result == null)
        {
            Debug.LogError("LevelResult missing!");
            return;
        }

        levelText.text = $"LEVEL {result.levelId}";
        coinText.text = result.coinsGained.ToString();
        enemyText.text = result.enemiesKilled.ToString();
    }
}
