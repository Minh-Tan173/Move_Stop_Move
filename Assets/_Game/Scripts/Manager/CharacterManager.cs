using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : Singleton<CharacterManager>
{
    [Header("Character Prefab")]
    [SerializeField] private CharacterBase playerPrefab;
    [SerializeField] private CharacterBase botPrefab;

    [Header("Spawn Bot Behavior")]
    [SerializeField] private Transform pooling;
    [SerializeField] private int maxBotCountInLevel = 50;
    [SerializeField] private int maxBotCountRuntime = 10;

    private List<CharacterBase> charActiveList = new List<CharacterBase>();
    private List<CharacterBase> charDeactiveList = new List<CharacterBase>();

    private LevelBase currentLevel;

    private Player player;
    private int totalBotSpawned;
        

    public void OnInit() {

        currentLevel = LevelManager.Instance.GetCurrentLeveL();

        ResetTotalBotSpawned();

        player = SpawnPlayer(currentLevel.GetSpawnPlayerPoint());
    }

    public void OnGamePlaying() {


    }

    public void OnDespawn() {

        for (int i = charActiveList.Count - 1; i >= 0; i--) {

            DeadCharacter(charActiveList[i], updateUI: false);
        }

        charActiveList.Clear();
        charDeactiveList.Clear();

        UpdateAliveUI();
    }

    private void Update() {


        if (totalBotSpawned < maxBotCountInLevel) {
            // Total character can't over 50 (includes player)

            if (charActiveList.Count < maxBotCountRuntime) {
                // If not enough character on field

               if (currentLevel.TryGetRandomSpawnPoint(out Vector3 spawnPos)) {

                    SpawnBot(spawnPos);
                }
            }
        }
    }

    private void UpdateAliveUI() {

        UIManager.Instance.GetUI<CanvasHUD>().UpdateAliveLeftText(charActiveList.Count);
    }

    private void IncreaseTotalBotSpawned() {

        totalBotSpawned += 1;
    }

    private void ResetTotalBotSpawned() {
        totalBotSpawned = 0;
    }

    private Player SpawnPlayer(Vector3 spawnPos) {

        CharacterBase player = SimplePool.Spawn<CharacterBase>(playerPrefab, spawnPos, Quaternion.identity);

        charDeactiveList.Remove(player);
        charActiveList.Add(player);

        player.OnInit();

        UpdateAliveUI();

        return player as Player;
    }

    private void SpawnBot(Vector3 spawnPos) {

        float randomYRot = Random.Range(0f, 180f);
        Quaternion botRot = Quaternion.Euler(0f, randomYRot, 0f);
        CharacterBase bot = SimplePool.Spawn<CharacterBase>(botPrefab, spawnPos, botRot);

        IncreaseTotalBotSpawned();

        bot.OnInit();
        bot.GetCanvasCharacter().SetIndex(totalBotSpawned);
        UIManager.Instance.GetUI<CanvasOffScreenIndicator>().Register(bot.GetCanvasCharacter());

        charDeactiveList.Remove(bot);
        charActiveList.Add(bot);


        UpdateAliveUI();
    }

    private void DespawnCharacter(CharacterBase character) {
        SimplePool.Despawn(character);
    }

    private IEnumerator IEDespawnCharacter(CharacterBase character) {

        yield return new WaitForSeconds(1.1f);

        SimplePool.Despawn(character);
    }

    public void DeadCharacter(CharacterBase character, bool updateUI = true) {

        UIManager.Instance.GetUI<CanvasOffScreenIndicator>().UnRegister(character.GetCanvasCharacter());

        character.Dead();

        StartCoroutine(IEDespawnCharacter(character));

        charActiveList.Remove(character);
        charDeactiveList.Add(character);

        if (updateUI) { UpdateAliveUI(); }
    }

    public List<CharacterBase> GetActiveCharacterList() {
        return charActiveList;
    }

    public Player GetPlayer() {
        return player;
    }
}
