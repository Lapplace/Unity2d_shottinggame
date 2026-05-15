using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    private void SafePlay(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            return;
        }

        if (!audioSource.gameObject.activeInHierarchy)
        {
            return;
        }

        if (!audioSource.enabled)
        {
            audioSource.enabled = true;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void SafeStop(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            return;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
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
        SafePlay(defaultAudioSource); // Phát nhạc nền mặc định
        SafeStop(bossAudioSource); // Dừng nhạc nền khi gặp boss
    }
    public void PlayBossMusic()
    {
        SafePlay(bossAudioSource); // Phát nhạc nền khi gặp boss
        SafeStop(defaultAudioSource); // Dừng nhạc nền mặc định khi gặp boss
    }
    public void StopAudioGame()
    {
        SafeStop(effectAudioSource); // Dừng âm thanh hiệu ứng
        SafeStop(defaultAudioSource); // Dừng nhạc nền mặc định
        SafeStop(bossAudioSource); // Dừng nhạc nền khi gặp boss
    }
}
