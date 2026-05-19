using UnityEngine;

public abstract class SkillRuntime : MonoBehaviour
{
    [SerializeField] private string skillId;

    public string SkillId => skillId;

    public abstract void Upgrade();
}
