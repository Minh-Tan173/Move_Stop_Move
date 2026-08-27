using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform parent;

    private const string UI_PREFABS_PATH = "UI/";

    private Dictionary<System.Type, UICanvas> canvasActiveDict = new Dictionary<System.Type, UICanvas>();
    private Dictionary<System.Type, UICanvas> canvasPrefab = new Dictionary<System.Type, UICanvas>();

    private void Awake() {

        UICanvas[] prefabArray = Resources.LoadAll<UICanvas>(UI_PREFABS_PATH);

        for (int i = 0; i < prefabArray.Length; i++) {

            canvasPrefab.Add(prefabArray[i].GetType(), prefabArray[i]);
        }

    }

    private T GetUIPrefab<T>() where T : UICanvas {

        return canvasPrefab[typeof(T)] as T;
    }

    // Open canvas
    public T OpenUI<T>() where T : UICanvas {

        T canvas = GetUI<T>();

        if (!canvas.gameObject.activeSelf) {

            canvas.gameObject.SetActive(true);
        }

        canvas.SetUp();
        canvas.Open();

        return canvas as T;
    }

    // Close canvas after time
    public void CloseUI<T>(float time) where T : UICanvas {

        if (IsUILoaded<T>()) {

            canvasActiveDict[typeof(T)].CloseUI(time);
        }

    }

    // Close canvas directly
    public void CloseUIDirectly<T>() where T : UICanvas {

        if (IsUILoaded<T>()) {

            canvasActiveDict[typeof(T)].CloseDirectly();
        }
    }
    public void CloseAllUI() {

        foreach (var canvas in canvasActiveDict) {

            if (canvas.Value != null && canvas.Value.gameObject.activeSelf) {

                canvas.Value.CloseUI(0f);
            }
        }
    }

    // Check if Canvas is loaded done
    public bool IsUILoaded<T>() where T : UICanvas {

        return canvasActiveDict.ContainsKey(typeof(T)) && canvasActiveDict[typeof(T)] != null;
    }

    // Check if Canvas is Opened
    public bool IsUIOpened<T>() where T : UICanvas {

        return IsUILoaded<T>() && canvasActiveDict[typeof(T)].gameObject.activeSelf;
    }

    public T GetUI<T>() where T : UICanvas {

        if (!IsUILoaded<T>()) {

            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasActiveDict[typeof(T)] = canvas;

        }

        return canvasActiveDict[typeof(T)] as T;
    }
}
