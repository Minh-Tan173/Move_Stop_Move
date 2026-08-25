using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UICanvas), true)]
public class UICanvasEditor : Editor
{
    private SerializedProperty uiElementAnims;

    private void OnEnable() {
        uiElementAnims = serializedObject.FindProperty("uiElementAnims");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();


        if (GUILayout.Button("Add UI Elements into animation List")) {

            UICanvas canvas = (UICanvas)target;

            UIElementAnim[] elements = canvas.GetComponentsInChildren<UIElementAnim>(true);


            Undo.RecordObject(canvas, "Add UI Elements Animation");


            uiElementAnims.arraySize = elements.Length;

            for (int i = 0; i < elements.Length; i++) {
                uiElementAnims.GetArrayElementAtIndex(i).objectReferenceValue = elements[i];
            }

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(canvas);
        }
    }
}
