using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

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

    public List<GameObject> spawnedWeapons;



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

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

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

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

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
            currentHealth -= damage;

            if (!isDead && cameraShake != null) // Kiểm tra trước khi rung
            {
                StopAllCoroutines(); // Dừng tất cả coroutine đang chạy
                StartCoroutine(cameraShake.Shake(0.05f, 0.1f));
            }

            FlashRed();
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
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
        Debug.Log("Player is dead");
    }

    public void RestoreHealth(float health)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += health;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }

        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon);
    }
}
