using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject {

    [SerializeField] private List<LevelBase> levelBaseList;

    public LevelBase GetLeveLByIndex(int levelIndex) {

        return levelBaseList[levelIndex];
    }
}
