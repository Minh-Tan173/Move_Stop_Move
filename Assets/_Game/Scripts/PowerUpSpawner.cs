
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class PowerUpSpawner : PoolUnit
{
    [Header("Ref")]
    [SerializeField] private PowerUpSpawnerVisual spawnerVisual;

    [Header("Life Time")]
    [SerializeField] private float lifeDuration = 10f;
    [SerializeField] private float shrinkDuration = 0.3f;

    [Header("Spawn")]
    [SerializeField] private int totalPowerUpSpawn;
    [SerializeField] private float safeRadius = 3f;
    [SerializeField] private int maxTry = 10;

    [Header("Booster Prefab")]
    [SerializeField] private List<TimedPowerUp> timedPowerUpPrefabList;
    [SerializeField] private List<ChargePowerUp> chargePowerUpPrefabList;

    private List<PowerUpBase> powerUpActiveList = new List<PowerUpBase>();

    private IEnumerator IESpawnerLifeTime() {

        yield return new WaitForSeconds(lifeDuration);

        yield return spawnerVisual.IEShrink(shrinkDuration);

        // Depsawn UnUsed power up
        for (int i = powerUpActiveList.Count - 1; i >= 0; i--) {

            if (powerUpActiveList[i] == null) { continue; }

            DespawnPowerUp(powerUpActiveList[i]);
        }

        SimplePool.Despawn(this);
    }

    private bool TryGetSafeSpawnPoint(out Vector3 safeSpawnPos) {

        LevelBase currentLevel = LevelManager.Instance.GetCurrentLeveL();
        safeSpawnPos = Vector3.zero;

        for (int i = 0; i < maxTry; i++) {

            if (currentLevel.TryGetRandomSpawnPoint(out Vector3 intendedSpawnPos)) {

                bool isSafe = true;

                List<CharacterBase> characterActiveList = CharacterManager.Instance.GetActiveCharacterList();

                foreach (CharacterBase character in characterActiveList) {
                    // Check if any character (bot / player) is in safeRadius

                    float distanceToSpawner = (character.UnitTF.position - intendedSpawnPos).sqrMagnitude;

                    if (distanceToSpawner < safeRadius * safeRadius) {

                        isSafe = false;
                        break;
                    }
                }

                if (isSafe) {
                    // If intended spawn pos dont having any character inside safe zone

                    safeSpawnPos = intendedSpawnPos;

                    return true;
                }
            }
        }

        return false;
    }

    private void SpawnPowerUp(PowerUpBase powerUpPrefab, Vector3 spawnPos) {

        PowerUpBase powerUp = SimplePool.Spawn<PowerUpBase>(powerUpPrefab, spawnPos, Quaternion.identity);
        powerUp.OnInit(this);

        powerUpActiveList.Add(powerUp);
    }

    private void DespawnPowerUp(PowerUpBase powerUp) {

        SimplePool.Despawn(powerUp);
    }

    public void OnInit() {

        powerUpActiveList.Clear();

        StartCoroutine(IESpawnerLifeTime());

        // Spawner appear on Field
        if (!TryGetSafeSpawnPoint(out Vector3 spawnPos)) {

            LevelManager.Instance.GetCurrentLeveL().TryGetRandomSpawnPoint(out spawnPos);
        }

        this.UnitTF.position = spawnPos;

        // Show Visual
        spawnerVisual.OnInit();
    }

    public void SpawnTimedPowerUP() {

        for (int i = 0; i < totalPowerUpSpawn; i++) {

            int powerUpIndex = Random.Range(0, timedPowerUpPrefabList.Count);
            SpawnPowerUp(timedPowerUpPrefabList[powerUpIndex], this.UnitTF.position);
        }
    }

    public void SpawnChargePowerUp() {

        if (chargePowerUpPrefabList.Count == 0) { return; }  // TODO: ADD CHARGE POWER UP IN THE FUTURE

        for (int i = 0; i < totalPowerUpSpawn; i++) {

            SpawnPowerUp(chargePowerUpPrefabList[i], this.UnitTF.position);
        }
    }

    public void UnRegisterPowerUp(PowerUpBase powerUp) {

        powerUpActiveList.Remove(powerUp);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.yellow;

        int segment = 40;

        Vector3 center = transform.position;
        Vector3 previousPoint = center + Vector3.forward * safeRadius;

        for (int i = 1; i <= segment; i++) {

            float angle = i * Mathf.PI * 2f / segment;

            Vector3 currentPoint = center + new Vector3(Mathf.Sin(angle) * safeRadius, 0f, Mathf.Cos(angle) * safeRadius);

            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
#endif
}
