using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OrbitingOrbHitbox : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";

    private OrbitingOrbSkill ownerSkill;

    public void Initialize(OrbitingOrbSkill skill)
    {
        ownerSkill = skill;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ownerSkill == null || !other.CompareTag(enemyTag))
        {
            return;
        }

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            ownerSkill.DealDamage(enemy);
        }
    }
}
