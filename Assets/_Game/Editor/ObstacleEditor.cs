using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(Obstacle))]
public class ObstacleEditor : Editor
{
    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        Obstacle obstacle = (Obstacle)target;
         
        if (GUILayout.Button("Add Obstacle Component Require")) {

            if (obstacle.GetComponent<BoxCollider>() == null) {
                Undo.AddComponent<BoxCollider>(obstacle.gameObject);
            }

            if (obstacle.GetComponent<NavMeshObstacle>() == null) {
                Undo.AddComponent<NavMeshObstacle>(obstacle.gameObject);
                obstacle.GetComponent<NavMeshObstacle>().carving = true;
            }
        }
    }
}
