using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Game Survive/Weapon Data")]
public class WeaponData : ItemData
{

    [HideInInspector] public string behaviour;
    public Weapon.Starts baseStarts;
    public Weapon.Starts[] linearGrowth;
    public Weapon.Starts[] randomGrowth;

    public Weapon.Starts GetLevelData(int level)
    {
        if (level - 2 < linearGrowth.Length)
        {
            return linearGrowth[level - 2];
        }

        if (randomGrowth.Length > 0)
        {
            return randomGrowth[Random.Range(0, randomGrowth.Length)];
        }

        return new Weapon.Starts();
    }
}
