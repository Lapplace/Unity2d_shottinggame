using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource; // Nguồn âm nhạc
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioClip shootClip; // Nguồn âm thanh hiệu ứng
    [SerializeField] private AudioClip reLoadClip; // Nguồn âm thanh hiệu ứng
    [SerializeField] private AudioClip energyClip;
    public void PlayShootSound()
    {
        effectAudioSource.PlayOneShot(shootClip); // Phát âm thanh bắn
    }
    public void PlayReLoadSound()
    {
        effectAudioSource.PlayOneShot(reLoadClip); // Phát âm thanh nạp đạn
    }
    public void PlayEnergySound()
    {
        effectAudioSource.PlayOneShot(energyClip); // Phát âm thanh nạp đạn
    }
    public void PlayDefaultMusic()
    {
        defaultAudioSource.Play(); // Phát nhạc nền mặc định
        bossAudioSource.Stop(); // Dừng nhạc nền khi gặp boss
    }
    public void PlayBossMusic()
    {
        bossAudioSource.Play(); // Phát nhạc nền khi gặp boss
        defaultAudioSource.Stop(); // Dừng nhạc nền mặc định khi gặp boss
    }
    public void StopAudioGame()
    {
        effectAudioSource.Stop(); // Dừng âm thanh hiệu ứng
        defaultAudioSource.Stop(); // Dừng nhạc nền mặc định
        bossAudioSource.Stop(); // Dừng nhạc nền khi gặp boss
    }
}
