using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    public float damagePerSecond = 1f; // Sát thương mỗi giây
    private HashSet<PlayerStats> playersInLava = new HashSet<PlayerStats>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playersInLava.Add(playerStats);
                StartCoroutine(ApplyLavaDamage(playerStats));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playersInLava.Remove(playerStats);
            }
        }
    }

    private IEnumerator ApplyLavaDamage(PlayerStats playerStats)
    {
        while (playersInLava.Contains(playerStats))
        {
            playerStats.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f); // Mỗi giây trừ máu
        }
    }
}
