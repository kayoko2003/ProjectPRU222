using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    private Rigidbody2D rb;
    private KnockBack knockBack;
    public EnemyScriptableObject enemyData;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();

        PlayerController playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene!");
        }
    }

    void FixedUpdate()
    {
        // Nếu enemy không đang bị knockback, thì mới di chuyển về phía player.
        if (knockBack == null || !knockBack.gettingKnockedBack)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, player.position, enemyData.MoveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
    }
}
