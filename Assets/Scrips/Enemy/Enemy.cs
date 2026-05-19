using UnityEngine;
using UnityEngine.UI;
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
    private PlayerProgression progression;
    //virtual các con ngoài su dung con co the viet them 
    protected virtual void Start()
    {
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

        nextContactDamageTime = Time.time + Mathf.Max(0.05f, contactDamageInterval);
        return true;
    }
    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp; // Cập nhật thanh máu
        }
    }
}
