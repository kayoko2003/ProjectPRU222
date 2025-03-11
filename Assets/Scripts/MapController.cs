using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius = 50f;
    public LayerMask terrainMask;
    public GameObject currentChunk;

    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>();
    private List<GameObject> spawnedChunks = new List<GameObject>();
    private Queue<GameObject> chunkPool = new Queue<GameObject>();

    private int maxChunks = 150;
    public float maxOpDist = 200f; // Tăng để tránh chunk bị ẩn sớm
    public float bufferZone = 50f; // Vùng đệm trước khi tắt chunk
    public int maxSpawnPerFrame = 6;
    public float chunkSize = 10f;

    void Start()
    {
        for (int i = 0; i < maxChunks; i++)
        {
            int rand = Random.Range(0, terrainChunks.Count);
            GameObject chunk = Instantiate(terrainChunks[rand], Vector3.zero, Quaternion.identity);
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
        }
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
        ConstrainPlayer();
    }

    void ChunkChecker()
    {
        if (!currentChunk) return;

        string[] directionNames = { "Right", "Left", "Up", "Down",
                                "Right Up", "Right Down", "Left Up", "Left Down" };

        int spawnedCount = 0;

        foreach (string dirName in directionNames)
        {
            if (spawnedCount >= maxSpawnPerFrame) break;

            Transform checkPoint = currentChunk.transform.Find(dirName);
            if (checkPoint == null) continue;

            Vector3 spawnPos = checkPoint.position;

            // Giới hạn trong phạm vi 100x100 (từ -50 đến 50)
            if (Mathf.Abs(spawnPos.x) > 120 || Mathf.Abs(spawnPos.y) > 120) continue;

            if (!spawnedPositions.Contains(spawnPos) &&
                !Physics2D.OverlapBox(spawnPos, new Vector2(chunkSize, chunkSize), 0, terrainMask))
            {
                SpawnChunk(spawnPos);
                spawnedCount++;
            }
        }
    }

    void SpawnChunk(Vector3 position)
    {
        if (spawnedPositions.Contains(position)) return;

        GameObject chunk;
        if (chunkPool.Count > 0)
        {
            chunk = chunkPool.Dequeue();
            chunk.transform.position = position;
            chunk.SetActive(true);
        }
        else
        {
            int rand = Random.Range(0, terrainChunks.Count);
            chunk = Instantiate(terrainChunks[rand], position, Quaternion.identity);
        }

        spawnedChunks.Add(chunk);
        spawnedPositions.Add(position);

        if (spawnedChunks.Count > maxChunks)
        {
            RemoveFarthestChunk();
        }
    }

    void RemoveFarthestChunk()
    {
        float maxDistance = 0;
        GameObject farthestChunk = null;

        foreach (GameObject chunk in spawnedChunks)
        {
            float distance = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestChunk = chunk;
            }
        }

        if (farthestChunk != null)
        {
            spawnedChunks.Remove(farthestChunk);
            spawnedPositions.Remove(farthestChunk.transform.position);
            farthestChunk.SetActive(false);
            chunkPool.Enqueue(farthestChunk);
        }
    }

    void ChunkOptimizer()
    {
        foreach (GameObject chunk in spawnedChunks)
        {
            float dist = Vector3.Distance(player.transform.position, chunk.transform.position);

            // Chỉ tắt chunk khi nó ra khỏi vùng đệm
            if (dist > maxOpDist + bufferZone)
            {
                chunk.SetActive(false);
            }
            else if (dist <= maxOpDist)
            {
                chunk.SetActive(true);
            }
        }
    }

    void ConstrainPlayer()
    {
        if (player != null)
        {
            Vector3 pos = player.transform.position;

            // Giới hạn trong phạm vi (-50 đến 50)
            pos.x = Mathf.Clamp(pos.x, -100, 100);
            pos.y = Mathf.Clamp(pos.y, -100, 100);

            player.transform.position = pos;
        }
    }
}
