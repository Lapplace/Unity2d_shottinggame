using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillOptionView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;

    private SkillDefinition skill;
    private LevelUpPanel panel;

    private static string FormatSkillNameWithLevel(string baseName, int previewLevel)
    {
        int bonusLevel = Mathf.Max(0, previewLevel - 1);
        if (bonusLevel <= 0)
        {
            return baseName;
        }

        return $"{baseName} +{bonusLevel}";
    }

    public void Setup(SkillDefinition definition, LevelUpPanel levelUpPanel, int previewLevel)
    {
        skill = definition;
        panel = levelUpPanel;

        if (icon != null)
        {
            icon.sprite = definition.Icon;
            icon.enabled = definition.Icon != null;
        }

        if (skillName != null)
        {
            skillName.text = FormatSkillNameWithLevel(definition.SkillName, previewLevel);
        }

        if (description != null)
        {
            description.text = definition.GetDescriptionForLevel(previewLevel);
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        panel.SelectSkill(skill);
    }
}
