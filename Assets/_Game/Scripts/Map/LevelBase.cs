using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    [SerializeField] private Transform spawnPlayerPoint;

    public void OnInit() {


    }

    public void OnGamePlaying() {

    }

    public void OnDespawn() {

    }

    //public bool TryGetRandomSpawnPoint(out Vector3 spawnPos) {

    //    spawnPos = Vector3.zero;

    //    if (navMeshSurface.navMeshData == null) return false;

    //    Bounds bounds = navMeshSurface.navMeshData.sourceBounds;

    //    NavMeshQueryFilter filter = new NavMeshQueryFilter {
    //        agentTypeID = navMeshSurface.agentTypeID,
    //        areaMask = NavMesh.AllAreas
    //    };

    //    const int maxTry = 10;

    //    for (int i = 0; i < maxTry; i++) {

    //        float randomX = Random.Range(bounds.min.x, bounds.max.x);
    //        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
    //        Vector3 randomPos = new Vector3(randomX, bounds.center.y, randomZ);

    //        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, filter)) {

    //            spawnPos = hit.position;
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    public bool TryGetRandomSpawnPoint(out Vector3 spawnPos) {

        spawnPos = Vector3.zero;

        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        if (navMeshData.indices.Length < 3) return false;

        int triangleCount = navMeshData.indices.Length / 3;
        int triangleIndex = Random.Range(0, triangleCount) * 3;

        Vector3 a = navMeshData.vertices[navMeshData.indices[triangleIndex]];
        Vector3 b = navMeshData.vertices[navMeshData.indices[triangleIndex + 1]];
        Vector3 c = navMeshData.vertices[navMeshData.indices[triangleIndex + 2]];

        float r1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;

        spawnPos =
            (1f - r1) * a +
            r1 * (1f - r2) * b +
            r1 * r2 * c;

        return true;
    }

    public Vector3 GetSpawnPlayerPoint() {
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
