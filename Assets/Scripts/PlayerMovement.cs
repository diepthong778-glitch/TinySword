using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float moveMultiplier = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Camera cam;
    private EntityStat stats;

    bool onStairs = false;
    float stairSlope;
    bool stairRightIsUp;



    private Vector2 input;

    public AudioClip footstepSound;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        stats = GetComponent<EntityStat>();
        moveSpeed = stats.moveSpeed;
    }

    void Update()
    {
        ReadInput();
        UpdateAnimation();
        HandleFacing();
        PlayFootStepSound();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ReadInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void Move()
    {
        Vector2 moveDir = input;

        if (onStairs)
        {
            float stairDir = stairRightIsUp ? 1f : -1f;

            moveDir.y += input.x * stairSlope * stairDir;

            moveDir.y += input.y * 0.5f;

            moveDir = moveDir.normalized;
        }

        rb.MovePosition(
            rb.position + moveDir * moveSpeed * moveMultiplier * Time.fixedDeltaTime
        );
    }


    void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking());
    }

    bool isWalking()
    {
        return input.sqrMagnitude > 0.01f;
    }

    void HandleFacing()
    {

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        float deltaX = mouseWorld.x - transform.position.x;

        if (Mathf.Abs(deltaX) < 0.01f)
            return;

        sr.flipX = deltaX < 0f;
    }

    public void SetMoveMultiplier(float value)
    {
        moveMultiplier = Mathf.Clamp(value, 0f, 1f);
    }

    void PlayFootStepSound()
    {
        if(isWalking())
            GlobalSound.Instance?.PlaySound(footstepSound);
        else
            GlobalSound.Instance?.StopLoop(footstepSound);
    }

    public void EnterStairs(float slope, bool rightIsUp)
    {
        onStairs = true;
        stairSlope = slope;
        stairRightIsUp = rightIsUp;
    }

    public void ExitStairs()
    {
        onStairs = false;
    }
}
