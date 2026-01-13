using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    [Header("Ranges")]
    public float detectRange = 6f;
    public float attackRange = 1.5f;
    public float leashRange = 8f;

    [Header("Combat")]
    public float attackCooldown = 1.2f;

    [Header("Obstacle")]
    public LayerMask obstacleMask;

    private Vector2 spawnPoint;
    private Rigidbody2D rb;
    private EntityStat stat;
    private LivingEntity livingEntity;
    private Animator anim;
    private SpriteRenderer sr;
    private bool lockAnimations = false;

    private float lastAttackTime;

    [Header("Pathfinder")]
    List<Waypoint> path = new List<Waypoint>();
    int pathIndex = 0;
    public float waypointReachDistance = 0.25f;

    private enum State { Idle, Chase, Return }
    private State state;

    void Awake()
    {
        livingEntity = GetComponent<LivingEntity>();
        stat = GetComponent<EntityStat>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        spawnPoint = transform.position;
    }

    void Start()
    {
        attackRange = stat.attackRange;
    }

    void Update()
    {
        if (stat.isDead) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        float distToSpawn = Vector2.Distance(transform.position, spawnPoint);

        switch (state)
        {
            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("isWalking", false);

                if (distToPlayer <= detectRange && HasLineOfSight())
                    state = State.Chase;
                break;

            case State.Chase:
                if (distToSpawn > leashRange)
                {
                    state = State.Return;
                    break;
                }

                if (distToPlayer <= attackRange)
                {
                    rb.linearVelocity = Vector2.zero;
                    anim.SetBool("isWalking", false);
                    TryAttack();
                }
                else
                {
                    MoveTowards(player.position);
                }
                break;

            case State.Return:
                {
                    if (path.Count == 0)
                        RequestPath(spawnPoint);

                    FollowPath();
                    break;
                }
        }
    }

    void MoveTowards(Vector2 target)
    {
        if (lockAnimations) return;
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        rb.linearVelocity = dir * stat.moveSpeed;

        anim.SetBool("isWalking", true);

        if (dir.x != 0)
            sr.flipX = dir.x < 0;
    }

    void TryAttack()
    { 
        if (Time.time < lastAttackTime + attackCooldown)
            return;
        lockAnimations = true;
        lastAttackTime = Time.time;

        anim.SetTrigger("attack");
    }

    public void DealDamage()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<LivingEntity>()
                .Hurt(stat.attack, (player.position - transform.position).normalized, stat.knockbackStrength);
        }
    }

    public void FinishAttack()
    {
        lockAnimations = false;
    }

    bool HasLineOfSight()
    {
        Vector2 dir = player.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir.normalized,
            detectRange,
            obstacleMask
        );

        return hit.collider == null;
    }

    Waypoint FindClosestWaypoint(Vector2 pos)
    {
        Waypoint[] all = FindObjectsOfType<Waypoint>();
        Waypoint best = null;
        float bestDist = Mathf.Infinity;

        foreach (var wp in all)
        {
            float d = Vector2.Distance(pos, wp.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = wp;
            }
        }
        return best;
    }

    void RequestPath(Vector2 targetPos)
    {
        Waypoint start = FindClosestWaypoint(transform.position);
        Waypoint goal = FindClosestWaypoint(targetPos);

        if (start == null || goal == null)
            return;

        path = PathFinder.FindPath(start, goal);
        pathIndex = 0;
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0)
            return;
        if (pathIndex >= path.Count)
        {
            path.Clear();
            pathIndex = 0;

            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);

            state = State.Idle;
            return;
        }

        Vector2 target = path[pathIndex].transform.position;

        if (Vector2.Distance(transform.position, target) < waypointReachDistance)
        {
            pathIndex++;
            return;
        }

        MoveTowards(target);
    }

    void OnEnable()
    {
        EnemyRegistry.Register();
    }

    void OnDisable()
    {
        EnemyRegistry.Unregister();
    }

    void OnDestroy()
    {
        EnemyRegistry.Unregister();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint, leashRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
