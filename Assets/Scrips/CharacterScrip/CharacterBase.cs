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
        float finalHp = baseData.baseHp + (hpLevel * hpPerLevel);
        float finalDamage = baseData.baseDamage + (damageLevel * damagePerLevel);

        player.SetMoveSpeed(baseData.moveSpeed);
        player.SetMaxHp(finalHp);
        gun.SetBaseBulletDamage(finalDamage);
    }
}
