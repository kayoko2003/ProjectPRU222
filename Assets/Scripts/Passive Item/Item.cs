using UnityEngine;

public class Item : MonoBehaviour
{
    public int currentLevel = 1, maxLevel = 1;

    protected PlayerStats owner;

    public virtual void Intialise(ItemData data)
    {
        maxLevel = data.maxLevel;
        owner = Object.FindAnyObjectByType<PlayerStats>();
    }

    public virtual bool CanLevelUp()
    {
        return currentLevel <= maxLevel;
    }

    public virtual bool DoLevelUp()
    {
        return true;
    }

    public virtual void OnEquip()
    {

    }

    public virtual void OnUnequip()
    {

    }
}
