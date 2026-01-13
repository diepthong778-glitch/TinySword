using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Info")]
    public string levelId;

    int enemiesKilled = 0;
    int coinsGained = 0;

    public void RegisterEnemyKill()
    {
        enemiesKilled++;
    }

    public void RegisterCoins(int amount)
    {
        coinsGained += amount;
    }

    public void CompleteLevel()
    {
        LevelResult result = new LevelResult
        {
            levelId = levelId,
            enemiesKilled = enemiesKilled,
            coinsGained = coinsGained
        };

        // Pass data to next scene
        LevelResultContext.LastResult = result;

        // Save to disk
        SaveManager.Instance.ApplyLevelResult(result);

        // Load LevelComplete scene
        SceneManager.LoadScene("LevelComplete");
    }
}
