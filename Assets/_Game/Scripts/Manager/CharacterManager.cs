using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : Singleton<CharacterManager>
{
    [Header("Spawn bot")]
    [SerializeField] private Transform pooling;
    [SerializeField] private int maxBotCountInLevel = 50;
    [SerializeField] private int maxBotCountRuntime = 10;

    private List<Bot> botActiveList = new List<Bot>();
    private List<Bot> botDeactiveList = new List<Bot>();

    public void OnInit() {


    }

    public void OnDespawn() {

        botActiveList.Clear();
        botDeactiveList.Clear();
    }

    private void Update() {

        int botActiveTotal = botActiveList.Count;
        int botDeactiveTotal = botDeactiveList.Count;
        int botTotal = botActiveTotal + botActiveTotal;

        if (botTotal < maxBotCountInLevel) {
            // Total character can't over 50 (includes player)

            if (botActiveTotal < maxBotCountRuntime) {
                // If not enough bot on field

                //SpawnBot()
            }
        }
    }

    private void SpawnPlayer(Vector3 spawnPos) {

        CharacterBase player = SimplePool.Spawn<CharacterBase>(PoolType.Character, spawnPos, Quaternion.identity);

        player.OnInit();
    }
    
    private void DespawnPlayer(Player player) {

        player.OnDespawn();
        SimplePool.Despawn(player);
    }

    private void SpawnBot(Vector3 spawnPos) {

        float randomYRot = Random.Range(0f, 180f);
        Quaternion botRot = Quaternion.Euler(0f, randomYRot, 0f);
        CharacterBase bot = SimplePool.Spawn<CharacterBase>(PoolType.Character, spawnPos, botRot);

        bot.OnInit();

        botActiveList.Add(bot as Bot);
    }

    public void DespawnBot(Bot bot) {

        bot.OnDespawn();
        SimplePool.Despawn(bot);

        botDeactiveList.Add(bot);
    }
}
