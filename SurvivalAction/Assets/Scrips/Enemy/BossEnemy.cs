using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy:Enemy
{
    [SerializeField] private GameObject bulletPerfabs;
    [SerializeField] private Transform firePoin;
    [SerializeField] private float speedDanThuong=20f;
    [SerializeField] private float speedDanVongTron=10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCooldown = 2f;
    private float nextSkillTime = 0f;
    [SerializeField] private GameObject usbPrefabs;
    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextSkillTime)
        {
            SuDungSkill();
        }
    }
    protected override void Die()
    {
        Instantiate(usbPrefabs, transform.position, Quaternion.identity);
        base.Die();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(player != null)
            {
                player.TakeDame(enterDame);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
            if (collision.CompareTag("Player"))
            {
                if(player != null)
                {
                    player.TakeDame(stayDame);
                }
            }
    }
    private void BanDanThuong() 
    {
        if (player != null)
        {
            Vector3 diretionToPlayer=player.transform.position - firePoin.position;
            diretionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPerfabs, firePoin.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(diretionToPlayer * speedDanThuong);
        }
    }
    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(bulletPerfabs, firePoin.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(direction * speedDanVongTron);
        }
    }
    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }
    private void SinhMiniEnemy()
    {
        Instantiate(miniEnemy, transform.position, Quaternion.identity);
    }
    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }
    private void ChonSkillNN()
    {
        int randomSkill = Random.Range(0, 5);
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                BanDanVongTron();
                break;
            case 2:
                HoiMau(hpValue);
                break;
            case 3:
                DichChuyen();
                break;
            case 4:
                SinhMiniEnemy();
                break;
        }
    }
    private void SuDungSkill()
    {
        nextSkillTime = Time.time + skillCooldown;
        ChonSkillNN();
    }
}
