using UnityEngine;

public enum LevelState { Start, Playing, Finish, Complete }

public class LevelManager : Singleton<LevelManager>
{
    [Header("List of Level")]
    [SerializeField] private LevelSO levelSO;

    private LevelState currentState;

    #region Level Data
    private int currentLevelIndex = 0; // TẠM THỜI
    private LevelBase currentLevel;
    #endregion

    private void Start() {

        OnInit();

        Invoke(nameof(OnPlay), 1f);
    }

    private void SwitchToNextLevel() {
        currentLevelIndex += 1;
    }

    private void LoadLevel() {

        if (currentLevel != null) {
            // Own old level before

            LevelBase.DestroyLevel(currentLevel);
        }

        currentLevel = LevelBase.SpawnLevel(levelSO.GetLeveLByIndex(currentLevelIndex));
    }

    public void OnInit() {

        LoadLevel();

        UIManager.Instance.GetUI<CanvasOffScreenIndicator>();

        CharacterManager.Instance.OnInit();

        CameraManager.Instance.SetTracking(CharacterManager.Instance.GetPlayer().UnitTF);

        ChangeLevelState(LevelState.Start);
    }

    public void OnDespawn() {

    }
    
    public void OnPlay() {

        UIManager.Instance.OpenUI<CanvasOffScreenIndicator>();

        ChangeLevelState(LevelState.Playing);
    }

    public void OnFinish() {

        ChangeLevelState(LevelState.Finish);
        Invoke(nameof(OnComplete), 0.5f);
    }

    public void OnComplete() {

        ChangeLevelState(LevelState.Complete);
    }

    private void ChangeLevelState(LevelState levelState) {

        this.currentState = levelState;
    }

    public bool IsGamePlaying() {
        return this.currentState == LevelState.Playing;
    }

    public LevelBase GetCurrentLeveL() {
        return currentLevel;
    }
}
