using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class AnimationEventFrameTool : EditorWindow {
    private UnityEngine.Object modelAsset;

    private ModelImporter modelImporter;
    private ModelImporterClipAnimation[] importerClips;
    private AnimationClip[] animationClips;

    private string[] clipNames;
    private int selectedClipIndex;

    private int eventFrame;

    private MonoScript receiverScript;
    private string[] functionNames;
    private int selectedFunctionIndex;

    private Vector2 eventScroll;

    [MenuItem("Tools/Animation/Animation Event Frame Tool")]
    private static void OpenWindow() {

        GetWindow<AnimationEventFrameTool>("Animation Event Tool");
    }

    private void OnGUI() {

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "FBX Animation Event",
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space();

        DrawModelField();

        if (modelImporter == null) {
            return;
        }

        EditorGUILayout.Space();

        DrawClipField();

        EditorGUILayout.Space();

        DrawFrameField();

        EditorGUILayout.Space();

        DrawReceiverField();

        EditorGUILayout.Space();

        DrawAddEventButton();

        EditorGUILayout.Space(15);

        DrawExistingEvents();
    }

    private void DrawModelField() {

        UnityEngine.Object newModelAsset =
            EditorGUILayout.ObjectField(
                "FBX",
                modelAsset,
                typeof(UnityEngine.Object),
                false
            );

        if (newModelAsset != modelAsset) {

            modelAsset = newModelAsset;

            LoadModel();
        }
    }

    private void DrawClipField() {

        if (clipNames == null || clipNames.Length == 0) {

            EditorGUILayout.HelpBox(
                "No Animation Clip found.",
                MessageType.Warning
            );

            return;
        }

        int newClipIndex =
            EditorGUILayout.Popup(
                "Clip",
                selectedClipIndex,
                clipNames
            );

        if (newClipIndex != selectedClipIndex) {

            selectedClipIndex = newClipIndex;
            eventFrame = 0;
        }

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        AnimationClip animationClip =
            GetSelectedAnimationClip();

        if (importerClip == null) {
            return;
        }

        int localEndFrame = GetLocalEndFrame(importerClip);

        EditorGUILayout.LabelField(
            "Source Frame Range",
            $"{importerClip.firstFrame:0} - {importerClip.lastFrame:0}"
        );

        EditorGUILayout.LabelField(
            "Local Frame Range",
            $"0 - {localEndFrame}"
        );

        if (animationClip != null) {

            EditorGUILayout.LabelField(
                "Frame Rate",
                animationClip.frameRate.ToString("0.##")
            );

            EditorGUILayout.LabelField(
                "Length",
                animationClip.length.ToString("0.###") + "s"
            );
        }
    }

    private void DrawFrameField() {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        AnimationClip animationClip =
            GetSelectedAnimationClip();

        if (importerClip == null) {
            return;
        }

        int localEndFrame =
            GetLocalEndFrame(importerClip);

        eventFrame =
            EditorGUILayout.IntField(
                "Event Frame",
                eventFrame
            );

        eventFrame =
            Mathf.Clamp(
                eventFrame,
                0,
                localEndFrame
            );

        float normalizedTime =
            GetNormalizedTime(
                eventFrame,
                importerClip
            );

        float sourceFrame =
            importerClip.firstFrame + eventFrame;

        EditorGUILayout.LabelField(
            "Source Frame",
            sourceFrame.ToString("0")
        );

        EditorGUILayout.LabelField(
            "Normalized Time",
            normalizedTime.ToString("0.0000")
        );

        if (animationClip != null) {

            float seconds =
                normalizedTime * animationClip.length;

            EditorGUILayout.LabelField(
                "Event Time",
                seconds.ToString("0.000") + "s"
            );
        }
    }

    private void DrawReceiverField() {

        MonoScript newReceiverScript =
            (MonoScript)EditorGUILayout.ObjectField(
                "Receiver Script",
                receiverScript,
                typeof(MonoScript),
                false
            );

        if (newReceiverScript != receiverScript) {

            receiverScript = newReceiverScript;

            LoadFunctions();
        }

        if (functionNames == null ||
            functionNames.Length == 0) {

            if (receiverScript != null) {

                EditorGUILayout.HelpBox(
                    "No public void function with zero parameters found.",
                    MessageType.Warning
                );
            }

            return;
        }

        selectedFunctionIndex =
            EditorGUILayout.Popup(
                "Function",
                selectedFunctionIndex,
                functionNames
            );
    }

    private void DrawAddEventButton() {

        GUI.enabled =
            GetSelectedImporterClip() != null &&
            functionNames != null &&
            functionNames.Length > 0;

        if (GUILayout.Button(
            "Add Animation Event",
            GUILayout.Height(32)
        )) {

            AddAnimationEvent();
        }

        GUI.enabled = true;
    }

    private void DrawExistingEvents() {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        if (importerClip == null) {
            return;
        }

        EditorGUILayout.LabelField(
            "Existing Events",
            EditorStyles.boldLabel
        );

        AnimationEvent[] events =
            importerClip.events;

        if (events == null ||
            events.Length == 0) {

            EditorGUILayout.HelpBox(
                "No Animation Events in this clip.",
                MessageType.Info
            );

            return;
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            $"Total: {events.Length}"
        );

        if (GUILayout.Button(
            "Clear All",
            GUILayout.Width(80)
        )) {

            bool confirm =
                EditorUtility.DisplayDialog(
                    "Clear Animation Events",
                    $"Remove all events from clip '{importerClip.name}'?",
                    "Clear All",
                    "Cancel"
                );

            if (confirm) {

                ClearAllEvents();

                GUIUtility.ExitGUI();
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        eventScroll =
            EditorGUILayout.BeginScrollView(
                eventScroll,
                GUILayout.MaxHeight(260)
            );

        for (int i = 0; i < events.Length; i++) {

            DrawExistingEvent(
                events[i],
                i,
                importerClip
            );
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawExistingEvent(
        AnimationEvent animationEvent,
        int eventIndex,
        ModelImporterClipAnimation importerClip
    ) {

        int frame =
            GetFrameFromNormalizedTime(
                animationEvent.time,
                importerClip
            );

        float sourceFrame =
            importerClip.firstFrame + frame;

        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            animationEvent.functionName,
            EditorStyles.boldLabel
        );

        if (GUILayout.Button(
            "Load",
            GUILayout.Width(50)
        )) {

            LoadExistingEvent(animationEvent);
        }

        if (GUILayout.Button(
            "Remove",
            GUILayout.Width(65)
        )) {

            RemoveEvent(eventIndex);

            GUIUtility.ExitGUI();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField(
            "Local Frame",
            frame.ToString()
        );

        EditorGUILayout.LabelField(
            "Source Frame",
            sourceFrame.ToString("0")
        );

        EditorGUILayout.LabelField(
            "Normalized",
            animationEvent.time.ToString("0.0000")
        );

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(3);
    }

    private void LoadExistingEvent(
        AnimationEvent animationEvent
    ) {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        if (importerClip == null) {
            return;
        }

        eventFrame =
            GetFrameFromNormalizedTime(
                animationEvent.time,
                importerClip
            );

        if (functionNames == null) {
            return;
        }

        int functionIndex =
            Array.IndexOf(
                functionNames,
                animationEvent.functionName
            );

        if (functionIndex >= 0) {

            selectedFunctionIndex =
                functionIndex;
        }
    }

    private void LoadModel(
        string preferredClipName = null
    ) {

        if (preferredClipName == null &&
            clipNames != null &&
            selectedClipIndex >= 0 &&
            selectedClipIndex < clipNames.Length) {

            preferredClipName =
                clipNames[selectedClipIndex];
        }

        modelImporter = null;
        importerClips = null;
        animationClips = null;
        clipNames = null;

        if (modelAsset == null) {
            return;
        }

        string path =
            AssetDatabase.GetAssetPath(modelAsset);

        modelImporter =
            AssetImporter.GetAtPath(path)
            as ModelImporter;

        if (modelImporter == null) {

            Debug.LogError(
                "Selected asset is not an FBX / Model."
            );

            return;
        }

        importerClips =
            modelImporter.clipAnimations;

        if (importerClips == null ||
            importerClips.Length == 0) {

            importerClips =
                modelImporter.defaultClipAnimations;
        }

        animationClips =
            AssetDatabase
                .LoadAllAssetsAtPath(path)
                .OfType<AnimationClip>()
                .Where(
                    clip =>
                        !clip.name.StartsWith("__preview__")
                )
                .ToArray();

        clipNames =
            importerClips
                .Select(clip => clip.name)
                .ToArray();

        selectedClipIndex = 0;

        if (!string.IsNullOrEmpty(preferredClipName)) {

            int index =
                Array.IndexOf(
                    clipNames,
                    preferredClipName
                );

            if (index >= 0) {

                selectedClipIndex = index;
            }
        }
    }

    private void LoadFunctions() {

        functionNames = null;
        selectedFunctionIndex = 0;

        if (receiverScript == null) {
            return;
        }

        Type type =
            receiverScript.GetClass();

        if (type == null) {
            return;
        }

        functionNames =
            type
                .GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly
                )
                .Where(method =>
                    !method.IsSpecialName &&
                    method.ReturnType == typeof(void) &&
                    method.GetParameters().Length == 0
                )
                .Select(method => method.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToArray();
    }

    private ModelImporterClipAnimation
        GetSelectedImporterClip() {

        if (importerClips == null ||
            importerClips.Length == 0) {

            return null;
        }

        if (selectedClipIndex < 0 ||
            selectedClipIndex >= importerClips.Length) {

            return null;
        }

        return importerClips[selectedClipIndex];
    }

    private AnimationClip
        GetSelectedAnimationClip() {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        if (importerClip == null ||
            animationClips == null) {

            return null;
        }

        return animationClips.FirstOrDefault(
            clip =>
                clip.name == importerClip.name
        );
    }

    private int GetLocalEndFrame(
        ModelImporterClipAnimation importerClip
    ) {

        return Mathf.RoundToInt(
            importerClip.lastFrame -
            importerClip.firstFrame
        );
    }

    private float GetNormalizedTime(
        int frame,
        ModelImporterClipAnimation importerClip
    ) {

        int localEndFrame =
            GetLocalEndFrame(importerClip);

        if (localEndFrame <= 0) {
            return 0f;
        }

        return Mathf.Clamp01(
            (float)frame / localEndFrame
        );
    }

    private int GetFrameFromNormalizedTime(
        float normalizedTime,
        ModelImporterClipAnimation importerClip
    ) {

        int localEndFrame =
            GetLocalEndFrame(importerClip);

        return Mathf.RoundToInt(
            Mathf.Clamp01(normalizedTime) *
            localEndFrame
        );
    }

    private void AddAnimationEvent() {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        if (importerClip == null) {
            return;
        }

        string clipName =
            importerClip.name;

        string functionName =
            functionNames[selectedFunctionIndex];

        float normalizedTime =
            GetNormalizedTime(
                eventFrame,
                importerClip
            );

        List<AnimationEvent> events =
            importerClip.events != null
                ? importerClip.events.ToList()
                : new List<AnimationEvent>();

        bool alreadyExists =
            events.Any(animationEvent => {

                int existingFrame =
                    GetFrameFromNormalizedTime(
                        animationEvent.time,
                        importerClip
                    );

                return
                    animationEvent.functionName ==
                    functionName &&
                    existingFrame == eventFrame;
            });

        if (alreadyExists) {

            Debug.LogWarning(
                $"Animation Event already exists | " +
                $"Clip: {clipName} | " +
                $"Frame: {eventFrame} | " +
                $"Function: {functionName}"
            );

            return;
        }

        AnimationEvent newEvent =
            new AnimationEvent();

        newEvent.functionName =
            functionName;

        newEvent.time =
            normalizedTime;

        events.Add(newEvent);

        importerClip.events =
            events
                .OrderBy(evt => evt.time)
                .ToArray();

        SaveImporterClip(
            importerClip,
            clipName
        );

        Debug.Log(
            $"Animation Event Added | " +
            $"Clip: {clipName} | " +
            $"Frame: {eventFrame} | " +
            $"Source Frame: " +
            $"{importerClip.firstFrame + eventFrame:0} | " +
            $"Function: {functionName}"
        );
    }

    private void RemoveEvent(int eventIndex) {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        if (importerClip == null ||
            importerClip.events == null) {

            return;
        }

        string clipName =
            importerClip.name;

        List<AnimationEvent> events =
            importerClip.events.ToList();

        if (eventIndex < 0 ||
            eventIndex >= events.Count) {

            return;
        }

        events.RemoveAt(eventIndex);

        importerClip.events =
            events.ToArray();

        SaveImporterClip(
            importerClip,
            clipName
        );
    }

    private void ClearAllEvents() {

        ModelImporterClipAnimation importerClip =
            GetSelectedImporterClip();

        if (importerClip == null) {
            return;
        }

        string clipName =
            importerClip.name;

        importerClip.events =
            Array.Empty<AnimationEvent>();

        SaveImporterClip(
            importerClip,
            clipName
        );
    }

    private void SaveImporterClip(
        ModelImporterClipAnimation importerClip,
        string clipName
    ) {

        importerClips[selectedClipIndex] =
            importerClip;

        modelImporter.clipAnimations =
            importerClips;

        modelImporter.SaveAndReimport();

        LoadModel(clipName);

        Repaint();
    }
}
