using System;
using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp;
    [SerializeField] private int baseExpToLevelUp = 5;
    [SerializeField] private int expGrowthPerLevel = 2;
    [SerializeField] private int maxLevel = 20; // Thêm biến Max Level ở đây

    // Events
    public event Action<int, int> OnExpChanged; // (currentExp, maxExp)
    public event Action<int> OnLevelUp;         // (newLevel)

    // Properties
    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int MaxLevel => maxLevel;
    public bool IsMaxLevel => currentLevel >= maxLevel; // Thuộc tính kiểm tra xem đã max cấp chưa

    // Nếu đã max level, trả về 0 hoặc giữ nguyên mốc cũ để tránh lỗi UI chia cho 0
    public int ExpToNextLevel => IsMaxLevel ? 0 : GetExpRequiredForLevel(currentLevel);

    private void Start()
    {
        NotifyExpChanged();
    }

    public void AddExp(int amount)
    {
        // Nếu lượng EXP hợp lệ hoặc đã đạt cấp tối đa thì không làm gì cả
        if (amount <= 0 || IsMaxLevel) return;

        currentExp += amount;

        // Vòng lặp thăng cấp kèm điều kiện kiểm tra Max Level
        while (currentExp >= ExpToNextLevel && !IsMaxLevel)
        {
            int expRequiredForThisLevel = ExpToNextLevel;

            currentLevel++;
            currentExp -= expRequiredForThisLevel;

            OnLevelUp?.Invoke(currentLevel);

            // Nếu sau khi lên cấp mà đạt Max Level luôn thì reset EXP thừa về 0 và thoát vòng lặp
            if (IsMaxLevel)
            {
                currentExp = 0;
                break;
            }
        }

        NotifyExpChanged();
    }

    public int GetExpRequiredForLevel(int level)
    {
        if (level <= 1) return baseExpToLevelUp;
        return baseExpToLevelUp + (level - 1) * expGrowthPerLevel;
    }

    private void NotifyExpChanged()
    {
        // Trả về (0, 0) hoặc (0, mốc_level_20) tùy thuộc vào cách bạn muốn hiển thị UI khi Max
        if (IsMaxLevel)
        {
            OnExpChanged?.Invoke(0, 0);
        }
        else
        {
            OnExpChanged?.Invoke(currentExp, ExpToNextLevel);
        }
    }
}