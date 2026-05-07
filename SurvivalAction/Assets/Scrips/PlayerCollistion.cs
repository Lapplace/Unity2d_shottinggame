using UnityEngine;

public class PlayerCollistion : MonoBehaviour
{
    [SerializeField] private GameManeger gameManager;
    [SerializeField] private AudioManeger audioManeger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyBullet"))
        {
            // Handle collision with enemy bullet
            Player player = GetComponent<Player>();
            player.TakeDame(10f); // Example damage value
        }
        else if(collision.CompareTag("Usb"))
        {
            Destroy(collision.gameObject);
            gameManager.WinGameMenu();
        }
        else if(collision.CompareTag("Energy"))
        {
            gameManager.AddEnergy();
            Destroy(collision.gameObject);
            audioManeger.PlayEnergySound();
        }
    }
}
