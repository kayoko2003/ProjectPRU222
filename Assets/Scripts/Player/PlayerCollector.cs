using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats playerStats;
    CircleCollider2D playerCollector;
    public float pullSpeed;
    void Start()
    {
        playerStats = Object.FindAnyObjectByType<PlayerStats>();   
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollector.radius = playerStats.CurrentMagnet;    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out ICollecible collecible))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 foreceDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(foreceDirection * pullSpeed);

            collecible.Collect();
        }    
    }
}
