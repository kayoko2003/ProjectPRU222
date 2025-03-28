using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;

    float health;

    public float CurrentHealth
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}", health, actualStats.maxHealth
                        );
                }
            }
        }
    }

    public float MaxHealth
    {
        get { return actualStats.maxHealth;}

        set
        {
            if(actualStats.maxHealth != value )
            {
                actualStats.maxHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                        );
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return Recovery; }
        set
        {
            Recovery = value;
        }
    }

    public float Recovery
    {
        get { return actualStats.recovery; }
        set
        {
            if (actualStats.recovery != value)
            {
                actualStats.recovery = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return MoveSpeed; }
        set { MoveSpeed = value; }
    }

    public float MoveSpeed 
    {
        get { return actualStats.moveSpeed; }
        set
        {
            if(actualStats.moveSpeed != value)
            {
                actualStats.moveSpeed = value;
                if(GameManager.instance != null )
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + actualStats.moveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return Might; }
        set { Might = value; }
    }

    public float Might
    {
        get { return actualStats.might; }
        set
        {
            if( actualStats.might != value)
            {
                actualStats.might = value;
                if(GameManager.instance != null )
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + actualStats.might;
                }
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return Speed; }
        set { Speed = value; }
    }

    public float Speed
    {
        get { return actualStats.speed; }
        set
        {
            if (actualStats.speed != value)
            {
                actualStats.speed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return Magnet; }
        set { Magnet =  value; }
    }

    public float Magnet
    {
        get { return actualStats.magnet; }
        set
        {
            if(actualStats.magnet != value)
            {
                actualStats.magnet = value;
                if(GameManager.instance != null )
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + actualStats.magnet;
                }
            }
        }
    }


    public ParticleSystem damageEffect;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    PlayerInventory inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;

    PlayerAnimator playerAnimator;

    void Start()
    {
        inventory.Add(characterData.StartingWeapon);

        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health: " + CurrentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + CurrentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + CurrentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + CurrentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + CurrentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }
    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
        Recover();
    }

    private void Awake()
    {
        characterData = CharacterSelector.GetData();

        if(CharacterSelector.instance) CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<PlayerInventory>();

        baseStats = actualStats = characterData.stats;
        health = actualStats.maxHealth;
        playerAnimator = GetComponent<PlayerAnimator>();
        if(characterData.controller)
            playerAnimator.SetAnimatorController(characterData.controller);
    }

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualStats += p.GetBoosts(); 
            }
        }
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();

        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange levelRange in levelRanges)
            {
                if(level >= levelRange.startLevel && level <= levelRange.endLevel)
                {
                    experienceCapIncrease = levelRange.experienceCapIncrease;
                }
            }
            experienceCap += experienceCapIncrease;
            GameManager.instance.StartLevelUp();
        }

        UpdateLevelText();
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float) experience / experienceCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "Level " + level.ToString();
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            CurrentHealth -= damage;

            if (damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = CurrentHealth / actualStats.maxHealth;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignchosenWeaponAndPassiveItemUI(inventory.weaponSlots, inventory.passiveSlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float health)
    {
        if (CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += health;
            if (CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
        UpdateHealthBar();
    }

    void Recover()
    {
        if(CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if(CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
            UpdateHealthBar();
        }
        
    }

    [System.Obsolete("Olb function that is kept to maintain compatibility with the InventoryManager. Will be removed soon.")]
    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        //inventoryManager.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    [System.Obsolete("No need to spawn passive items directly now.")]
    public void SpawnPasiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveSlots.Count - 1)
        {
            return;
        }

        GameObject spawnPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnPassiveItem.transform.SetParent(transform);
        //inventoryManager.AddPassiveItem(passiveItemIndex, spawnPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}
