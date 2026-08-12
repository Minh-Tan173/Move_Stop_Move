using UnityEngine;

public enum LevelState {
    Start,
    Playing,
    Finish,
    Complete
}

public class LevelManager : Singleton<LevelManager>
{
    private LevelState currentState;
    
    private void OnInit() { 
    

    }

    private void OnDespawn() {

    }

    private void LoadLevel() {

    }
    
    private void OnPlay() {

        ChangeLevelState(LevelState.Playing);
    }

    private void OnComplete() {

    }

    private void ChangeLevelState(LevelState levelState) {

        this.currentState = levelState;
    }

    public bool IsGamePlaying() {
        return this.currentState == LevelState.Playing;
    }
}
