using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected CharacterData baseData;
    [SerializeField] protected Player player;
    [SerializeField] protected Gun gun;

    protected virtual void Awake()
    {
        if (player == null) player = GetComponent<Player>();
        if (gun == null) gun = GetComponentInChildren<Gun>();
    }

    public void ApplyLoadout(int hpLevel, int damageLevel, float hpPerLevel, float damagePerLevel)
    {
        ApplyLoadoutWithBase(baseData.baseHp, baseData.baseDamage, baseData.moveSpeed, hpLevel, damageLevel, hpPerLevel, damagePerLevel);
    }

    public void ApplyLoadoutWithBase(float baseHp, float baseDamage, float moveSpeed, int hpLevel, int damageLevel, float hpPerLevel, float damagePerLevel)
    {
        float finalHp = baseHp + (hpLevel * hpPerLevel);
        float finalDamage = baseDamage + (damageLevel * damagePerLevel);

        player.SetMoveSpeed(moveSpeed);
        player.SetMaxHp(finalHp);
        gun.SetBaseBulletDamage(finalDamage);
    }
}
