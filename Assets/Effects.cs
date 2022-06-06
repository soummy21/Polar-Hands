using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject BloodSplatDirectionalAnimated,
        BloodPoolGrowing, NukeVerticalExplosionFire, CartoonyPunchLight;
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject g = Instantiate(pool.prefab);
                g.SetActive(false);
                objectPool.Enqueue(g);
            }
            poolDictionary.Add(pool.tag, objectPool);

        }
    }

    public GameObject spawnFromPool(string tag, Vector3 pos, Quaternion rot) {
        if (!poolDictionary.ContainsKey(tag)) {
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(false);
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

}
