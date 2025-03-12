using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    Vector3 initinalPosition;
    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();
        initinalPosition = transform.position;
    }

    void Update()
    {
        if (pickup && !pickup.hasBeenCollected)
        {
            transform.position = initinalPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }
}
