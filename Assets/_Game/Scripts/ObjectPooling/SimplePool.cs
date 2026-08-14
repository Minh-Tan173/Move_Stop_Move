using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    private static Dictionary<PoolUnit, Pool> poolInstance = new Dictionary<PoolUnit, Pool>();

    public static void Preload(PoolUnit prefab, int amount, Transform parent) {

        if (prefab == null) {

            Debug.LogError("Preafab is empty!!!");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.prefabKey) || poolInstance[prefab.prefabKey] == null) {

            Pool p = new Pool();
            p.PreLoad(prefab, amount, parent);
            poolInstance[prefab.prefabKey] = p;
        }
        else {
            poolInstance[prefab].PreLoad(prefab, amount, parent);
        }

    }

    public static T Spawn<T>(T prefab, Vector3 spawnPos, Quaternion rot) where T : PoolUnit {

        if (!poolInstance.ContainsKey(prefab)) {

            Debug.LogError($"{prefab.name} is not preload!!!");
            return null;
        }

        return poolInstance[prefab].Spawn(spawnPos, rot) as T;
    }

    public static void Despawn(PoolUnit unit) {

        PoolUnit prefabKey = unit.prefabKey;

        if (prefabKey == null || !poolInstance.ContainsKey(unit.prefabKey)) {
            Debug.LogError($"{unit.prefabKey.name} is not preload!!!");
        }

        poolInstance[prefabKey].Depsawn(unit);
    }

}

public class Pool {

    private Transform parent;
    private PoolUnit prefab;

    // Game obj in Pool
    private Queue<PoolUnit> inactives = new Queue<PoolUnit>();

    // Game obj is using
    private List<PoolUnit> actives = new List<PoolUnit>();

    // Init pool
    public void PreLoad(PoolUnit prefab, int amount, Transform parent) {

        this.parent = parent;
        this.prefab = prefab;

        for (int i = 0; i < amount; i++) {

            Depsawn(Spawn(Vector3.zero, Quaternion.identity));
        }

    }

    // Get obj from pool
    public PoolUnit Spawn(Vector3 spawnPos, Quaternion rot) {

        PoolUnit unit;

        if (inactives.Count <= 0) {

            unit = GameObject.Instantiate(prefab, parent);
            unit.prefabKey = prefab;
        }
        else {

            unit = inactives.Dequeue();
        }

        unit.UnitTF.SetPositionAndRotation(spawnPos, rot);
        actives.Add(unit);
        unit.gameObject.SetActive(true);

        return unit;
    }

    // Set element back to pool
    public void Depsawn(PoolUnit unit) {

        if (unit != null && unit.gameObject.activeSelf) {

            actives.Remove(unit);
            inactives.Enqueue(unit);
            unit.gameObject.SetActive(false);
        }
    }

    // Collect all element are using back to pool
    public void CollectAll() {

        while (actives.Count > 0) {

            Depsawn(actives[0]);
        }
    }

    // Destroy all element in pool
    public void Release() {

        CollectAll();

        while (inactives.Count > 0) {

            GameObject.Destroy(inactives.Dequeue().gameObject);
        }

        inactives.Clear();
    }
}