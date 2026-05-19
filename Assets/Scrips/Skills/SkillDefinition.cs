using UnityEngine;

[CreateAssetMenu(fileName = "SkillDefinition", menuName = "Game/Skills/Skill Definition")]
public class SkillDefinition : ScriptableObject
{
    [SerializeField] private string skillId;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private SkillRuntime skillPrefab;

    public string SkillId => skillId;
    public string SkillName => skillName;
    public string Description => description;
    public Sprite Icon => icon;
    public SkillRuntime SkillPrefab => skillPrefab;
}
