using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;

    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    public List<GameObject> spawnedChunks;
    GameObject lastestChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 dir)
    {
        dir = dir.normalized;

        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if(dir.y > 0.5f)
            {
                return dir.x > 0 ? "Right Up" : "Left Up";   
            }else if(dir.y < -0.5f)
            {
                return dir.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                return dir.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            if(dir.x > 0.5)
            {
                return dir.y > 0 ? "Right Up" : "Right Down";
            }
            else if(dir.x < -0.5)
            {
                return dir.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                return dir.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        lastestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(lastestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;   
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else { 
                chunk.SetActive(true);
            }
        }
    }
}
