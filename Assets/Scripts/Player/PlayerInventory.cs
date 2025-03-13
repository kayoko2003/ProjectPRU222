using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Item item;
        public Image image;

        public void Assign(Item assignedItem)
        {
            item = assignedItem;
            if(item is Weapon)
            {
                Weapon weapon = item as Weapon;
                image.enabled = true;
                image.sprite = weapon.data.icon;
            }
            else
            {
                Passive p = item as Passive;
                image.enabled = true;
                image.sprite = p.data.icon;
            }
        }

        public void Clear()
        {
            item = null;
            image.enabled = false;
            image.sprite = null;
        }

        public bool IsEmpty() { return item == null; }
    }

    public List<Slot> weaponSlots = new List<Slot>(6);
    public List<Slot> passiveSlots = new List<Slot>(6);

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    [Header("UI Element")]
    public List<WeaponData> availableWeapons = new List<WeaponData>();
    public List<PassiveData> availablePassives = new List<PassiveData>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public bool Has(ItemData type) { return Get(type); }

    public Item Get(ItemData type) 
    {
        if(type is WeaponData) return Get(type as WeaponData);
        else if(type is PassiveData) return Get(type as PassiveData);
        return null;
    }

    public Passive Get(PassiveData type)
    {
        foreach(Slot s in passiveSlots)
        {
            Passive p = s.item as Passive;
            if(p.data == type) return p;
        }
        return null;
    }

    public Weapon Get(WeaponData type)
    {
        foreach(Slot s in weaponSlots)
        {
            Weapon w = s.item as Weapon;
            if(w.data == type) return w;
        }
        return null;
    }

    public bool Remove(WeaponData data, bool removeUpgradeAvailabilty = false)
    {
        if(removeUpgradeAvailabilty) availableWeapons.Remove(data);

        for(int i = 0; i < weaponSlots.Count; i++)
        {
            Weapon w = weaponSlots[i].item as Weapon;
            if(w.data == data)
            {
                weaponSlots[i].Clear();
                w.OnUnequip();
                Destroy(w.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool Remove(PassiveData data, bool removeUpgradeAvailabilty = false)
    {
        if (removeUpgradeAvailabilty) availablePassives.Remove(data);

        for (int i = 0; i < passiveSlots.Count; i++)
        {
            Passive p = passiveSlots[i].item as Passive;
            if (p.data == data)
            {
                weaponSlots[i].Clear();
                p.OnUnequip();
                Destroy(p.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool Remove(ItemData data, bool removeUpgradeAvailabilty = false)
    {
        if(data is PassiveData) return Remove(data as PassiveData, removeUpgradeAvailabilty);
        else if(data is WeaponData) return Remove(data as WeaponData, removeUpgradeAvailabilty);
        return false;
    }

    public int Add(WeaponData data)
    {
        int slotNum = -1;

        for(int i = 0;i < weaponSlots.Capacity; i++)
        {
            if (weaponSlots[i].IsEmpty())
            {
                slotNum = i; break;
            }
        }

        if(slotNum < 0) return slotNum;

        Type weaponType = Type.GetType(data.behaviour);

        if(weaponType != null)
        {
            GameObject go = new GameObject(data.baseStarts.name + " Controller");
            Weapon spawnedWeapon = (Weapon)go.AddComponent(weaponType);
            spawnedWeapon.Initialise(data);
            spawnedWeapon.transform.SetParent(transform);
            spawnedWeapon.transform.localPosition = Vector2.zero;
            spawnedWeapon.OnEquip();

            weaponSlots[slotNum].Assign(spawnedWeapon);

            if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
                GameManager.instance.EndLevelUp();
            return slotNum;
        }
        else
        {
            Debug.LogWarning(string.Format("Invalid weapon {0}", data.name));
        }
        return -1;
    }

    public int Add(PassiveData data)
    {
        int slotNum = -1;

        for (int i = 0; i < passiveSlots.Capacity; i++)
        {
            if (passiveSlots[i].IsEmpty())
            {
                slotNum = i; break;
            }
        }

        if (slotNum < 0) return slotNum;

        GameObject go = new GameObject(data.baseStats.name + " Controller");
        Passive p = go.AddComponent<Passive>();
        p.Initialise(data);
        p.transform.SetParent(transform);
        p.transform.localPosition = Vector2.zero;

        passiveSlots[slotNum].Assign(p);

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
        player.RecalculateStats();

        return slotNum;
          
    }

    public int Add(ItemData data)
    {
        if(data is WeaponData) return Add(data as WeaponData);
        else if(data is PassiveData) return Add(data as PassiveData);
        return -1;
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            Weapon weapon = weaponSlots[slotIndex].item as Weapon;

            if (!weapon.DoLevelUp())
            {
                return;
            }
        }

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveSlots.Count > slotIndex)
        {
            Passive passive = passiveSlots[slotIndex].item as Passive;

            if (!passive.DoLevelUp())
            {
                return;
            }
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }

        player.RecalculateStats();
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponData> availableWeaponUpgrades = new List<WeaponData> (availableWeapons);
        List<PassiveData> availablePassiveItemUpgrade = new List<PassiveData>(availablePassives);

        foreach(UpgradeUI upgradeOption in upgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrade.Count == 0)
            {
                return;
            }

            int upgradeType;
            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrade.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = UnityEngine.Random.Range(1, 3);
            }

            if (upgradeType == 1)
            {
                WeaponData chosenWeaponUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnebleUpgradeUI(upgradeOption);

                    bool isLevelUp = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        Weapon w = weaponSlots[i].item as Weapon;
                        if(w != null && w.data == chosenWeaponUpgrade)
                        {
                            if(chosenWeaponUpgrade.maxLevel <= w.currentLevel)
                            {
                                isLevelUp = false ;
                                break;
                            }
                        }

                        upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));
                        Weapon.Starts nextLevel = chosenWeaponUpgrade.GetLevelData(w.currentLevel + 1);
                        upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                        upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                        isLevelUp = true;
                        break;
                    }

                    if(!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenWeaponUpgrade));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.baseStarts.description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.baseStarts.name;
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                    }
                }
            }
            else if(upgradeType == 2)
            {
                PassiveData chosenPassiveUpgrade = availablePassiveItemUpgrade[UnityEngine.Random.Range(0, availablePassiveItemUpgrade.Count)];
                availablePassiveItemUpgrade.Remove(chosenPassiveUpgrade);

                if(chosenPassiveUpgrade != null)
                {
                    EnebleUpgradeUI(upgradeOption);

                    bool isLevelUp = false;
                    for(int i = 0; i< passiveSlots.Count; i++)
                    {
                        Passive p = passiveSlots[i].item as Passive;
                        if(p != null && p.data == chosenPassiveUpgrade)
                        {
                            if(chosenPassiveUpgrade.maxLevel <= p.currentLevel)
                            {
                                isLevelUp = false;
                                break;
                            }
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, i));
                            Passive.Modifier nextLevel = chosenPassiveUpgrade.GetLevelData(p.currentLevel + 1);
                            upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenPassiveUpgrade));
                        Passive.Modifier nextLevel = chosenPassiveUpgrade.baseStats;
                        upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                        upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                        upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                    }
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (UpgradeUI upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnebleUpgradeUI(UpgradeUI upgradeUI )
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
