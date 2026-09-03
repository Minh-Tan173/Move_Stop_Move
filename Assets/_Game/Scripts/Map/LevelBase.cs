using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Header("Spawn Player")]
    [SerializeField] private Transform spawnPlayerPoint;

    [Header("Spawn Bot")]
    [SerializeField] private float minDistanceToPlayer = 8f;
    [SerializeField] private int maxTry = 20;

    private bool IsPointOnScreen(Vector3 worldPos) {

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);

        if (viewportPos.z <= 0f) { return false; }

        if (viewportPos.x < 0f || viewportPos.x > 1f) { return false; }

        if (viewportPos.y < 0f || viewportPos.y > 1f) { return false; }

        return true;

    }

    public void OnInit() {


    }

    public void OnDespawn() {

    }

    public bool TryGetRandomSpawnPoint(out Vector3 spawnPos) {

        Player player = CharacterManager.Instance.GetPlayer();

        Vector3 playerPos = player.UnitTF.position;

        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int triangleCount = navMeshData.indices.Length / 3;


        for (int i = 0; i < maxTry; i++) {

            int triangleIndex = Random.Range(0, triangleCount) * 3;

            Vector3 a = navMeshData.vertices[navMeshData.indices[triangleIndex]];
            Vector3 b = navMeshData.vertices[navMeshData.indices[triangleIndex + 1]];
            Vector3 c = navMeshData.vertices[navMeshData.indices[triangleIndex + 2]];

            float r1 = Mathf.Sqrt(Random.value);
            float r2 = Random.value;

            Vector3 randomPos = (1f - r1) * a + r1 * (1f - r2) * b + r1 * r2 * c;

            if (Vector3.Distance(randomPos, playerPos) < minDistanceToPlayer) { continue; }


            if (IsPointOnScreen(randomPos)) { continue; }

            spawnPos = randomPos;
            return true;
        }

        spawnPos = playerPos + Random.insideUnitSphere * 10f;
        spawnPos.y = playerPos.y;

        return true;
    }

    public Vector3 GetSpawnPlayerPoint() {

        if (NavMesh.SamplePosition(spawnPlayerPoint.position, out NavMeshHit hitGround, maxDistance: 10f, NavMesh.AllAreas)) {

            float offset = CharacterManager.Instance.GetPlayerPrefab().GetComponent<CapsuleCollider>().height / 2f;

            return hitGround.position + Vector3.up * offset;
        }

        return spawnPlayerPoint.position;
    }

    public static LevelBase SpawnLevel(LevelBase prefab) {

        Transform levelTransform = Instantiate(prefab.transform);
        LevelBase levelBase = levelTransform.GetComponent<LevelBase>();

        return levelBase;
    }

    public static void DestroyLevel(LevelBase levelBase) {

        Destroy(levelBase.transform.gameObject);
    }
}
