using UnityEngine;

public class ExperienceGem : Pickup
{
    public int experienceGranted;
    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }

        PlayerStats player = Object.FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
