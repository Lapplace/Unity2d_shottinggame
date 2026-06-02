using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    // Các phương thức để phát và dừng âm thanh một cách an toàn tránh lỗi khi tham chiếu đến AudioSource hoặc AudioClip bị thiếu hoặc không hợp lệ.
    private void SafePlay(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            return;
        }

        if (!audioSource.gameObject.activeInHierarchy)// Kiểm tra nếu GameObject chứa AudioSource đang ko hoạt động trong cảnh
        {
            return;
        }

        if (!audioSource.enabled)// Kiểm tra nếu AudioSource bị vô hiệu hóa
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
