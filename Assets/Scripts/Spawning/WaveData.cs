using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Game Survive/Wave Data")]
public class WaveData : SpawnData
{
    [Header("Wave Data")]
    [Tooltip("If there are less than this number of enemies, we will keep spawning util we get there.")]
    [Min(0)] public int startingCount = 0;

    [Tooltip("How many anemies can this wave spawn at maximum ?")]
    [Min(1)] public uint totalSpawns = uint.MaxValue;

    [System.Flags] public enum ExitCondition { WaveDuration = 1, reachedTotalSpawns = 2}
    [Tooltip("Set the things that can trigger the end of this wave")]
    public ExitCondition exitCondition = (ExitCondition)1;
    
    [Tooltip("All enemies must be dead for the wave to advance.")]
    public bool mustKillAll = false;

    [HideInInspector] public uint spawntCount;

    public override GameObject[] GetSpawns(int totalEnemies = 0)
    {
        int count = Random.Range(spawnsPerTick.x, spawnsPerTick.y);

        if (totalEnemies + count < startingCount)
        {
            count = startingCount - totalEnemies;
        }

        GameObject[] result = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = possibleSpawnPrefabs[Random.Range(0, possibleSpawnPrefabs.Length)];
        }

        return result;

    }
}
