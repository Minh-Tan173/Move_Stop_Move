using System;
using System.Collections;
using UnityEngine;

public enum LevelState { Start, Playing, Finish, Complete }

public class LevelManager : Singleton<LevelManager>
{

    [Header("List of Level")]
    [SerializeField] private LevelSO levelSO;

    private LevelState currentState;

    #region Level Data
    private int currentLevelIndex;
    private LevelBase currentLevel;
    #endregion

    private void Start() {

        OnStart();
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

    public void OnStart() {

        DataManager.OnInit();
        currentLevelIndex = DataManager.GetGameData().GetPlayerData().CurrentLevelIndex;

        LoadLevel();

        CharacterManager.Instance.OnInit();

        EventManager.Instance.OnInit();

        UIManager.Instance.GetUI<CanvasOffScreenIndicator>();
        UIManager.Instance.CloseUI<CanvasOffScreenIndicator>(0f);
        UIManager.Instance.OpenUI<CanvasMainMenu>();

        CameraManager.Instance.SetTracking(CharacterManager.Instance.GetPlayer().UnitTF);

        ChangeLevelState(LevelState.Start);
    }

    public void OnDespawn() {

        CharacterManager.Instance.OnDespawn();

        SimplePool.CollectAll();
    }
    
    public void OnPlay() {

        UIManager.Instance.OpenUI<CanvasOffScreenIndicator>();
        CanvasHUD canvasHUD = UIManager.Instance.OpenUI<CanvasHUD>();
        canvasHUD.Invoke(nameof(canvasHUD.ActiveCountdown), 0.5f);

        CameraManager.Instance.SwitchCam(CameraType.GamePlayCamera);
    }

    public void OnRestart() {

        OnDespawn();

        OnStart();
        UIManager.Instance.CloseUI<CanvasMainMenu>(0f);
        
        CanvasLoading canvasLoading = UIManager.Instance.OpenUI<CanvasLoading>();
        canvasLoading.ActiveLoading(() => {

            OnPlay();
        });
    }

    public void OnFinish() {

        UIManager.Instance.CloseUI<CanvasHUD>(0.3f);

        ChangeLevelState(LevelState.Finish);
        Invoke(nameof(OnComplete), 0.5f);
    }

    public void OnComplete() {

        ChangeLevelState(LevelState.Complete);
    }

    public void ChangeLevelState(LevelState levelState) {

        this.currentState = levelState;
    }

    public bool IsGamePlaying() {
        return this.currentState == LevelState.Playing;
    }

    public LevelBase GetCurrentLeveL() {
        return currentLevel;
    }
}
