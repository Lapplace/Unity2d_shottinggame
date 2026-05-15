using UnityEngine;

public class PlayerCollistion : MonoBehaviour
{
    [SerializeField] private GameManeger gameManager;
    [SerializeField] private AudioManeger audioManeger;
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManeger>();
        }

        if (audioManeger == null)
        {
            audioManeger = FindFirstObjectByType<AudioManeger>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("EnemyBullet"))
        {
            if (player != null)
            {
                player.TakeDame(10f);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Usb"))
        {
            Destroy(collision.gameObject);
            if (gameManager != null)
            {
                gameManager.WinGameMenu();
            }
        }
        else if (collision.CompareTag("Energy"))
        {
            if (gameManager != null)
            {
                gameManager.AddEnergy();
            }
            Destroy(collision.gameObject);
            if (audioManeger != null)
            {
                audioManeger.PlayEnergySound();
            }
        }
    } 
}
