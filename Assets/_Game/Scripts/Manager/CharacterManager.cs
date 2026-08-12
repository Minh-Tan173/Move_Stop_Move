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

    public void OnInit() {


    }

    public void OnDespawn() {

        charActiveList.Clear();
        charDeactiveList.Clear();
    }

    private void Update() {

        int botActiveTotal = charActiveList.Count;
        int botDeactiveTotal = charDeactiveList.Count;
        int botTotal = botActiveTotal + botActiveTotal;

        if (botTotal <= maxBotCountInLevel) {
            // Total character can't over 50 (includes player)

            if (botActiveTotal < maxBotCountRuntime) {
                // If not enough character on field

                //SpawnBot()
            }
        }
    }

    private void SpawnPlayer(Vector3 spawnPos) {

        CharacterBase player = SimplePool.Spawn<CharacterBase>(PoolType.Character, spawnPos, Quaternion.identity);

        charActiveList.Add(player);

        player.OnInit();
    }
    
    private void DespawnPlayer(Player player) {

        player.OnDespawn();
        SimplePool.Despawn(player);

        charActiveList.Remove(player);
        charDeactiveList.Add(player);
    }

    private void SpawnBot(Vector3 spawnPos) {

        float randomYRot = Random.Range(0f, 180f);
        Quaternion botRot = Quaternion.Euler(0f, randomYRot, 0f);
        CharacterBase bot = SimplePool.Spawn<CharacterBase>(PoolType.Character, spawnPos, botRot);

        bot.OnInit();

        charActiveList.Add(bot);
    }

    public void DespawnBot(Bot bot) {

        bot.OnDespawn();
        SimplePool.Despawn(bot);

        charActiveList.Remove(bot);
        charDeactiveList.Add(bot);
    }

    public List<CharacterBase> GetActiveCharacterList() {
        return charActiveList;
    }
}
