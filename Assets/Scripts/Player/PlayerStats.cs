using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

<<<<<<< HEAD
=======
    //Health
    public int maxHealth;
    private SpriteRenderer spriteRenderer;
    private CameraShake cameraShake;
    public HealthBar healthBar;
    public GameObject GameOverUI;
    public bool isDead = false;
    //End Health

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;
>>>>>>> NQThe_

    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }

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

    InventoryManager inventoryManager;
    public int weaponIndex;
    public int passiveItemIndex;

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

<<<<<<< HEAD
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData); 
=======
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        if (GameOverUI == null)
        {
            Debug.LogError("GameOverUI chưa được gán! Hãy kiểm tra trong Inspector.");
        }
        else
        {
            GameOverUI.SetActive(false);
        }

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);
>>>>>>> NQThe_
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
        CharacterSelector.instance.DestroySingleton();

        inventoryManager = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        SpawnWeapon(characterData.StartingWeapon);
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
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
        }

    }


    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            CurrentHealth -= damage;

            if (!isDead && cameraShake != null) // Kiểm tra trước khi rung
            {
                StopAllCoroutines(); // Dừng tất cả coroutine đang chạy
                StartCoroutine(cameraShake.Shake(0.05f, 0.1f));
            }

            FlashRed();
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;

                if (gameObject.CompareTag("Player"))
                {
                    Die();
                    Kill();
                }
                else if (gameObject.CompareTag("Enemy"))
                {
                    Destroy(gameObject, 0.125f);
                }
            }

            if (healthBar != null)
                healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }
    //Hiệu ứng va chạm khi quái tấn công
    private void FlashRed()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashEffect());
        }
    }
    private IEnumerator FlashEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1f);
        spriteRenderer.color = Color.white;
    }
    //End
    private void Die()
    {
        StopAllCoroutines(); // Dừng tất cả coroutine đang chạy, bao gồm camera shake

        if (GameOverUI != null && !GameOverUI.activeSelf)
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0f; // Dừng game
        }

        // Vô hiệu hóa điều khiển nhân vật nếu có PlayerController
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }


    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignchosenWeaponAndPassiveItemUI(inventoryManager.weaponUISlot, inventoryManager.passiveItemUISlot);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float health)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += health;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }

        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponIndex >= inventoryManager.weaponSlots.Count - 1)
        {
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventoryManager.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPasiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventoryManager.passiveItemSlot.Count - 1)
        {
            return;
        }

        GameObject spawnPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnPassiveItem.transform.SetParent(transform);
        inventoryManager.AddPassiveItem(passiveItemIndex, spawnPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}
