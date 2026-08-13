using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : Singleton<CharacterManager>
{
    [Header("Spawn bot")]
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

                Debug.Log("Can Spawn Bot");

               if (currentLevel.TryGetRandomSpawnPoint(out Vector3 spawnPos)) {

                    SpawnBot(spawnPos);
                }
            }
        }
    }

    private void SpawnPlayer(Vector3 spawnPos) {

        CharacterBase player = SimplePool.Spawn<CharacterBase>(PoolType.Character, spawnPos, Quaternion.identity);

        charDeactiveList.Remove(player);
        charActiveList.Add(player);

        player.OnInit();
    }

    private void SpawnBot(Vector3 spawnPos) {

        Debug.Log("SPAWN BOT");

        float randomYRot = Random.Range(0f, 180f);
        Quaternion botRot = Quaternion.Euler(0f, randomYRot, 0f);
        CharacterBase bot = SimplePool.Spawn<CharacterBase>(PoolType.Character, spawnPos, botRot);

        bot.OnInit();

        charDeactiveList.Remove(bot);
        charActiveList.Add(bot);
    }

    public void DeadCharacter(CharacterBase character) {

        character.OnDespawn();
        SimplePool.Despawn(character);

        charActiveList.Remove(character);
        charDeactiveList.Add(character);
    }

    public List<CharacterBase> GetActiveCharacterList() {
        return charActiveList;
    }
}
