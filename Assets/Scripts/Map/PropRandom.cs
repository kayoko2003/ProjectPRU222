using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PropRandom : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[rand], sp.transform.position, Quaternion.identity);
            prop.transform.parent = sp.transform;
        }

    }
}
