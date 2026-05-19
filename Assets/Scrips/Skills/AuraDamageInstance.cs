using System.Collections;
using UnityEngine;

public class AuraDamageInstance : MonoBehaviour
{
    [SerializeField] private CircleCollider2D auraCollider;
    [SerializeField] private LayerMask targetMask = ~0;
    [SerializeField] private string enemyTag = "Enemy";

    private Transform owner;
    private float damage;
    private float tick;
    private float duration;
    private float growTime;
    private float shrinkTime;

    private float tickTimer;
    private float auraRange;
    private Vector3 targetScale = Vector3.one;

    public void Initialize(Transform auraOwner, float auraDamage, float range, float damageTick, float auraDuration, float growDuration, float shrinkDuration)
    {
        owner = auraOwner;
        damage = auraDamage;
        auraRange = Mathf.Max(0.1f, range);
        tick = Mathf.Max(0.05f, damageTick);
        duration = Mathf.Max(0.05f, auraDuration);
        growTime = Mathf.Max(0.01f, growDuration);
        shrinkTime = Mathf.Max(0.01f, shrinkDuration);

        if (auraCollider == null)
        {
            auraCollider = GetComponent<CircleCollider2D>();
        }

        if (auraCollider != null)
        {
            auraCollider.radius = auraRange;
        }

        targetScale = Vector3.one * auraRange * 2f;
        transform.localScale = Vector3.zero;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, auraRange, targetMask);
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
            }
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return ScaleRoutine(Vector3.zero, targetScale, growTime);

        float waitTime = Mathf.Max(0f, duration - shrinkTime);
        if (waitTime > 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }

        yield return ScaleRoutine(transform.localScale, Vector3.zero, shrinkTime);
        Destroy(gameObject);
    }

    private IEnumerator ScaleRoutine(Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(from, to, elapsed / time);
            yield return null;
        }

        transform.localScale = to;
    }
}
