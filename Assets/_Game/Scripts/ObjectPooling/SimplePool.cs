using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    private static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    public static void Preload(PoolUnit prefab, int amount, Transform parent) {

        if (prefab == null) {

            Debug.LogError("Preafab is empty!!!");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.poolType) || poolInstance[prefab.poolType] == null) {

            Pool p = new Pool();
            p.PreLoad(prefab, amount, parent);
            poolInstance[prefab.poolType] = p;
        }

    }

    public static T Spawn<T>(PoolType poolType, Vector3 spawnPos, Quaternion rot) where T : PoolUnit {

        if (!poolInstance.ContainsKey(poolType)) {

            Debug.LogError($"{poolType} is not preload!!!");
            return null;
        }

        return poolInstance[poolType].Spawn(spawnPos, rot) as T;
    }

    public static void Despawn(PoolUnit unit) {

        if (!poolInstance.ContainsKey(unit.poolType)) {
            Debug.LogError($"{unit.poolType} is not preload!!!");
        }

        poolInstance[unit.poolType].Depsawn(unit);
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