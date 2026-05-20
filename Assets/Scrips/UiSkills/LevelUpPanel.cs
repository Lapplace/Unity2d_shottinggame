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
        Time.timeScale = 0f;

        if (player != null)
        {
            player.SetAnimatorUseUnscaledTime(true);
            player.SetUseUnscaledMovement(true);
        }

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

        if (player != null)
        {
            player.SetAnimatorUseUnscaledTime(false);
            player.SetUseUnscaledMovement(false);
        }
    }

    public void SelectSkill(SkillDefinition definition)
    {
        skillManager.AcquireOrUpgrade(definition);
        HidePanel();
    }

    private void RenderOptions()
    {
        List<SkillDefinition> selected = PickRandomSkills(Mathf.Min(3, allSkills.Length));

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

    private List<SkillDefinition> PickRandomSkills(int count)
    {
        List<SkillDefinition> pool = new(allSkills);
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
