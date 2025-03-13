using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerCollector : MonoBehaviour
{
    PlayerStats playerStats;
    CircleCollider2D detector;
    public float pullSpeed;
    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();   
        detector = GetComponent<CircleCollider2D>();
    }

    public void SetRadius(float radius)
    {
        if (!detector)
        {
            detector = GetComponent<CircleCollider2D>(); 
        }
        detector.radius = radius;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Pickup pickup))
        {
            pickup.Collect(playerStats, pullSpeed);
        }    
    }
}
