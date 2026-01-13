using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour
{
    [Header("Faction")]
    public bool isEnemy;
    public bool isFriendly;
    public bool isNeutral;
    public bool isPlayer;

    private Rigidbody2D rb;
    private Animator anim;
    private EntityStat stats;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        stats = GetComponent<EntityStat>();
    }

    public void Hurt(int damage)
    {
        stats.TakeDamage(damage);
    }

    public void Hurt(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        stats.TakeDamage(damage);
        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }
}
