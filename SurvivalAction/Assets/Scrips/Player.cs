using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameManeger.PauseMenu();
        }
    }
    void MovePlayer()
    {
        Vector2 playerInput=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        rb.linearVelocity=playerInput.normalized*moveSpeed;
        if(playerInput.x<0)
        {
            spriteRenderer.flipX=true;
        }
        else if(playerInput.x>0)
        {
            spriteRenderer.flipX=false;
        }
        if(playerInput!=Vector2.zero)
        {
            animator.SetBool("isRun",true);
        }
        else
        {
            animator.SetBool("isRun",false);
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
    public void Die()
    {
        gameManeger.GameOverMenu();
    }
}
