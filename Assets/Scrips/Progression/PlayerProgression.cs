using System;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp;
    [SerializeField] private int baseExpToLevelUp = 5;
    [SerializeField] private int expGrowthPerLevel = 2;

    public event Action<int, int> OnExpChanged;
    public event Action<int> OnLevelUp;

    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => GetExpRequiredForLevel(currentLevel);

    private void Start()
    {
        NotifyExpChanged();
    }

    public void AddExp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentExp += amount;

        while (currentExp >= ExpToNextLevel)
        {
            currentExp -= ExpToNextLevel;
            currentLevel++;
            OnLevelUp?.Invoke(currentLevel);
        }

        NotifyExpChanged();
    }

    private int GetExpRequiredForLevel(int level)
    {
        return Mathf.Max(1, baseExpToLevelUp + (level - 1) * expGrowthPerLevel);
    }

    private void NotifyExpChanged()
    {
        OnExpChanged?.Invoke(currentExp, ExpToNextLevel);
    }
}
