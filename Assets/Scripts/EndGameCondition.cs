using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameZone : MonoBehaviour
{
    [Header("Message")]
    [SerializeField] string notClearedMessage = "Defeat all enemies first!";

    private bool triggered;
    public AudioClip levelCompletedSound;
    public GameObject blockable;
    public int Level;

    private void Update()
    {
        if (blockable != null && EnemyRegistry.AliveEnemies <= 0)
        {
            Destroy(blockable);
            blockable = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (!other.CompareTag("Player"))
            return;

        if (EnemyRegistry.AliveEnemies <= 0)
        {
            triggered = true;
            TriggerEndGame();
        }
        else
        {
            ShowMessage();
        }
    }

    void TriggerEndGame()
    {
        Object.FindFirstObjectByType<LevelManager>()?.CompleteLevel();
        SceneManager.LoadScene("LevelComplete");
        GlobalSound.Instance?.PlaySound(levelCompletedSound);
    }

    void ShowMessage()
    {
        Debug.Log(notClearedMessage);

        // Example:
        // FloatingText.Show(notClearedMessage);
        // ToastUI.Show(notClearedMessage);
    }
}
