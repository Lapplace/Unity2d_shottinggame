using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected Player player;
    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDame = 10f;
    [SerializeField] protected float stayDame = 1f;
    [SerializeField] protected float contactDamageInterval = 0.5f;
    [SerializeField] private int expReward = 1;
    private float nextContactDamageTime;
    // Biến để lưu tốc độ gốc của quái vật, dùng để phục hồi sau khi bị ảnh hưởng bởi knockback
    private float originalSpeed;
    private Coroutine knockbackSlowCoroutine;

    private PlayerProgression progression;
    //virtual các con ngoài su dung con co the viet them 
    protected virtual void Start()
    {
        originalSpeed = enemyMoveSpeed; // Lưu lại tốc độ gốc để có thể phục hồi sau khi bị ảnh hưởng bởi knockback
        player = FindAnyObjectByType<Player>();
        progression = FindFirstObjectByType<PlayerProgression>();
        currentHp = maxHp;
        UpdateHpBar(); // Cập nhật thanh máu khi bắt đầu
    }
    protected virtual void Update()
    {
        MoveToPlayer();
    }
    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);
            FlipEnemy();
        }
    }
    protected void FlipEnemy()
    {
        if (player != null)
        {
            if (enemyMoveSpeed < 0)
            {
                return; // Nếu tốc độ di chuyển đã là âm, không cần lật
            }
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }
    public virtual void TakeDame(float damage)
    {
        currentHp -= damage; // Giảm máu khi bị tấn công
        currentHp = Mathf.Max(currentHp, 0); // Đảm bảo máu không âm
        UpdateHpBar(); // Cập nhật thanh máu sau khi bị tấn công
        if (currentHp <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        if (progression != null)
        {
            progression.AddExp(expReward);
        }
        Destroy(gameObject);
    }
    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp; // Cập nhật thanh máu
        }
    }
    // giãn cách gây damage khi tiếp xúc với player
    protected void ResetContactDamageTimer()
    {
        nextContactDamageTime = 0f;
    }

    protected bool CanDealContactDamage()
    {
        if (Time.time < nextContactDamageTime)
        {
            return false;
        }

        nextContactDamageTime = Time.time + Mathf.Max(0.05f, contactDamageInterval); // Đảm bảo khoảng thời gian tối thiểu và giá trị không bị âm
        return true;
    }
    public void ApplyNegativeSpeed(float duration)
    {
        
        if (knockbackSlowCoroutine != null)
        {
            StopCoroutine(knockbackSlowCoroutine); // Làm mới lại thời gian áp dụng nếu trúng đòn liên tục
        }
        knockbackSlowCoroutine = StartCoroutine(NegativeSpeedRoutine(duration));
    }

    private IEnumerator NegativeSpeedRoutine(float duration)
    {
        // Biến tốc độ thành âm (Ví dụ tốc độ gốc là 3 -> thành -3 để đi lùi)
        // Bạn có thể nhân thêm hệ số nếu muốn quái bị đẩy lùi mạnh hơn (ví dụ: -originalSpeed * 1.5f)
        enemyMoveSpeed = -originalSpeed;

        yield return new WaitForSeconds(duration);

        // Trả lại tốc độ di chuyển bình thường
        enemyMoveSpeed = originalSpeed;
        knockbackSlowCoroutine = null;
    }
}
