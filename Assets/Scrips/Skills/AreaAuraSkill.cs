using System.Collections;
using UnityEngine;

public class AreaAuraSkill : SkillRuntime
{
    [System.Serializable]
    public class AuraLevelStats
    {
        public float damage = 5f;
        public float range = 1.5f;
        public float damageTick = 0.5f;
        public float cooldown = 2f;
        public float duration = 1.25f;
    }

    [Header("Setup")]
    [SerializeField] private Transform owner;
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private AuraLevelStats[] statsByLevel;

    [Header("Visual")]
    [SerializeField] private float growTime = 0.15f;
    [SerializeField] private float shrinkTime = 0.2f;

    private int currentLevel = 1;
    private Coroutine auraLoop;
  //  public override int CurrentLevel => currentLevel;
    private void OnEnable()
    {
        TryResolveOwner();

        if (auraLoop == null)
        {
            auraLoop = StartCoroutine(AuraLoop());
        }
    }

    private void OnDisable()
    {
        if (auraLoop != null)
        {
            StopCoroutine(auraLoop);
            auraLoop = null;
        }
    }

    public override void Upgrade()
    {
        currentLevel = Mathf.Min(currentLevel + 1, statsByLevel.Length);
    }

    private AuraLevelStats CurrentStats()
    {
        if (statsByLevel == null || statsByLevel.Length == 0)
        {
            return new AuraLevelStats();
        }

        return statsByLevel[Mathf.Clamp(currentLevel - 1, 0, statsByLevel.Length - 1)];
    }

    private IEnumerator AuraLoop()
    {
        while (true)
        {
            AuraLevelStats stats = CurrentStats();
            SpawnAuraInstance(stats);
            yield return new WaitForSeconds(stats.cooldown);
        }
    }

    private void SpawnAuraInstance(AuraLevelStats stats)
    {
        if (auraPrefab == null)
        {
            return;
        }

        if (!TryResolveOwner())
        {
            return;
        }

        GameObject aura = Instantiate(auraPrefab, owner.position, Quaternion.identity);
        AuraDamageInstance damageInstance = aura.GetComponent<AuraDamageInstance>();
        if (damageInstance != null)
        {
            damageInstance.Initialize(owner, stats.damage, stats.range, stats.damageTick, stats.duration, growTime, shrinkTime);
        }
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
