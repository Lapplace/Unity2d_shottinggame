using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeSkill : SkillRuntime
{
    [System.Serializable]
    public class LightningLevelStats
    {
        public int strikeCount = 1;
        public float damage = 50f;
        public float cooldown = 5f;
        public float areaRadius;
    }

    [Header("Setup")]
    [SerializeField] private Transform owner;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private LightningLevelStats[] statsByLevel;

    [Header("Targeting")]
    [SerializeField] private float targetRange = 6f;
    [SerializeField] private LayerMask targetMask = ~0;
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Timing")]
    [SerializeField] private float delayBetweenStrikes = 0.2f;
    [SerializeField] private float visualLifetime = 0.6f;

    private int currentLevel = 1;
    private Coroutine strikeLoop;

    public override int CurrentLevel => currentLevel;
    public override int MaxLevel => statsByLevel != null && statsByLevel.Length > 0 ? statsByLevel.Length : 1;

    private void OnEnable()
    {
        TryResolveOwner();

        if (strikeLoop == null)
        {
            strikeLoop = StartCoroutine(StrikeLoop());
        }
    }

    private void OnDisable()
    {
        if (strikeLoop != null)
        {
            StopCoroutine(strikeLoop);
            strikeLoop = null;
        }
    }

    public override void Upgrade()
    {
        currentLevel = Mathf.Min(currentLevel + 1, MaxLevel);
    }

    private IEnumerator StrikeLoop()
    {
        while (true)
        {
            if (TryResolveOwner())
            {
                LightningLevelStats stats = CurrentStats();
                yield return CastLightningSequence(stats);
                yield return new WaitForSeconds(Mathf.Max(0.05f, stats.cooldown));
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator CastLightningSequence(LightningLevelStats stats)
    {
        List<Enemy> targetPool = CollectTargetsInRange();
        int strikeCount = Mathf.Max(1, stats.strikeCount);

        for (int i = 0; i < strikeCount; i++)
        {
            if (targetPool.Count == 0)
            {
                targetPool = CollectTargetsInRange();
            }

            Enemy target = PickRandomTarget(targetPool);
            if (target == null)
            {
                yield break;
            }

            StrikeTarget(target, stats);

            if (i < strikeCount - 1)
            {
                yield return new WaitForSeconds(Mathf.Max(0f, delayBetweenStrikes));
            }
        }
    }

    private List<Enemy> CollectTargetsInRange()
    {
        List<Enemy> targets = new();
        HashSet<Enemy> uniqueTargets = new();

        if (owner == null)
        {
            return targets;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.position, targetRange, targetMask);
        for (int i = 0; i < hits.Length; i++)
        {
            Enemy enemy = GetEnemyFromCollider(hits[i]);
            if (enemy != null && uniqueTargets.Add(enemy))
            {
                targets.Add(enemy);
            }
        }

        return targets;
    }

    private Enemy PickRandomTarget(List<Enemy> targetPool)
    {
        while (targetPool.Count > 0)
        {
            int index = Random.Range(0, targetPool.Count);
            Enemy target = targetPool[index];
            targetPool.RemoveAt(index);

            if (target != null && target.gameObject.activeInHierarchy)
            {
                return target;
            }
        }

        return null;
    }

    private void StrikeTarget(Enemy target, LightningLevelStats stats)
    {
        Vector3 strikePosition = target.transform.position;
        SpawnLightningVisual(strikePosition);

        if (stats.areaRadius > 0f)
        {
            DealAreaDamage(strikePosition, stats.damage, stats.areaRadius);
            return;
        }

        target.TakeDame(stats.damage);
    }

    private void DealAreaDamage(Vector3 center, float damage, float areaRadius)
    {
        HashSet<Enemy> damagedEnemies = new();
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, areaRadius, targetMask);

        for (int i = 0; i < hits.Length; i++)
        {
            Enemy enemy = GetEnemyFromCollider(hits[i]);
            if (enemy != null && damagedEnemies.Add(enemy))
            {
                enemy.TakeDame(damage);
            }
        }
    }

    private void SpawnLightningVisual(Vector3 position)
    {
        if (lightningPrefab == null)
        {
            return;
        }

        GameObject lightning = Instantiate(lightningPrefab, position, Quaternion.identity);
        Destroy(lightning, Mathf.Max(0.05f, visualLifetime));
    }

    private Enemy GetEnemyFromCollider(Collider2D hit)
    {
        if (hit == null)
        {
            return null;
        }

        Enemy enemy = hit.GetComponentInParent<Enemy>();
        if (enemy == null)
        {
            return null;
        }

        if (!hit.CompareTag(enemyTag) && !enemy.CompareTag(enemyTag))
        {
            return null;
        }

        return enemy;
    }

    private LightningLevelStats CurrentStats()
    {
        if (statsByLevel == null || statsByLevel.Length == 0)
        {
            return new LightningLevelStats();
        }

        return statsByLevel[Mathf.Clamp(currentLevel - 1, 0, statsByLevel.Length - 1)];
    }

    private bool TryResolveOwner()
    {
        if (owner != null && owner.gameObject.activeInHierarchy)
        {
            return true;
        }

        Player player = FindFirstObjectByType<Player>(FindObjectsInactive.Include);
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            return false;
        }

        owner = player.transform;
        return true;
    }
}
