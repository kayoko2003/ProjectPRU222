using UnityEngine;

public class QuicksandEffect : MonoBehaviour
{
    public float gravityIncrease = 2.0f; // Mức tăng trọng lực khi lún
    public float slowMultiplier = 0.5f; // Hệ số giảm tốc độ (50%)

    private float defaultGravity; // Trọng lực gốc
    private float defaultSpeed; // Tốc độ di chuyển gốc

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (rb != null)
            {
                defaultGravity = rb.gravityScale;
                rb.gravityScale += gravityIncrease; // Tăng trọng lực
            }

            if (playerStats != null)
            {
                defaultSpeed = playerStats.CurrentMoveSpeed;
                playerStats.CurrentMoveSpeed *= slowMultiplier; // Giảm tốc độ
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (rb != null)
            {
                rb.gravityScale = defaultGravity; // Khôi phục trọng lực
            }

            if (playerStats != null)
            {
                playerStats.CurrentMoveSpeed = defaultSpeed; // Khôi phục tốc độ
            }
        }
    }
}
