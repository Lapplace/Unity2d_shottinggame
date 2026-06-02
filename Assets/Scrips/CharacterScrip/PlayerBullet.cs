using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float timeToDestroy = 0.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject BloodPrefab;

    private bool piercesTargets;

    private bool useUnscaledMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }
    void MoveBullet()
    {
        float delta = useUnscaledMovement ? Time.unscaledDeltaTime : Time.deltaTime;
        transform.Translate(Vector2.right * moveSpeed * delta);
    }
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    public void SetUseUnscaledMovement(bool value)
    {
        useUnscaledMovement = value;
    }
    public void SetPiercing(bool value)
    {
        piercesTargets = value;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Enemy enemy = collision.GetComponent<Enemy>();
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDame(damage);
                if (BloodPrefab != null)
                {
                    GameObject blood = Instantiate(BloodPrefab, collision.transform.position, Quaternion.identity);
                    Destroy(blood, 1f);
                }
            }

            if (!piercesTargets)
            {
                Destroy(gameObject);
            }
        }
    }
}
