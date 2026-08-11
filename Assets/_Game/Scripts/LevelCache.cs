using System.Collections.Generic;
using UnityEngine;

public static class LevelCache<TKey, TValue> where TKey : Component where TValue : MonoBehaviour
{
    private static readonly Dictionary<TKey, TValue> cacheDict = new Dictionary<TKey, TValue>();

    public static TValue GetValueWithKey(TKey key) {

        if (key == null) { return null; }

        if (!cacheDict.ContainsKey(key)) {

            cacheDict.Add(key, key.GetComponent<TValue>());
        }

        return cacheDict[key];
    }

    public static void ClearCacheDict() {
        cacheDict.Clear();
    }
}
