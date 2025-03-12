using UnityEngine;

public class HealthPotion : Pickup, ICollecible
{
    public int healthToRestore;
    public void Collect()
    {
        PlayerStats playerStats = Object.FindAnyObjectByType<PlayerStats>();
        playerStats.RestoreHealth(healthToRestore);
    }

}
