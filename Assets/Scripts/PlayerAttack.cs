using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange;
    public float attackAngle = 60f;

    [Header("Delay Phase")]
    public float delayOnAttack = 0.25f;

    [Header("Failsafe")]
    public float maxAttackLockTime = 1.2f;

    private Animator anim;

    private bool isAttacking;
    private bool canQueueCombo;
    private bool comboQueued;
    private int comboStep;
    public PlayerMovement playerMovement;

    private float attackTimer;
    private EntityStat entityStat;

    public AudioClip swordSwingSound;
    void Awake()
    {
        anim = GetComponent<Animator>();
        entityStat = GetComponent<EntityStat>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        attackRange = entityStat.attackRange;
    }
    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= maxAttackLockTime)
            {
                ForceResetCombat();
            }
        }

        if (Input.GetMouseButtonDown(0))
            RegisterAttackInput();
    }

    void RegisterAttackInput()
    {
        if (!isAttacking && !comboQueued)
        {
            StartAttack1();
            return;
        }

        if (canQueueCombo)
        {
            comboQueued = true;
            anim.SetBool("comboQueued", true);
        }
    }

    void StartAttack1()
    {
        isAttacking = true;
        comboStep = 1;
        comboQueued = false;
        canQueueCombo = false;
        attackTimer = 0f;

        anim.SetTrigger("attack");
        anim.SetInteger("comboStep", 1);
        anim.SetBool("isAttacking", true);
        anim.SetBool("comboQueued", false);
        playerMovement.SetMoveMultiplier(0.2f);
        GlobalSound.Instance?.PlaySound(swordSwingSound);
    }

    void StartAttack2()
    {
        comboStep = 2;
        comboQueued = false;
        canQueueCombo = false;
        attackTimer = 0f;

        anim.SetTrigger("attack");
        anim.SetInteger("comboStep", 2);
        anim.SetBool("comboQueued", false);
        playerMovement.SetMoveMultiplier(0.2f);
        GlobalSound.Instance?.PlaySound(swordSwingSound);
    }

    public void EnableComboQueue()
    {
        canQueueCombo = true;
    }

    public void AttackHit_1()
    {
        DoConeAttack();
    }

    public void AttackHit_2()
    {
        DoConeAttack();
    }

    public void AttackEnd_1()
    {
        if (comboQueued)
        {
            StartAttack2();
        }
        else
        {
            playerMovement.SetMoveMultiplier(1f);
            ResetCombat();
        }
    }

    public void AttackEnd_2()
    {
        playerMovement.SetMoveMultiplier(1f);
        ResetCombat();
    }

    void ResetCombat()
    {
        isAttacking = false;
        comboQueued = false;
        canQueueCombo = false;
        comboStep = 0;
        anim.SetBool("attack", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("comboQueued", false);
        anim.SetInteger("comboStep", 0);
    }

    void ForceResetCombat()
    {
        Debug.LogWarning("Attack force-reset (failsafe)");
        ResetCombat();
    }

    void DoConeAttack()
    {
        Vector2 dir = GetMouseDirection();

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            LivingEntity target = hit.GetComponent<LivingEntity>();

            if (target == null) continue;

            if (target.isFriendly) continue;

            Vector2 toTarget =
                ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;

            float angle = Vector2.Angle(dir, toTarget);

            if (angle <= attackAngle * 0.5f)
            {
                target.Hurt(entityStat.attack, toTarget, entityStat.knockbackStrength);
            }
        }
    }

    Vector2 GetMouseDirection()
    {
        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return (mouse - transform.position).normalized;
    }
}
