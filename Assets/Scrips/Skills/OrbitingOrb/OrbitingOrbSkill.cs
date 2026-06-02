using System.Collections.Generic;
using UnityEngine;

public class OrbitingOrbSkill : SkillRuntime
{
    [System.Serializable]
    public class OrbLevelStats
    {
        public int orbCount = 1;
        public float damage = 20f;
        public float rotationSpeed = 180f;
    }

    [Header("Setup")]
    [SerializeField] private Transform owner;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private OrbLevelStats[] statsByLevel;

    [Header("Orbit")]
    [SerializeField] private float radius = 1.6f;

    private readonly List<Transform> orbs = new();
    private int currentLevel = 1;
    private float angle;

    public override int CurrentLevel => currentLevel;
    public override int MaxLevel => statsByLevel != null && statsByLevel.Length > 0 ? statsByLevel.Length : 1;

    private void OnEnable()
    {
        TryResolveOwner();
        RebuildOrbs();
    }

    private void OnDisable()
    {
        ClearOrbs();
    }

    private void Update()
    {
        if (!TryResolveOwner())
        {
            return;
        }

        OrbLevelStats stats = CurrentStats();
        angle = Mathf.Repeat(angle + stats.rotationSpeed * Time.deltaTime, 360f);
        UpdateOrbPositions(stats);
    }

    public override void Upgrade()
    {
        currentLevel = Mathf.Min(currentLevel + 1, MaxLevel);
        RebuildOrbs();
    }

    public void DealDamage(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        enemy.TakeDame(CurrentStats().damage);
    }

    private OrbLevelStats CurrentStats()
    {
        if (statsByLevel == null || statsByLevel.Length == 0)
        {
            return new OrbLevelStats();
        }

        return statsByLevel[Mathf.Clamp(currentLevel - 1, 0, statsByLevel.Length - 1)];
    }

    private void RebuildOrbs()
    {
        ClearOrbs();

        OrbLevelStats stats = CurrentStats();
        int orbCount = Mathf.Max(1, stats.orbCount);
        for (int i = 0; i < orbCount; i++)
        {
            Transform orb = CreateOrb(i);
            if (orb != null)
            {
                orbs.Add(orb);
            }
        }

        UpdateOrbPositions(stats);
    }

    private Transform CreateOrb(int index)
    {
        if (orbPrefab == null)
        {
            Debug.LogWarning("OrbitingOrbSkill needs an orb prefab assigned in the Inspector.", this);
            return null;
        }

        GameObject orb = Instantiate(orbPrefab, transform);
        orb.name = $"Orbiting Orb {index + 1}";

        OrbitingOrbHitbox hitbox = orb.GetComponentInChildren<OrbitingOrbHitbox>();
        if (hitbox != null)
        {
            hitbox.Initialize(this);
        }
        else
        {
            Debug.LogWarning("Orbiting orb prefab should include an OrbitingOrbHitbox component.", orb);
        }

        return orb.transform;
    }

    private void UpdateOrbPositions(OrbLevelStats stats)
    {
        if (owner == null || orbs.Count == 0)
        {
            return;
        }

        int orbCount = Mathf.Max(1, stats.orbCount);
        float step = 360f / orbCount;
        for (int i = 0; i < orbs.Count; i++)
        {
            float orbAngle = (angle + step * i) * Mathf.Deg2Rad;
            Vector3 offset = new(Mathf.Cos(orbAngle), Mathf.Sin(orbAngle), 0f);
            orbs[i].position = owner.position + offset * radius;
        }
    }

    private void ClearOrbs()
    {
        for (int i = orbs.Count - 1; i >= 0; i--)
        {
            if (orbs[i] != null)
            {
                orbs[i].gameObject.SetActive(false);
                Destroy(orbs[i].gameObject);
            }
        }

        orbs.Clear();
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
