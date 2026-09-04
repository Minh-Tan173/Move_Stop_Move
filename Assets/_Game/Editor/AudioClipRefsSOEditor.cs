using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(AudioClipRefsSO))]
public class AudioClipRefsSOEditor : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();

        SerializedProperty list =
            serializedObject.FindProperty("sfxList");

        for (int i = 0; i < list.arraySize; i++) {
            SerializedProperty element =
                list.GetArrayElementAtIndex(i);

            SerializedProperty type =
                element.FindPropertyRelative("sfxType");

            SerializedProperty name =
                element.FindPropertyRelative("sfxName");

            name.stringValue =
                type.enumDisplayNames[type.enumValueIndex];
        }

        serializedObject.ApplyModifiedProperties();

        DrawDefaultInspector();

        if (GUI.changed) {
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
    }
}
