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

    private bool isWin;
    private bool isLoss;

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

        isWin = false;
        isLoss = true;

        DataManager.OnInit();
        currentLevelIndex = DataManager.GetGameData().GetPlayerData().CurrentLevelIndex;

        LoadLevel();

        CharacterManager.Instance.OnInit();

        EventManager.Instance.OnInit();

        UIManager.Instance.GetUI<CanvasOffScreenIndicator>();
        UIManager.Instance.OpenUI<CanvasMainMenu>();

        CameraManager.Instance.SetTracking(CharacterManager.Instance.GetPlayer().UnitTF);

        MusicManager.Instance.PlayGameTheme();

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

        UIManager.Instance.GetUI<CanvasHUD>().StopUIAnimation();
        UIManager.Instance.CloseUI<CanvasOffScreenIndicator>(0f);

        OnDespawn();

        OnStart();
        UIManager.Instance.CloseUI<CanvasMainMenu>(0f);

        CanvasLoading canvasLoading = UIManager.Instance.OpenUI<CanvasLoading>();
        canvasLoading.ActiveLoading(OnPlay);
    }

    public void OnFinish() {

        UIManager.Instance.GetUI<CanvasHUD>().StopUIAnimation();
        UIManager.Instance.CloseUI<CanvasHUD>(0.3f);

        ChangeLevelState(LevelState.Finish);

        if (isWin) {
            Invoke(nameof(OnWin), 0.5f);
        }
        else if (isLoss) {

            Invoke(nameof(OnLoss), 0.5f);
        }
    }

    public void OnWin() {

        MusicManager.Instance.StopPlayTheme();
        UIManager.Instance.OpenUI<CanvasWin>();
    }

    public void OnLoss() {

        MusicManager.Instance.StopPlayTheme();
        UIManager.Instance.OpenUI<CanvasLoss>();

        ChangeLevelState(LevelState.Complete);
    }

    public void BackToMainMenu() {

        OnDespawn();

        UIManager.Instance.CloseAllUI();
        CanvasLoading canvasLoading = UIManager.Instance.OpenUI<CanvasLoading>();
        canvasLoading.ActiveLoading(OnStart, isDelayClose: false);
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

    public void SetWin() {
        isWin = true;
    }

    public void SetLoss() {
        isLoss = true;
    }
}
