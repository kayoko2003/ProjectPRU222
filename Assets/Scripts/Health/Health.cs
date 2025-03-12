using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public float currentHealth;
    public HealthBar healthBar;
    public float safeTimeDuration = 0f;
    private float safeTime;
    public bool isDead = false;

    public GameObject GameOverUI;
    private SpriteRenderer spriteRenderer;
    private CameraShake cameraShake;

    private void Start()
    {
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

    public void TakeDam(float damage)
    {
        if (safeTime > 0 || isDead) return; // Ngăn nhận sát thương nếu đã chết

        currentHealth -= damage;

        if (!isDead && cameraShake != null) // Kiểm tra trước khi rung
        {
            StopAllCoroutines(); // Dừng tất cả coroutine đang chạy
            StartCoroutine(cameraShake.Shake(0.05f, 0.1f));
        }

        FlashRed();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;

            if (gameObject.CompareTag("Player"))
            {
                Die();
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject, 0.125f);
            }
        }

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);

        safeTime = safeTimeDuration;
    }



    private void Die()
    {
        StopAllCoroutines(); // Dừng tất cả coroutine đang chạy, bao gồm camera shake

        if (GameOverUI != null && !GameOverUI.activeSelf)
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0f; // Dừng game
        }

        GetComponent<PlayerController>().enabled = false; // Vô hiệu hóa điều khiển nhân vật
    }


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
}