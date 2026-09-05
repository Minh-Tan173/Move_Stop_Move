using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(Obstacle))]
public class ObstacleEditor : Editor
{
    private const string FADE_MAT_PATH =
        "Assets/_Game/Shader/Material/TransparentObject_MAT.mat";


    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Obstacle obstacle = (Obstacle)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Auto Setup Obstacle")) {
            SetupObstacle(obstacle);
        }
    }


    private void SetupObstacle(Obstacle obstacle) {
        GameObject obj = obstacle.gameObject;


        // Add BoxCollider
        if (obj.GetComponent<BoxCollider>() == null) {
            Undo.AddComponent<BoxCollider>(obj);
        }


        // Add NavMeshObstacle
        NavMeshObstacle navObstacle =
            obj.GetComponent<NavMeshObstacle>();

        if (navObstacle == null) {
            navObstacle = Undo.AddComponent<NavMeshObstacle>(obj);
        }

        navObstacle.carving = true;


        // Get MeshRenderer
        MeshRenderer renderer =
            obj.GetComponent<MeshRenderer>();

        if (renderer == null) {
            Debug.LogError($"{obj.name} has no MeshRenderer");
            return;
        }


        SerializedObject so = new SerializedObject(obstacle);


        SerializedProperty meshRenderer =
            so.FindProperty("meshRenderer");

        SerializedProperty baseMAT =
            so.FindProperty("baseMAT");

        SerializedProperty fadeMAT =
            so.FindProperty("fadeMAT");


        // Assign MeshRenderer
        meshRenderer.objectReferenceValue = renderer;


        // Assign base material
        if (renderer.sharedMaterial != null) {
            baseMAT.objectReferenceValue =
                renderer.sharedMaterial;
        }


        // Assign fade material
        Material fadeMaterial =
            AssetDatabase.LoadAssetAtPath<Material>(FADE_MAT_PATH);


        if (fadeMaterial != null) {
            fadeMAT.objectReferenceValue = fadeMaterial;
        }
        else {
            Debug.LogError(
                $"Cannot find material: {FADE_MAT_PATH}"
            );
        }


        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(obstacle);
        EditorUtility.SetDirty(navObstacle);
    }
}
