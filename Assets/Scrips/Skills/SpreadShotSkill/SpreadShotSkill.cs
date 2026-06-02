using UnityEngine;

public class SpreadShotSkill : SkillRuntime
{
    [System.Serializable]
    public class SpreadShotLevelStats
    {
        public int bulletCount = 2;
        public float damage = 25f;
        public bool piercesTargets;
    }

    [Header("Setup")]
    [SerializeField] private Gun gun;
    [SerializeField] private SpreadShotLevelStats[] statsByLevel;

    [Header("Spread")]
    [SerializeField] private float angleStep = 12f;// góc giữa các viên đạn trong một phát bắn

    private int currentLevel = 1;
    private bool statsApplied;

    public override int CurrentLevel => currentLevel;
    public override int MaxLevel => statsByLevel != null && statsByLevel.Length > 0 ? statsByLevel.Length : 4;

    private void OnEnable()
    {
        ApplyCurrentStats();
    }

    private void Update()
    {
        if (!statsApplied)
        {
            ApplyCurrentStats();
        }
    }

    public override void Upgrade()
    {
        currentLevel = Mathf.Min(currentLevel + 1, MaxLevel);
        statsApplied = false;
        ApplyCurrentStats();
    }

    private void ApplyCurrentStats()
    {
        if (!TryResolveGun())
        {
            return;
        }

        SpreadShotLevelStats stats = CurrentStats();
        gun.SetSpreadShotStats(stats.bulletCount, stats.damage, stats.piercesTargets, angleStep);
        statsApplied = true;
    }

    private SpreadShotLevelStats CurrentStats()
    {
        if (statsByLevel != null && statsByLevel.Length > 0)
        {
            return statsByLevel[Mathf.Clamp(currentLevel - 1, 0, statsByLevel.Length - 1)];
        }

        return DefaultStatsForLevel(currentLevel);
    }

    private SpreadShotLevelStats DefaultStatsForLevel(int level)
    {
        switch (Mathf.Clamp(level, 1, 4))
        {
            case 1:
                return new SpreadShotLevelStats { bulletCount = 2, damage = 25f, piercesTargets = false };
            case 2:
                return new SpreadShotLevelStats { bulletCount = 3, damage = 30f, piercesTargets = false };
            case 3:
                return new SpreadShotLevelStats { bulletCount = 5, damage = 35f, piercesTargets = false };
            default:
                return new SpreadShotLevelStats { bulletCount = 5, damage = 35f, piercesTargets = true };
        }
    }

    private bool TryResolveGun()
    {
        if (gun != null && gun.gameObject.activeInHierarchy)
        {
            return true;
        }

        gun = FindFirstObjectByType<Gun>(FindObjectsInactive.Include);
        return gun != null && gun.gameObject.activeInHierarchy;
    }
}
