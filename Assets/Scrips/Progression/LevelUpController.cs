using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private PlayerProgression progression;
    [SerializeField] private LevelUpPanel levelUpPanel;

    private void Awake()
    {
        if (progression == null)
        {
            progression = FindFirstObjectByType<PlayerProgression>();
        }

        if (levelUpPanel == null)
        {
            levelUpPanel = FindFirstObjectByType<LevelUpPanel>(FindObjectsInactive.Include);
        }
    }

    private void OnEnable()
    {
        if (progression != null)
        {
            progression.OnLevelUp += OnLevelUp;
        }
    }

    private void OnDisable()
    {
        if (progression != null)
        {
            progression.OnLevelUp -= OnLevelUp;
        }
    }

    private void OnLevelUp(int newLevel)
    {
        if (progression != null && progression.IsMaxLevel)
        {
            return;
        }

        if (levelUpPanel != null && !levelUpPanel.HasAnyUpgradableSkill())
        {
            return;
        }
        levelUpPanel.OpenPanel();
    }
}
