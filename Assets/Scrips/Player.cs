using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private float speedMultiplier = 1f;
    private bool useUnscaledMovement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameManeger gameManeger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (hpBar == null)
        {
            hpBar = GetComponentInChildren<Image>();
        }

        if (gameManeger == null)
        {
            gameManeger = FindFirstObjectByType<GameManeger>();
        }
    }
    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar(); // Cập nhật thanh máu khi bắt đầu
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManeger.PauseMenu();
        }
    }
    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float finalSpeed = moveSpeed * speedMultiplier;
        if (useUnscaledMovement)
        {
            transform.position += (Vector3)(playerInput.normalized * finalSpeed * Time.unscaledDeltaTime);
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = playerInput.normalized * finalSpeed;
        }
        if (playerInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (playerInput != Vector2.zero)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }
    public void TakeDame(float damage)
    {
        currentHp -= damage; // Giảm máu khi bị tấn công
        currentHp = Mathf.Max(currentHp, 0); // Đảm bảo máu không âm
        UpdateHpBar(); // Cập nhật thanh máu sau khi bị tấn công
        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue; // Tăng máu khi được hồi phục
            currentHp = Mathf.Min(currentHp, maxHp); // Đảm bảo máu không vượt quá max
            UpdateHpBar(); // Cập nhật thanh máu sau khi được hồi phục
        }
    }
    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp; // Cập nhật thanh máu
        }
    }
    public void SetMoveSpeed(float value)
    {
        moveSpeed = Mathf.Max(0.1f, value);
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = Mathf.Max(0.1f, value);
    }
    public void SetAnimatorUseUnscaledTime(bool value)
    {
        if (animator != null)
        {
            animator.updateMode = value ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        }
    }

    public void SetUseUnscaledMovement(bool value)
    {
        useUnscaledMovement = value;
    }

    public void SetMaxHp(float value)
    {
        maxHp = Mathf.Max(1f, value);
        currentHp = maxHp;
        UpdateHpBar();
    }
    public void Die()
    {
        gameManeger.GameOverMenu();
    }
}
