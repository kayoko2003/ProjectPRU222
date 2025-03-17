using System.Collections.Generic;
using UnityEngine;

public class PropRandom : MonoBehaviour
{
    public List<GameObject> propSpawnPoints; // Điểm spawn prop
    public List<GameObject> quicksandSpawnPoints; // Điểm spawn cát lún
    public List<GameObject> propPrefabs; // Các loại prop
    public GameObject quicksandPrefab; // Prefab cát lún
    [Range(0f, 1f)] public float quicksandSpawnChance = 0.2f; // 20% spawn cát lún

    void Start()
    {
        SpawnProps();
        SpawnQuicksand();
    }

    void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            Instantiate(propPrefabs[rand], sp.transform.position, Quaternion.identity, sp.transform);
        }
    }

    void SpawnQuicksand()
    {
        List<GameObject> availablePoints = new List<GameObject>(quicksandSpawnPoints);

        // Lọc ra các điểm không có prop
        foreach (GameObject sp in propSpawnPoints)
        {
            availablePoints.Remove(sp);
        }

        if (availablePoints.Count > 0)
        {
            bool hasSpawnedQuicksand = false;
            foreach (GameObject sp in availablePoints)
            {
                if (Random.value <= quicksandSpawnChance)
                {
                    Instantiate(quicksandPrefab, sp.transform.position, Quaternion.identity, sp.transform);
                    hasSpawnedQuicksand = true;
                    break; // Nếu muốn spawn nhiều cát lún, bỏ dòng này
                }
            }

            // Đảm bảo ít nhất 1 cát lún xuất hiện
            if (!hasSpawnedQuicksand)
            {
                int randIndex = Random.Range(0, availablePoints.Count);
                Instantiate(quicksandPrefab, availablePoints[randIndex].transform.position, Quaternion.identity, availablePoints[randIndex].transform);
            }
        }
    }
}
