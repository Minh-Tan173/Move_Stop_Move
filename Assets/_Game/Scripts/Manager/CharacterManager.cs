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

    public void OnInit() {

        currentLevel = LevelManager.Instance.GetCurrentLeveL();

        //SpawnPlayer(currentLevel.GetSpawnPlayerPoint());
    }

    public void OnGamePlaying() {


    }

    public void OnDespawn() {

        for (int i = charActiveList.Count - 1; i >= 0; i--) {

            DeadCharacter(charActiveList[i]);
        }

        charActiveList.Clear();
        charDeactiveList.Clear();
    }

    private void Update() {

        int botActiveTotal = charActiveList.Count;
        int botDeactiveTotal = charDeactiveList.Count;
        int botTotal = botActiveTotal + botDeactiveTotal;

        if (botTotal <= maxBotCountInLevel) {
            // Total character can't over 50 (includes player)

            if (botActiveTotal < maxBotCountRuntime) {
                // If not enough character on field

               if (currentLevel.TryGetRandomSpawnPoint(out Vector3 spawnPos)) {

                    SpawnBot(spawnPos);
                }
            }
        }
    }

    private void SpawnPlayer(Vector3 spawnPos) {

        CharacterBase player = SimplePool.Spawn<CharacterBase>(playerPrefab, spawnPos, Quaternion.identity);

        charDeactiveList.Remove(player);
        charActiveList.Add(player);

        player.OnInit();
    }

    private void SpawnBot(Vector3 spawnPos) {

        float randomYRot = Random.Range(0f, 180f);
        Quaternion botRot = Quaternion.Euler(0f, randomYRot, 0f);
        CharacterBase bot = SimplePool.Spawn<CharacterBase>(botPrefab, spawnPos, botRot);

        bot.OnInit();

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

        character.Dead();

        StartCoroutine(IEDespawnCharacter(character));
        //SimplePool.Despawn(character);

        charActiveList.Remove(character);
        charDeactiveList.Add(character);
    }

    public List<CharacterBase> GetActiveCharacterList() {
        return charActiveList;
    }
}
