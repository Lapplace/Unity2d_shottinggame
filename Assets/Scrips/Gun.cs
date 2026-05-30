using UnityEngine;
using TMPro;
using System.Xml.Serialization;
public class Gun : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 24;
    public int currentAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private AudioManeger audioManeger;

    [SerializeField] private float baseBulletDamage = 10f;
    [SerializeField] private float spreadAngleStep = 12f;
    private float currentBulletDamage = 10f;
    private int spreadBulletCount = 1;
    private bool spreadBulletPierces;
    private bool useUnscaledTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioManeger == null)
        {
            audioManeger = FindFirstObjectByType<AudioManeger>();
        }
        currentAmmo = maxAmmo;
        currentBulletDamage = baseBulletDamage;
        UpdateAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        Shoot();
        Reload();
    }
    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacment = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacment.y, displacment.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);
        if (angle < -90 || angle > 90)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }
    void Shoot()
    {
        float currentTime = useUnscaledTime ? Time.unscaledTime : Time.time;
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && currentTime > nextShot)
        {
            nextShot = currentTime + shotDelay;
            FireSpreadShot();
            currentAmmo--;
            UpdateAmmoText();
            if (audioManeger != null)
            {
                audioManeger.PlayShootSound();
            }
        }
    }
    void Reload()
    {
        if (Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            UpdateAmmoText();
            if (audioManeger != null)
            {
                audioManeger.PlayShootSound();
            }
        }
    }
    //public void SetUseUnscaledTime(bool value)
    //{
    //    useUnscaledTime = value;
    //    nextShot = useUnscaledTime ? Time.unscaledTime : Time.time;
    //}

    private void FireSpreadShot()
    {
        int bulletCount = Mathf.Max(1, spreadBulletCount);
        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, GetSpreadAngleOffset(i));
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            if (playerBullet != null)
            {
                playerBullet.SetDamage(currentBulletDamage);
                playerBullet.SetUseUnscaledMovement(useUnscaledTime);
                playerBullet.SetPiercing(spreadBulletPierces);
            }
        }
    }

    private float GetSpreadAngleOffset(int bulletIndex)
    {
        if (bulletIndex <= 0)
        {
            return 0f;
        }

        int pairIndex = (bulletIndex + 1) / 2;
        int side = bulletIndex % 2 == 1 ? 1 : -1;
        return side * pairIndex * spreadAngleStep;
    }

    public void SetSpreadShotStats(int bulletCount, float damage, bool pierces, float angleStep)
    {
        spreadBulletCount = Mathf.Max(1, bulletCount);
        currentBulletDamage = Mathf.Max(1f, damage);
        spreadBulletPierces = pierces;
        spreadAngleStep = Mathf.Max(0f, angleStep);
    }

    public void SetBaseBulletDamage(float value)
    {
        baseBulletDamage = Mathf.Max(1f, value);
        currentBulletDamage = baseBulletDamage;
    }
    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            if (currentAmmo > 0)
            {
                ammoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
            }
            else
            {
                ammoText.text = "Empty";
            }
        }
    }
}
