using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerController playerS;
    public int minDamage;
    public int maxDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerS = collision.GetComponent<PlayerController>();
            InvokeRepeating("DamagePlayer", 0, 0.1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerS = null;
            CancelInvoke("DamagePlayer");

        }
    }

    void DamagePlayer()
    {
        int damage = UnityEngine.Random.Range(minDamage, maxDamage);
        playerS.TakeDamage(damage);
    }


}
