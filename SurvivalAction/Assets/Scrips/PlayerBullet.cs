using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed=20f;
    [SerializeField] private float timeToDestroy=0.5f;
    [SerializeField] private float damage=10f;
    [SerializeField] private GameObject BloodPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();  
    }
    void MoveBullet()
    {
        transform.Translate(Vector2.right*moveSpeed*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDame(damage);
                GameObject blood = Instantiate(BloodPrefab, collision.transform.position, Quaternion.identity);
                Destroy(blood, 1f);
            }
             Destroy(gameObject);
        }
    }
}
