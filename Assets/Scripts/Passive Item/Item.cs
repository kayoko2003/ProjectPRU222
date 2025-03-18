using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int currentLevel = 1, maxLevel = 1;
    protected ItemData.Evolution[] evolutionData;
    protected PlayerInventory inventory;
    protected PlayerStats owner;

    public PlayerStats Owner { get { return owner; } }
    public virtual void Intialise(ItemData data)
    {
        maxLevel = data.maxLevel;

        evolutionData = data.evolutionData;

        inventory = Object.FindAnyObjectByType<PlayerInventory>();
        owner = Object.FindAnyObjectByType<PlayerStats>();
    }

    public virtual ItemData.Evolution[] CanEvole()
    {
        List<ItemData.Evolution> possibleEvolutions = new List<ItemData.Evolution>();

        foreach (ItemData.Evolution e in evolutionData)
        {
            if(CanEvolve(e)) possibleEvolutions.Add(e);
        }

        return possibleEvolutions.ToArray();
    }

    public virtual bool CanEvolve(ItemData.Evolution evolution, int levelUpAmount = 1)
    {
        if(evolution.evolutionLevel > currentLevel + levelUpAmount)
        {
            return false;
        }

        foreach(ItemData.Evolution.Config c in evolution.catalyst)
        {
            Item item = inventory.Get(c.itemType);
            if(!item || item.currentLevel < c.level)
            {
                return false;
            }
        } 

        return true;
    }

    public virtual bool AttemptEvolution(ItemData.Evolution evolutionData, int levelUpAmount = 1)
    {
        if (!CanEvolve(evolutionData, levelUpAmount))
        {
            return false;
        }

        bool consumePassives = (evolutionData.consumes & ItemData.Evolution.Consumption.passives) > 0;
        bool comsumeWeapons = (evolutionData.consumes & ItemData.Evolution.Consumption.weapon) > 0;

        foreach (ItemData.Evolution.Config c in evolutionData.catalyst)
        {
            if (c.itemType is PassiveData && consumePassives) inventory.Remove(c.itemType, true);
            if (c.itemType is WeaponData && comsumeWeapons) inventory.Remove(c.itemType, true);
        }

        if(this is Passive && consumePassives) inventory.Remove((this as Passive).data, true);
        else if (this is Weapon && comsumeWeapons) inventory.Remove((this as Weapon).data, true);

        inventory.Add(evolutionData.outcome.itemType);

        return true; 
    }

    public virtual bool CanLevelUp()
    {
        return currentLevel <= maxLevel;
    }

    public virtual bool DoLevelUp()
    {
        if(evolutionData == null) return true;

        foreach (ItemData.Evolution e in evolutionData)
        {
            if (e.condition == ItemData.Evolution.Condition.auto)
            {
                AttemptEvolution(e);
            }
        } 

        return true;
    }

    public virtual void OnEquip()
    {

    }

    public virtual void OnUnequip()
    {

    }
}
