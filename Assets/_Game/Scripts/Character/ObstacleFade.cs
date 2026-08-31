using System.Collections.Generic;
using UnityEngine;

public class ObstacleFade : MonoBehaviour
{
    [SerializeField] private Collider fadeZoneColl;
    private readonly Dictionary<Collider, Obstacle> obstacleDict = new();
    private readonly HashSet<Collider> fadedObstacleSet = new();

    private void OnTriggerEnter(Collider objectColl) {

        Obstacle obstacle = LevelCache<Collider, Obstacle>.GetValueWithKey(objectColl);

        if (obstacle == null) return;

        obstacleDict[objectColl] = obstacle;

        if (fadedObstacleSet.Add(objectColl)) {
            obstacle.OnFadeMAT();
        }
    }

    private void Update() {

        if (!LevelManager.Instance.IsGamePlaying()) { return; }
        if (obstacleDict.Count == 0) return;

        CheckObstacle();
    }

    private void CheckObstacle() {

        Bounds fadeBounds = fadeZoneColl.bounds;
        Vector3 fadeCenter = fadeBounds.center;

        float fadeRadius = Mathf.Max(fadeBounds.extents.x, fadeBounds.extents.z);

        foreach (var pair in obstacleDict) {

            Collider objectColl = pair.Key;
            Obstacle obstacle = pair.Value;

            Bounds objectBounds = objectColl.bounds;

            float objectRadius = Mathf.Max(objectBounds.extents.x, objectBounds.extents.z);

            float deltaX = objectBounds.center.x - fadeCenter.x;
            float deltaZ = objectBounds.center.z - fadeCenter.z;

            float distanceSqr = deltaX * deltaX + deltaZ * deltaZ;

            float maxDistance = fadeRadius + objectRadius;

            bool isInside = distanceSqr <= maxDistance * maxDistance;

            if (isInside) {
                // Base → Fade

                if (fadedObstacleSet.Add(objectColl)) {
                    obstacle.OnFadeMAT();
                }
            }
            else {
                // Fade → Base

                if (fadedObstacleSet.Remove(objectColl)) {
                    obstacle.OnBaseMAT();
                }
            }
        }
    }
}
