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

    private int currentCharacterOnField;

    public void OnInit() {

        currentLevel = LevelManager.Instance.GetCurrentLeveL();

        ResetTotalBotSpawned();

        player = SpawnPlayer(currentLevel.GetSpawnPlayerPoint());

        UpdateAliveUI(maxBotCountInLevel + 1); // Include Player
        UIManager.Instance.CloseUI<CanvasHUD>(0f);
    }

    public void OnGamePlaying() {


    }

    public void OnDespawn() {

        StopAllCoroutines();

        for (int i = charActiveList.Count - 1; i >= 0; i--) {

            CharacterBase character = charActiveList[i];

            character.OnDespawn();
            SimplePool.Despawn(character);
        }


        charActiveList.Clear();
        charDeactiveList.Clear();
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

    private void UpdateAliveUI(int aliveValue) {

        currentCharacterOnField = aliveValue;
        UIManager.Instance.GetUI<CanvasHUD>().UpdateAliveLeftText(currentCharacterOnField);
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

        player.OnDespawn();
        player.OnInit();

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
    }

    private void DespawnCharacter(CharacterBase character) {
        SimplePool.Despawn(character);
    }

    private IEnumerator IEDespawnCharacter(CharacterBase character) {

        yield return new WaitForSeconds(1.1f);

        SimplePool.Despawn(character);
    }

    public void DeadCharacter(CharacterBase character) {

        UIManager.Instance.GetUI<CanvasOffScreenIndicator>().UnRegister(character.GetCanvasCharacter());

        character.Dead();

        StartCoroutine(IEDespawnCharacter(character));

        charActiveList.Remove(character);
        charDeactiveList.Add(character);

        UpdateAliveUI(currentCharacterOnField - 1);
    }

    public List<CharacterBase> GetActiveCharacterList() {
        return charActiveList;
    }

    public Player GetPlayer() {
        return player;
    }
}
