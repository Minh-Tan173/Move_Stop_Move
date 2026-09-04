using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Data")]
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    private float sfxVolume;

    private void Start() {

        sfxVolume = GetSFXVolume();
    }

    private float GetSFXVolume() {
        return DataManager.GetGameData().IsMutedSFX() ? 0f : 1f;
    }

    public void SetMutedSFX(bool isMuted) {

        DataManager.MutedSFX(isMuted);

        sfxVolume = GetSFXVolume();
    }

    public void PlaySound(Vector3 position, SFXType sfxType, int audioIndex) {

        AudioClip audioClip = audioClipRefsSO.GetAudioClipListWithTypeAndIndex(sfxType, audioIndex);
        AudioSource.PlayClipAtPoint(audioClip, position, sfxVolume);

    }

    public void PlayUISound(SFXType sfxType, int audioIndex) {

        AudioClip audioClip = audioClipRefsSO.GetAudioClipListWithTypeAndIndex(sfxType, audioIndex);
        audioSource.PlayOneShot(audioClip, sfxVolume);
    }

    public AudioClipRefsSO GetAudioClipRefsSO() {
        return this.audioClipRefsSO;
    }
}
