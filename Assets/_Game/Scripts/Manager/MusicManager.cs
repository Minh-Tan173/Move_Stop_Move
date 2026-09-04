using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private AudioSource audioSource;

    private AudioClip theme;

    public void PlayGameTheme() {

        audioSource.clip = audioClipRefsSO.GetMusic();
        audioSource.volume = DataManager.GetGameData().IsMutedMusic() ? 0f : 1f;
        audioSource.Play();
    }

    public void StopPlayTheme() {

        audioSource.Stop();
    }

    public void SetMutedMusic(bool isMutedMusic) {

        DataManager.MutedMusic(isMutedMusic);

        audioSource.volume = DataManager.GetGameData().IsMutedMusic() ? 0f : 1f;
    }
}
