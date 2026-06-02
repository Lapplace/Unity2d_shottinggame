using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Transform skillRoot;

    private readonly Dictionary<string, SkillRuntime> acquiredSkills = new();

    private void Awake()
    {
        if (skillRoot == null)
        {
            skillRoot = transform;
        }
    }

    public void AcquireOrUpgrade(SkillDefinition definition)
    {
        if (definition == null || definition.SkillPrefab == null)
        {
            return;
        }

        int maxLevel = GetMaxLevel(definition.SkillId, definition);

        if (acquiredSkills.TryGetValue(definition.SkillId, out SkillRuntime runtime) && runtime != null)
        {
            if (runtime.CurrentLevel >= maxLevel)
            {
                return;
            }
            runtime.Upgrade();
            return;
        }

        SkillRuntime instance = Instantiate(definition.SkillPrefab, skillRoot);
        acquiredSkills[definition.SkillId] = instance;
    }
    public int GetCurrentLevel(string skillId)
    {
        if (string.IsNullOrEmpty(skillId))
        {
            return 0;
        }

        if (acquiredSkills.TryGetValue(skillId, out SkillRuntime runtime) && runtime != null)
        {
            return runtime.CurrentLevel;
        }

        return 0;
    }
    public int GetMaxLevel(string skillId, SkillDefinition definition = null)
    {
        int maxByDefinitionText = 1;
        if (definition != null)
        {
            maxByDefinitionText = Mathf.Max(1, definition.DescriptionLevelsCount);

            if (definition.SkillPrefab != null)
            {
                return Mathf.Max(maxByDefinitionText, definition.SkillPrefab.MaxLevel);
            }
        }

        return maxByDefinitionText;
    }
}
