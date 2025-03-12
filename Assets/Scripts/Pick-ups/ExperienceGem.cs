using UnityEngine;

public class ExperienceGem : Pickup, ICollecible
{
    public int experienceGranted;
    public void Collect()
    {
        PlayerStats player = Object.FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
