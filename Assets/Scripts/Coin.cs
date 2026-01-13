using UnityEngine;

public class CoinFlyToPlayer : MonoBehaviour
{
    public float flySpeed = 8f;
    public float collectDistance = 0.2f;

    private Transform player;
    private bool collected = false;

    public AudioClip coinPicked;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null || collected)
            return;

        // Move straight toward player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            flySpeed * Time.deltaTime
        );

        // Safety auto-collect (in case trigger misses)
        if (Vector2.Distance(transform.position, player.position) <= collectDistance)
        {
            SaveManager.Instance?.Data.player.AddCoins(1);
            Object.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.Instance.Data.player.AddCoins(1);
            FindObjectOfType<LevelManager>()?.RegisterCoins(1);
            GlobalSound.Instance?.PlaySound(coinPicked);
            Object.Destroy(gameObject);
        }
    }
}
