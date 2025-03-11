using UnityEngine;

public class ExperienceGem : MonoBehaviour, ICollecible
{
    public int experienceGranted;
    public void Collect()
    {
        PlayerStats player = Object.FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        Destroy(gameObject);
    }
}
