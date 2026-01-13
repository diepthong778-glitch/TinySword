using UnityEngine;

public class EntityStat : MonoBehaviour
{
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

    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHP = maxHP;
        healthbar.setMaxHealth(maxHP);
    }

    private void Update()
    {
        healthbar.setMaxHealth(maxHP);
        healthbar.setHealth(currentHP);
    }
    public void TakeDamage(int rawDamage)
    {
        if (isDead) return;

        healthbar.Shake();
        int finalDamage = Mathf.Max(rawDamage - defense, 1);
        currentHP -= finalDamage;

        if (currentHP <= 0)
        { 
            anim.SetTrigger("die");
            Die();
        }              
    }
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
    void Die()
    {
        isDead = true;
        healthbar.Shake();
        GetComponent<Lootable>()?.DropLoot();
        FindObjectOfType<LevelManager>()?.RegisterEnemyKill();
        Object.Destroy(gameObject, 1f);
    }
}
