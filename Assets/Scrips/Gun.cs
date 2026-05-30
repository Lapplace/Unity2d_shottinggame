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
    private float currentBulletDamage = 10f;
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
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            if (playerBullet != null)
            {
                playerBullet.SetDamage(currentBulletDamage);
                playerBullet.SetUseUnscaledMovement(useUnscaledTime);
            }
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
