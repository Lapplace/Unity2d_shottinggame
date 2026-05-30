using UnityEngine;

public abstract class SkillRuntime : MonoBehaviour
{
    [SerializeField] private string skillId;

    public string SkillId => skillId;
    public virtual int CurrentLevel => 1;
    public virtual int MaxLevel => 1;
    public abstract void Upgrade();
}
