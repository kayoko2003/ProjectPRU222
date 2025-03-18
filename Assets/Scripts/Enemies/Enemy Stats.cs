﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public float currentMoveSpeed;
    public float currentHealth;
    public float currentDamage;

    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    public static int count;

    void Awake()
    {
        count++;
    }

    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerStats>().transform; 
        
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        movement = GetComponent<EnemyMovement>();
    }

    public void TakeDame(float dmg, Vector2 sourcePosition, float knockbackFore = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        if (dmg > 0) 
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        if(knockbackFore > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;

            if (dir.magnitude < 0.1f)
            {
                dir = Random.insideUnitCircle.normalized;
            }

            movement.KnockBack(dir.normalized * knockbackFore, knockbackDuration);
        }


        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void Kill()
    {
        DropRateManager drops = GetComponent<DropRateManager>();
        if (drops)
        {
            drops.active = true;
        }

        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {

        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        while (t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        count--;
    }

}
