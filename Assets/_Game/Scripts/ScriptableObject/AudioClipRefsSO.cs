using System.Collections.Generic;
using UnityEngine;

public enum SFXType {

    KnifeThrow,
    HammerThrow,
    BoomerangThrow,
    PlayerDead,
    PlayerSizeUp
}

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    [SerializeField] private List<SFX> sfxList;
    [SerializeField] private AudioClip music;

    private Dictionary<SFXType, SFX> sfxDict = new Dictionary<SFXType, SFX>();

    private SFX GetSFX(SFXType sfxType) {

        if (!sfxDict.ContainsKey(sfxType)) {

            foreach (SFX sfx in sfxList) {

                if (sfx.IsSameType(sfxType)) {

                    sfxDict.Add(sfxType, sfx);
                    break;
                }
            }
        }

        return sfxDict[sfxType];
    }

    public List<AudioClip> GetAudioClipListWithType(SFXType sfxType) {

        return GetSFX(sfxType).GetAudioCliplist();
    }

    public AudioClip GetAudioClipListWithTypeAndIndex(SFXType sfxType, int index) {

        return GetSFX(sfxType).GetAudioClipWithIndex(index);
    }

    public AudioClip GetMusic() {
        return music;
    }
}

[System.Serializable]
public class SFX {
    
    [SerializeField] private string sfxName;
    [SerializeField] private SFXType sfxType;
    [SerializeField] private List<AudioClip> audioClipList;

    public AudioClip GetAudioClipWithIndex(int index) {

        return audioClipList[index];
    }

    public List<AudioClip> GetAudioCliplist() {
        return audioClipList;
    }

    public bool IsSameType(SFXType sfxType) {
        return this.sfxType == sfxType;
    }
}
