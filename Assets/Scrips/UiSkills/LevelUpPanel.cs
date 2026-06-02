using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private SkillOptionView[] optionViews;
    [SerializeField] private SkillDefinition[] allSkills;
    [SerializeField] private SkillManager skillManager;

    private Player player;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();

        if (skillManager == null)
        {
            skillManager = FindFirstObjectByType<SkillManager>();
        }

        HidePanel();
    }

    public void OpenPanel()
    {
        if (!HasAnyUpgradableSkill())
        {
            HidePanel();
            return;
        }
        Time.timeScale = 0f;

        //if (player != null)
        //{
        //    player.SetAnimatorUseUnscaledTime(true);
        //    player.SetUseUnscaledMovement(true);
        //}

        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        RenderOptions();
    }

    public void HidePanel()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        Time.timeScale = 1f;

        //if (player != null)
        //{
        //    player.SetAnimatorUseUnscaledTime(false);
        //    player.SetUseUnscaledMovement(false);
        //}
    }

    public void SelectSkill(SkillDefinition definition)
    {
        skillManager.AcquireOrUpgrade(definition);
        HidePanel();
    }

    private void RenderOptions()
    {
        List<SkillDefinition> upgradableSkills = GetUpgradableSkills();
        List<SkillDefinition> selected = PickRandomSkills(upgradableSkills, Mathf.Min(3, upgradableSkills.Count));

        for (int i = 0; i < optionViews.Length; i++)
        {
            bool active = i < selected.Count;
            optionViews[i].gameObject.SetActive(active);

            if (active)
            {
                int nextLevel = skillManager != null ? skillManager.GetCurrentLevel(selected[i].SkillId) + 1 : 1;
                optionViews[i].Setup(selected[i], this, nextLevel);
            }
        }
    }

    public bool HasAnyUpgradableSkill()
    {
        return GetUpgradableSkills().Count > 0;
    }

    private List<SkillDefinition> GetUpgradableSkills()
    {
        List<SkillDefinition> result = new();

        if (allSkills == null || skillManager == null)
        {
            return result;
        }

        foreach (SkillDefinition skill in allSkills)
        {
            if (skill == null || string.IsNullOrEmpty(skill.SkillId))
            {
                continue;
            }

            int currentLevel = skillManager.GetCurrentLevel(skill.SkillId);
            int maxLevel = skillManager.GetMaxLevel(skill.SkillId, skill);
            if (currentLevel < maxLevel)
            {
                result.Add(skill);
            }
        }

        return result;
    }
    private List<SkillDefinition> PickRandomSkills(List<SkillDefinition> sourceSkills, int count)
    {
        List<SkillDefinition> pool = new(sourceSkills);
        List<SkillDefinition> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        return result;
    }
}
