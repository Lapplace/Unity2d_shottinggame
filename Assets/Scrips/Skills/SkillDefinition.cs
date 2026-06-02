using UnityEngine;

[CreateAssetMenu(fileName = "SkillDefinition", menuName = "Game/Skills/Skill Definition")]
public class SkillDefinition : ScriptableObject
{
    [SerializeField] private string skillId;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string description;// khi descriptionsByLevel == null thì đưa ra nội dung thay thế
    [TextArea]
    [SerializeField] private string[] descriptionsByLevel;
    [SerializeField] private Sprite icon;
    [SerializeField] private SkillRuntime skillPrefab;

    public string SkillId => skillId;
    public string SkillName => skillName;
    public string Description => description;
    public Sprite Icon => icon;
    public SkillRuntime SkillPrefab => skillPrefab;
    public int DescriptionLevelsCount => descriptionsByLevel != null && descriptionsByLevel.Length > 0 ? descriptionsByLevel.Length : 1;
    public string GetDescriptionForLevel(int level)
    {
        if (descriptionsByLevel == null || descriptionsByLevel.Length == 0)
        {
            return description;
        }

        int index = Mathf.Clamp(level - 1, 0, descriptionsByLevel.Length - 1);
        string levelDescription = descriptionsByLevel[index];

        if (string.IsNullOrWhiteSpace(levelDescription))
        {
            return description;
        }

        return levelDescription;
    }
}
