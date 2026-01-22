using UnityEngine;
using System;

public class EntityStat : MonoBehaviour
{
    public static event Action<EntityStat> OnEntityDied;

    [Header("Health")]
    public int maxHP = 100;
    public int currentHP;
    public Healthbar healthbar;

    [Header("Combat")]
    public int attack = 10;
    public float attackRange = 1.5f;
    public int defense = 2;
    public float knockbackStrength = 1f;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("State")]
    public bool isDead;

    [Header("Identity")]
    public bool isPlayer; // CHECK THIS ON PLAYER ONLY

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHP = maxHP;

        if (healthbar != null)
        {
            healthbar.setMaxHealth(maxHP);
            healthbar.setHealth(currentHP);
        }
    }

    public void TakeDamage(int rawDamage)
    {
        if (isDead) return;

        healthbar?.Shake();

        int finalDamage = Mathf.Max(rawDamage - defense, 1);
        currentHP -= finalDamage;

        if (healthbar != null)
            healthbar.setHealth(currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHP = Mathf.Min(currentHP + amount, maxHP);
        healthbar?.setHealth(currentHP);
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        anim?.SetTrigger("die");

        // Notify listeners (UI, managers)
        OnEntityDied?.Invoke(this);

        // Enemy-only logic
        if (!isPlayer)
        {
            GetComponent<Lootable>()?.DropLoot();
            FindObjectOfType<LevelManager>()?.RegisterEnemyKill();
        }

        Destroy(gameObject, 1f);
    }
}
