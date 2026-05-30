using System.Collections;
using UnityEngine;

public class AuraDamageInstance : MonoBehaviour
{
    [SerializeField] private CircleCollider2D auraCollider;
    [SerializeField] private LayerMask targetMask = ~0;
    [SerializeField] private string enemyTag = "Enemy";

    private SpriteRenderer spriteRenderer;
    private Transform owner;
    private float damage;
    private float tick;
    private float duration;
    private float growTime;
    private float shrinkTime;

    private float tickTimer;
    private float auraRange; // Đây là bán kính TỐI ĐA khi hoàn thành Grow

    // Biến lưu trữ bán kính thực tế thay đổi theo thời gian (dùng để quét Dame và Collider)
    private float currentRadius;
    private float maxVisualScale;

    public void Initialize(Transform auraOwner, float auraDamage, float range, float damageTick, float auraDuration, float growDuration, float shrinkDuration)
    {
        owner = auraOwner;
        damage = auraDamage;
        auraRange = Mathf.Max(0.1f, range);
        tick = Mathf.Max(0.05f, damageTick);
        duration = Mathf.Max(0.05f, auraDuration);
        growTime = Mathf.Max(0.01f, growDuration);
        shrinkTime = Mathf.Max(0.01f, shrinkDuration);

        // 1. CỐ ĐỊNH SCALE CỦA OBJECT GỐC LUÔN BẰNG 1
        transform.localScale = Vector3.one;
        currentRadius = 0f;

        // Cấu hình Collider (nếu bạn vẫn cần dùng Trigger cho logic khác, nếu không chỉ cần hàm quét vật lý bên dưới)
        if (auraCollider == null)
        {
            auraCollider = GetComponent<CircleCollider2D>();
        }
        if (auraCollider != null)
        {
            auraCollider.radius = currentRadius;
            auraCollider.isTrigger = true;
        }

        // 2. TÍNH TOÁN SCALE CHO SPRITE ĐỂ VỪA VỚI BÁN KÍNH MONG MUỐN
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float spriteOriginalWidth = spriteRenderer.sprite.bounds.size.x;
            float desiredDiameter = auraRange * 2f; // Đường kính = Bán kính * 2

            // Tỷ lệ scale tối đa của riêng SpriteRenderer
            maxVisualScale = desiredDiameter / spriteOriginalWidth;
            spriteRenderer.transform.localScale = Vector3.zero; // Ban đầu ẩn đi
        }
        else
        {
            maxVisualScale = auraRange * 2f;
        }

        StartCoroutine(LifetimeRoutine());
    }

    private void Update()
    {
        if (owner != null)
        {
            transform.position = owner.position;
        }

        tickTimer += Time.deltaTime;
        if (tickTimer >= tick)
        {
            tickTimer = 0f;
            DealTickDamage();
        }
    }

    private void DealTickDamage()
    {
        // Nếu hào quang chưa kịp xuất hiện (bán kính quá nhỏ), bỏ qua quét sát thương
        if (currentRadius <= 0.05f) return;

        // SỬ DỤNG currentRadius THAY VÌ auraRange CỐ ĐỊNH
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentRadius, targetMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            if (!hit.CompareTag(enemyTag))
            {
                continue;
            }

            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDame(damage);
                enemy.ApplyNegativeSpeed(0.15f);
            }
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        // Giai đoạn mở rộng (Grow): progress chạy từ 0 -> 1
        yield return AnimateAuraRoutine(0f, 1f, growTime);

        // Giai đoạn duy trì
        float waitTime = Mathf.Max(0f, duration - shrinkTime);
        if (waitTime > 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }

        // Giai đoạn thu nhỏ (Shrink): progress chạy từ 1 -> 0
        yield return AnimateAuraRoutine(1f, 0f, shrinkTime);
        Destroy(gameObject);
    }

    // Hàm nội bộ thay thế hoàn toàn cho ScaleRoutine cũ
    private IEnumerator AnimateAuraRoutine(float startProgress, float endProgress, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float currentProgress = Mathf.Lerp(startProgress, endProgress, elapsed / time);

            // 1. Cập nhật bán kính tính toán sát thương & Collider
            currentRadius = auraRange * currentProgress;
            if (auraCollider != null)
            {
                auraCollider.radius = currentRadius;
            }

            // 2. Cập nhật scale riêng cho Sprite hiển thị
            if (spriteRenderer != null)
            {
                float currentVisualScale = maxVisualScale * currentProgress;
                spriteRenderer.transform.localScale = new Vector3(currentVisualScale, currentVisualScale, 1f);
            }

            yield return null;
        }

        // Đảm bảo gán giá trị cuối chính xác tuyệt đối khi kết thúc vòng lặp
        currentRadius = auraRange * endProgress;
        if (auraCollider != null)
        {
            auraCollider.radius = currentRadius;
        }
        if (spriteRenderer != null)
        {
            float finalVisualScale = maxVisualScale * endProgress;
            spriteRenderer.transform.localScale = new Vector3(finalVisualScale, finalVisualScale, 1f);
        }
    }
}