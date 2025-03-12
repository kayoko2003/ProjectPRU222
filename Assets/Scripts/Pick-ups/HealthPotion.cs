using UnityEngine;

public class HealthPotion : Pickup
{
    public int healthToRestore;
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

        PlayerStats playerStats = Object.FindAnyObjectByType<PlayerStats>();
        playerStats.RestoreHealth(healthToRestore);
    }

}
