using UnityEngine;

[System.Serializable]
public class GameData {

    [SerializeField] private bool isMutedMusic;
    [SerializeField] private bool isMutedSFX;
    [SerializeField] private PlayerData playerData;

    public GameData() {

        this.isMutedMusic = false;
        this.isMutedSFX = false;
        this.playerData = new PlayerData();
    }

    public PlayerData GetPlayerData() {
        return playerData;
    }

    public bool IsMutedMusic() {
        return isMutedMusic;
    }

    public bool IsMutedSFX() {
        return isMutedSFX;
    }

    public void MutedSFX() {
        this.isMutedSFX = true;
    }

    public void UnMutedSFX() {
        this.isMutedSFX = false;
    }

    public void MutedMusic() {
        this.isMutedMusic = true;
    }

    public void UnMutedMusic() {
        this.isMutedMusic = false;
    }
}
