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

    public void Setup(SkillDefinition definition, LevelUpPanel levelUpPanel)
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
            skillName.text = definition.SkillName;
        }

        if (description != null)
        {
            description.text = definition.Description;
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
