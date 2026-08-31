using UnityEngine;

public class EventManager : Singleton<EventManager> {
    [Header("Spawn PowerUp Event")]
    [SerializeField] private PowerUpSpawner powerUpSpawnerPrefab;
    [SerializeField] private float powerUpSpawnEventDuration;

    private float elapsedPowerUpEvent;
    private bool hasSpawnedTimedPowerUp;

    public void OnInit() {

        elapsedPowerUpEvent = 0f;
        hasSpawnedTimedPowerUp = false;
    }

    private void Update() {

        if (!LevelManager.Instance.IsGamePlaying()) { return; }

        HandleEvent();
    }

    private void HandleEvent() {

        elapsedPowerUpEvent += Time.deltaTime;

        if (elapsedPowerUpEvent >= powerUpSpawnEventDuration) {

            if (!hasSpawnedTimedPowerUp) {
                // Spawn timed power up first

                PowerUpSpawner powerUpSpawner = SimplePool.Spawn<PowerUpSpawner>(powerUpSpawnerPrefab, Vector3.zero, Quaternion.identity);
                powerUpSpawner.OnInit();
                powerUpSpawner.SpawnTimedPowerUP();

            }
            else {
                // Spawn charge power up after timed power up

                PowerUpSpawner powerUpSpawner = SimplePool.Spawn<PowerUpSpawner>(powerUpSpawnerPrefab, Vector3.zero, Quaternion.identity);
                powerUpSpawner.OnInit();
                powerUpSpawner.SpawnChargePowerUp();
            }

            hasSpawnedTimedPowerUp = !hasSpawnedTimedPowerUp;
            elapsedPowerUpEvent -= powerUpSpawnEventDuration;

            // UI Visualize
            UIManager.Instance.GetUI<CanvasHUD>().ShowEventNoti();
        }
    }
}
