using UnityEngine;

[CreateAssetMenu(fileName = "Passive Data", menuName = "Game Survive/Passive Data")]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int level)
    {
        if(level - 2 < growth.Length)
            return growth[level - 2];

        return new Passive.Modifier();
    }
}
